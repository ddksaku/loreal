IF OBJECT_ID ('tr_CustomerAllocation_CheckFixedAgainstAllocationQuantity', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerAllocation_CheckFixedAgainstAllocationQuantity;
GO

CREATE TRIGGER tr_CustomerAllocation_CheckFixedAgainstAllocationQuantity
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
		DECLARE @itemTypeID uniqueidentifier
		DECLARE @groupID uniqueidentifier
		DECLARE @customerID uniqueidentifier
		DECLARE @multiple int
		DECLARE @errorMsg nvarchar(max)
		DECLARE @allocationQuantity int
		DECLARE @sumOfFixed int		

		SELECT @customerID = IDCustomer FROM inserted
		SELECT @animationProductDetailId = IDAnimationProductDetail FROM inserted
		SELECT @animationProductId = IDAnimationProduct FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId
		SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId	
		SELECT @groupID = IDCustomerGroup FROM Customer WHERE ID = @customerID
		SELECT @itemTypeID = IDItemType FROM dbo.AnimationProduct WHERE ID = @animationProductId
		SELECT @allocationQuantity = AllocationQuantity FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId

		-- get customer groups 
		DECLARE @groups TABLE (groupID uniqueidentifier)

		INSERT INTO @groups 
			SELECT IDCustomerGroup FROM dbo.AnimationCustomerGroup WHERE IDAnimation = @animationID

		-- get customers and their capaties
		DECLARE @customers TABLE (customerID uniqueidentifier, fixed int)

		INSERT INTO @customers (customerID)
			SELECT ID FROM Customer
				WHERE IDCustomerGroup IN (SELECT groupID FROM @groups)

		-- update their capacities
		UPDATE @customers SET fixed = b.FixedAllocation
			FROM @customers as a inner join dbo.CustomerAllocation as b 
				ON (a.customerID = b.IDCustomer AND b.IDAnimationProductDetail = @animationProductDetailId)

		-- sum of all stores capacities
		SELECT @sumOfFixed = SUM(ISNULL(fixed,0)) FROM @customers
		set @sumOfFixed = isnull(@sumOfFixed, 0)
		
		IF @allocationQuantity < @sumOfFixed
		BEGIN
			/*SET @errorMsg = 'Sum of fixed allocations exceeds allocation quantity' + @newLineChar
			 + '  Quantity: ' + cast(@allocationQuantity as nvarchar(50)) + @newLineChar
			 + '  Sum of Fixed: ' + cast(@sumOfFixed as nvarchar(50)) */
			SET @errorMsg = dbo.uf_getSystemMessage('tr_CustomerAllocation_CheckFixedAgainstAllocationQuantity', cast(@allocationQuantity as nvarchar(50)), cast(@sumOfFixed as nvarchar(50)), null, null, null, null, null, null)
			RAISERROR (@errorMsg, 16, 5)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END