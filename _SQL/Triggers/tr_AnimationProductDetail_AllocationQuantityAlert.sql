IF OBJECT_ID ('tr_AnimationProductDetail_AllocationQuantityAlert', 'TR') IS NOT NULL
   DROP TRIGGER tr_AnimationProductDetail_AllocationQuantityAlert;
GO

CREATE TRIGGER tr_AnimationProductDetail_AllocationQuantityAlert
ON AnimationProductDetail
AFTER UPDATE
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @animationProductId uniqueidentifier
	DECLARE @animationId UNIQUEIDENTIFIER
	DECLARE @animationName NVARCHAR(100)
	DECLARE @animationCode NVARCHAR(20)
	DECLARE @productID UNIQUEIDENTIFIER
	DECLARE @productName NVARCHAR(200)
	DECLARE @productCode NVARCHAR(20)	
	DECLARE @animationProductDetailID UNIQUEIDENTIFIER
	DECLARE @divisionId UNIQUEIDENTIFIER
	DECLARE @allocationQuantityOld INT
	DECLARE @allocationQuantityNew INT
	DECLARE @animationStatus INT	
	DECLARE @modifiedBy NVARCHAR(255)
	
	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)
	
	SELECT @modifiedBy = ModifiedBy FROM inserted
	SELECT @allocationQuantityOld = AllocationQuantity FROM DELETED
	SELECT @allocationQuantityNew = AllocationQuantity FROM INSERTED
	
	SELECT @animationProductId = IDAnimationProduct FROM INSERTED
	SELECT @animationId = IDAnimation FROM dbo.AnimationProduct

	SELECT @divisionId = IDDivision, @animationStatus = a.[Status], @animationName = a.Name, @animationCode = a.Code 
		FROM dbo.Animation AS a 		
		 WHERE a.ID = @animationID
	
	
	-- h.	Allocation quantity changes during the draft, published or closed allocation 
	IF @animationStatus = 3 AND @allocationQuantityOld <> @allocationQuantityNew
	BEGIN
		
		SELECT @productID = IDProduct FROM dbo.AnimationProduct WHERE ID = @animationProductId
		SELECT @productName = [Description], @productCode = [MaterialCode] FROM dbo.Product WHERE ID = @productID
		SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId
		
		DECLARE @alertMessage NVARCHAR(max)
		SET @alertMessage = dbo.uf_getSystemMessage('tr_AnimationProductDetail_AllocationQuantityAlert', @animationName, @animationCode, @productName, @productCode, null, null, null, null)
		/*SET @alertMessage = 'Allocation quantity has been changed' + @newLineChar + 
							'Animation: ' + @animationName + ' (' + @animationCode + ')' + @newLineChar +
							'Product: ' + @productName + ' (' + @productCode + ')'*/
							
		INSERT INTO dbo.AuditAlert
		        ( 
		          AlertType ,
		          AlertDescription ,
		          DateCreated ,
		          IDDivision ,
		          OriginalValue ,
		          NewValue ,
		          Processed	,
		          ModifiedBy	          
		        )
		VALUES  ( 
		          'Allocation Quantity'
		          ,@alertMessage
		          ,GETDATE()
		          ,@divisionId
		          ,@allocationQuantityOld
		          ,@allocationQuantityNew
		          ,0
		          ,@modifiedBy
		        )	
	
	END	 
	
	RETURN
END