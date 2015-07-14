IF OBJECT_ID ('tr_CustomerAllocation_DivisibilityOfFixedAllocation', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerAllocation_DivisibilityOfFixedAllocation;
GO

CREATE TRIGGER tr_CustomerAllocation_DivisibilityOfFixedAllocation
ON CustomerAllocation
AFTER UPDATE, INSERT 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @fixedAllocation int
	SELECT @fixedAllocation = FixedAllocation FROM inserted

	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)

	IF @fixedAllocation IS NOT NULL
	BEGIN
		
		DECLARE @animationProductDetailId uniqueidentifier
		DECLARE @animationProductId uniqueidentifier
		DECLARE @animationId uniqueidentifier
		DECLARE @multipleID uniqueidentifier
		DECLARE @multiple int
		DECLARE @errorMsg nvarchar(max)

		SELECT @animationProductDetailId = IDAnimationProductDetail FROM inserted
		SELECT @animationProductId = IDAnimationProduct FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId
		SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId
		SELECT @multipleID = IDMultipleNormal FROM dbo.AnimationProduct WHERE ID = @animationProductId
		SELECT @multiple = Value FROM dbo.Multiple WHERE ID = @multipleID
		
		IF @fixedAllocation % @multiple > 0
		BEGIN
			/*SET @errorMsg = 'Fixed quantity is not divisible by product multiple' + @newLineChar
			 + '  Fixed: ' + cast(@fixedAllocation as nvarchar(50)) + @newLineChar
			 + '  Multiple: ' + cast(@multiple as nvarchar(50))*/
			SET  @errorMsg = dbo.uf_getSystemMessage('tr_CustomerAllocation_DivisibilityOfFixedAllocation', cast(@fixedAllocation as nvarchar(50)), cast(@multiple as nvarchar(50)), null, null, null, null, null, null)
			RAISERROR (@errorMsg, 16, 3)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END