IF OBJECT_ID ('tr_CustomerAllocation_CheckFixedAgainstCapacity', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerAllocation_CheckFixedAgainstCapacity;
GO

CREATE TRIGGER tr_CustomerAllocation_CheckFixedAgainstCapacity
ON CustomerAllocation
AFTER UPDATE, INSERT 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @fixedAllocation int
	DECLARE @errorMsg nvarchar(max)
	SELECT @fixedAllocation = FixedAllocation FROM inserted
	
	DECLARE @customerAllocationID uniqueidentifier
	SELECT @customerAllocationID = ID from inserted

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
		DECLARE @capacityWithinAllStores int
		DECLARE @sumOfFixed int		
		DECLARE @animationTypeID uniqueidentifier	
		DECLARE @priorityID uniqueidentifier	

	
		SELECT @customerID = IDCustomer FROM inserted
		SELECT @animationProductDetailId = IDAnimationProductDetail FROM inserted
		SELECT @animationProductId = IDAnimationProduct FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId
		SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId	
		SELECT @animationTypeID = IDAnimationType FROM dbo.Animation WHERE ID = @animationID
		SELECT @priorityID = IDPriority FROM dbo.Animation WHERE ID = @animationID
		SELECT @groupID = IDCustomerGroup FROM Customer WHERE ID = @customerID
		SELECT @itemTypeID = IDItemType FROM dbo.AnimationProduct WHERE ID = @animationProductId
		
		/*
		-- get customer groups 
		DECLARE @groups TABLE (groupID uniqueidentifier, fixed int)

		INSERT INTO @groups (groupID)
			SELECT IDCustomerGroup FROM dbo.AnimationCustomerGroup WHERE IDAnimation = @animationID
			
			
		UPDATE @groups SET fixed = b.ManualFixedAllocation
			FROM @groups as a inner join dbo.CustomerGroupAllocation as b on (a.groupID = b.IDCustomerGroup
				AND b.IDAnimationProductDetail = @animationProductDetailId)	
			
		SELECT @sumOfFixed = sum(isnull(fixed, 0)) from @groups
		*/

		/*
		-- get customers and their capaties
		DECLARE @customers TABLE (customerID uniqueidentifier, capacity int)

		INSERT INTO @customers (customerID)
			SELECT ID FROM Customer
				WHERE IDCustomerGroup IN (SELECT groupID FROM @groups)

		-- update their capacities
		/*UPDATE @customers SET capacity = b.Capacity
			FROM @customers as a inner join dbo.CustomerCapacity as b 
				ON (a.customerID = b.IDCustomer AND b.IDAnimationType = @animationTypeID AND b.IDPriority = @priorityID
					AND b.IDItemType = @itemTypeId)*/
		UPDATE @customers SET capacity = dbo.calculate_ProductDetailCustomerTotalCapacity(@animationProductDetailId, customerID)			
					

		-- sum of all stores capacities
		SELECT @capacityWithinAllStores = SUM(ISNULL(capacity,0)) FROM @customers	*/
		
		
		SELECT @capacityWithinAllStores = dbo.calculate_TotalCapacityCustomer(@customerAllocationID)
		set @capacityWithinAllStores = isnull(@capacityWithinAllStores, 0)

		IF @capacityWithinAllStores < @fixedAllocation
		BEGIN
			/*SET @errorMsg = 'Sum of fixed allocations exceeds maximum capacity' + @newLineChar
			 + '  Capacity: ' + cast(@capacityWithinAllStores as nvarchar(50)) + @newLineChar
			 + '  Sum of Fixed: ' + cast(@sumOfFixed as nvarchar(50)) */
			SET @errorMsg = dbo.uf_getSystemMessage('tr_CustomerAllocation_CheckFixedAgainstCapacity', cast(@capacityWithinAllStores as nvarchar(50)), cast(@fixedAllocation as nvarchar(50)) , null, null, null, null, null, null )
			RAISERROR (@errorMsg, 16, 8)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END