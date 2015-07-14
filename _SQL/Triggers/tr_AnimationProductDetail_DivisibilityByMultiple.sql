IF OBJECT_ID ('tr_AnimationProductDetail_DivisibilityByMultiple', 'TR') IS NOT NULL
   DROP TRIGGER tr_AnimationProductDetail_DivisibilityByMultiple;
GO

CREATE TRIGGER tr_AnimationProductDetail_DivisibilityByMultiple
ON AnimationProductDetail
AFTER UPDATE 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @animationProductId uniqueidentifier
	DECLARE @animationId uniqueidentifier
	DECLARE @animationProductDetailID uniqueidentifier	
	DECLARE @allocationQuantity int
	DECLARE @errorMsg nvarchar(max)
	DECLARE @multipleNormalID uniqueidentifier
	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)
	
	SELECT @allocationQuantity = AllocationQuantity FROM inserted
	SELECT @animationProductId = IDAnimationProduct FROM inserted
	SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId
	
	SELECT @multipleNormalID = IDMultipleNormal FROM dbo.AnimationProduct WHERE ID = @animationProductId
	
	IF @multipleNormalID IS NULL
	BEGIN	
		RETURN
	END
	ELSE
	BEGIN
		DECLARE @multipleValue int
		SELECT @multipleValue = Value FROM Multiple WHERE ID = @multipleNormalID

		IF @allocationQuantity % @multipleValue > 0
		BEGIN
			/*SET @errorMsg = 'Allocation quantity is not divisible by product multiple' + @newLineChar
			 + '  Quantity: ' + cast(@allocationQuantity as nvarchar(50)) + @newLineChar
			 + '  Multiple: ' + cast(@multipleValue as nvarchar(50))*/
			SET @errorMsg =  dbo.uf_getSystemMessage('tr_AnimationProductDetail_DivisibilityByMultiple', cast(@allocationQuantity as nvarchar(50)), cast(@multipleValue as nvarchar(50)), null, null, null, null, null, null)
			RAISERROR (@errorMsg, 16, 2)
			ROLLBACK TRAN
			RETURN	
		END
	END	
	RETURN
END