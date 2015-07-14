IF OBJECT_ID ('tr_CustomerGroupAllocation_CheckFixedAgainstAllocationQuantity', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerGroupAllocation_CheckFixedAgainstAllocationQuantity;
GO

CREATE TRIGGER tr_CustomerGroupAllocation_CheckFixedAgainstAllocationQuantity
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
		DECLARE @allocationQuantity int
		DECLARE @sumOfFixed int		
		
		SELECT @animationProductDetailId = IDAnimationProductDetail FROM inserted
		SELECT @animationProductId = IDAnimationProduct FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId
		SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId	
		SELECT @groupID = IDCustomerGroup FROM inserted
		SELECT @itemTypeID = IDItemType FROM dbo.AnimationProduct WHERE ID = @animationProductId
		SELECT @allocationQuantity = AllocationQuantity FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId

		-- get customer groups 
		DECLARE @groups TABLE (groupID uniqueidentifier, fixed int)

		INSERT INTO @groups (groupID)
			SELECT IDCustomerGroup FROM dbo.AnimationCustomerGroup WHERE IDAnimation = @animationID

		
		-- update their capacities
		UPDATE @groups SET fixed = b.ManualFixedAllocation
			FROM @groups as a inner join dbo.CustomerGroupAllocation as b 
				ON (a.groupID = b.IDCustomerGroup AND b.IDAnimationProductDetail = @animationProductDetailId)

		-- sum of all stores capacities
		SELECT @sumOfFixed = SUM(ISNULL(fixed,0)) FROM @groups
		
		IF @allocationQuantity < @sumOfFixed
		BEGIN
			/*SET @errorMsg = 'Sum of group fixed allocations exceeds allocation quantity' + @newLineChar
			 + '  Quantity: ' + cast(@allocationQuantity as nvarchar(50)) + @newLineChar
			 + '  Sum of Fixed: ' + cast(@sumOfFixed as nvarchar(50)) */
			SET @errorMsg = dbo.uf_getSystemMessage('tr_CustomerGroupAllocation_CheckFixedAgainstAllocationQuantity', cast(@allocationQuantity as nvarchar(50)), cast(@sumOfFixed as nvarchar(50)) , null, null, null, null, null, null)
			RAISERROR (@errorMsg, 16, 6)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END