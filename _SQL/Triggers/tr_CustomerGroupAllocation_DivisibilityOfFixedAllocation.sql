IF OBJECT_ID ('tr_CustomerGroupAllocation_DivisibilityOfFixedAllocation', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerGroupAllocation_DivisibilityOfFixedAllocation;
GO

CREATE TRIGGER tr_CustomerGroupAllocation_DivisibilityOfFixedAllocation
ON CustomerGroupAllocation
AFTER UPDATE, INSERT 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @fixedAllocation int
	SELECT @fixedAllocation = ManualFixedAllocation FROM inserted

	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)

	IF @fixedAllocation IS NOT NULL
	BEGIN
		
		DECLARE @animationProductDetailId uniqueidentifier
		DECLARE @animationProductId uniqueidentifier
		DECLARE @animationId uniqueidentifier
		DECLARE @multipleID uniqueidentifier
		DECLARE @itemTypeID uniqueidentifier
		DECLARE @groupID uniqueidentifier
		DECLARE @multiple int
		DECLARE @errorMsg nvarchar(max)
		DECLARE @isWarehouse bit

		SELECT @animationProductDetailId = IDAnimationProductDetail FROM inserted
		SELECT @animationProductId = IDAnimationProduct FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId
		SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId	
		SELECT @groupID = IDCustomerGroup FROM inserted
		SELECT @itemTypeID = IDItemType FROM dbo.AnimationProduct WHERE ID = @animationProductId

		SELECT @isWarehouse = ISNULL(WarehouseAllocation, 0) FROM dbo.CustomerGroupItemType 
			WHERE IDCustomerGroup = @groupId AND IDItemType = @itemTypeID

		IF @isWarehouse = 1
		BEGIN
			SELECT @multipleID = IDMutlipleWarehouse FROM dbo.AnimationProduct WHERE ID = @animationProductId
		END
		ELSE
		BEGIN
			SELECT @multipleID = IDMultipleNormal FROM dbo.AnimationProduct WHERE ID = @animationProductId
		END
		SELECT @multiple = Value FROM dbo.Multiple WHERE ID = @multipleID
		
		IF @fixedAllocation % @multiple > 0
		BEGIN
			
				/*SET @errorMsg = 'Fixed quantity is not divisible by product multiple' + @newLineChar
				 + '  Fixed: ' + cast(@fixedAllocation as nvarchar(50)) + @newLineChar
				 + '  Multiple: ' + cast(@multiple as nvarchar(50)) + CASE WHEN @isWarehouse = 1 THEN ' (Warehouse account)' ELSE ' (Normal account)' END */
			SET @errorMsg = dbo.uf_getSystemMessage('tr_CustomerGroupAllocation_DivisibilityOfFixedAllocation', cast(@fixedAllocation as nvarchar(50)), cast(@multiple as nvarchar(50)), null, null, null, null, null, null) + CASE WHEN @isWarehouse = 1 THEN ' (Warehouse account)' ELSE ' (Normal account)' END
			
			RAISERROR (@errorMsg, 16, 4)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END