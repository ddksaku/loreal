IF OBJECT_ID ('tr_AnimationProductDetail_AllocationQuantity', 'TR') IS NOT NULL
   DROP TRIGGER tr_AnimationProductDetail_AllocationQuantity;
GO

CREATE TRIGGER tr_AnimationProductDetail_AllocationQuantity
ON AnimationProductDetail
AFTER UPDATE, INSERT 
AS 
BEGIN
	DECLARE @animationProductId uniqueidentifier
	DECLARE @animationId uniqueidentifier
	DECLARE @animationProductDetailID uniqueidentifier
	DECLARE @animationTypeID uniqueidentifier
	DECLARE @priorityID uniqueidentifier
	DECLARE @itemTypeID uniqueidentifier
	DECLARE @capacityWithinAllStores int
	DECLARE @allocationQuantity int
	DECLARE @errorMsg nvarchar(max)
	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)
	
	SELECT @allocationQuantity = AllocationQuantity FROM inserted
	SELECT @animationProductId = IDAnimationProduct FROM inserted
	SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId

	SELECT @animationTypeID = IDAnimationType FROM dbo.Animation WHERE ID = @animationID
	SELECT @priorityID = IDPriority FROM dbo.Animation WHERE ID = @animationID
	SELECT @itemTypeID = IDItemType FROM dbo.AnimationProduct WHERE ID = @animationProductId

	-- get customer groups 
	DECLARE @groups TABLE (groupID uniqueidentifier)

	INSERT INTO @groups 
		SELECT IDCustomerGroup FROM dbo.AnimationCustomerGroup WHERE IDAnimation = @animationID

	-- get customers and their capaties
	DECLARE @customers TABLE (customerID uniqueidentifier, capacity int)

	INSERT INTO @customers (customerID)
		SELECT ID FROM Customer
			WHERE IDCustomerGroup IN (SELECT groupID FROM @groups)

	-- update their capacities
	UPDATE @customers SET capacity = b.Capacity
		FROM @customers as a inner join dbo.CustomerCapacity as b 
			ON (a.customerID = b.IDCustomer AND b.IDAnimationType = @animationTypeID AND b.IDPriority = @priorityID
				AND b.IDItemType = @itemTypeId)

	-- sum of all stores capacities
	SELECT @capacityWithinAllStores = SUM(ISNULL(capacity,0)) FROM @customers
	
	IF @allocationQuantity > @capacityWithinAllStores
	BEGIN
		SET @errorMsg = 'Allocation quantity exceeds maximum capacity' + @newLineChar
			 + '  Quantity: ' + cast(@allocationQuantity as nvarchar(50)) + @newLineChar
			 + '  Capacity: ' + cast(@capacityWithinAllStores as nvarchar(50))
		RAISERROR (@errorMsg, 16, 1)
		ROLLBACK TRAN
		RETURN			
	END
	ELSE	
	RETURN
END

GO

IF OBJECT_ID ('tr_AnimationProductDetail_DivisibilityByMultiple', 'TR') IS NOT NULL
   DROP TRIGGER tr_AnimationProductDetail_DivisibilityByMultiple;
GO

CREATE TRIGGER tr_AnimationProductDetail_DivisibilityByMultiple
ON AnimationProductDetail
AFTER UPDATE, INSERT 
AS 
BEGIN
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
			SET @errorMsg = 'Allocation quantity is not divisible by product multiple' + @newLineChar
			 + '  Quantity: ' + cast(@allocationQuantity as nvarchar(50)) + @newLineChar
			 + '  Multiple: ' + cast(@multipleValue as nvarchar(50))
			RAISERROR (@errorMsg, 16, 2)
			ROLLBACK TRAN
			RETURN	
		END
	END	
	RETURN
END

GO

IF OBJECT_ID ('tr_CustomerAllocation_DivisibilityOfFixedAllocation', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerAllocation_DivisibilityOfFixedAllocation;
GO

CREATE TRIGGER tr_CustomerAllocation_DivisibilityOfFixedAllocation
ON CustomerAllocation
AFTER UPDATE, INSERT 
AS 
BEGIN
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
			SET @errorMsg = 'Fixed quantity is not divisible by product multiple' + @newLineChar
			 + '  Fixed: ' + cast(@fixedAllocation as nvarchar(50)) + @newLineChar
			 + '  Multiple: ' + cast(@multiple as nvarchar(50))
			RAISERROR (@errorMsg, 16, 3)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END

GO

IF OBJECT_ID ('tr_CustomerAllocation_CheckFixedAgainstAllocationQuantity', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerAllocation_CheckFixedAgainstAllocationQuantity;
GO

CREATE TRIGGER tr_CustomerAllocation_CheckFixedAgainstAllocationQuantity
ON CustomerAllocation
AFTER UPDATE, INSERT 
AS 
BEGIN
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
		
		IF @allocationQuantity < @sumOfFixed
		BEGIN
			SET @errorMsg = 'Sum of fixed allocations exceeds allocation quantity' + @newLineChar
			 + '  Quantity: ' + cast(@allocationQuantity as nvarchar(50)) + @newLineChar
			 + '  Sum of Fixed: ' + cast(@sumOfFixed as nvarchar(50)) 
			RAISERROR (@errorMsg, 16, 5)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END

GO

IF OBJECT_ID ('tr_CustomerAllocation_CheckFixedAgainstCapacity', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerAllocation_CheckFixedAgainstCapacity;
GO

CREATE TRIGGER tr_CustomerAllocation_CheckFixedAgainstCapacity
ON CustomerAllocation
AFTER UPDATE, INSERT 
AS 
BEGIN
	DECLARE @fixedAllocation int
	DECLARE @errorMsg nvarchar(max)
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
		
			

		-- get customer groups 
		DECLARE @groups TABLE (groupID uniqueidentifier)

		INSERT INTO @groups 
			SELECT IDCustomerGroup FROM dbo.AnimationCustomerGroup WHERE IDAnimation = @animationID

		-- get customers and their capaties
		DECLARE @customers TABLE (customerID uniqueidentifier, capacity int)

		INSERT INTO @customers (customerID)
			SELECT ID FROM Customer
				WHERE IDCustomerGroup IN (SELECT groupID FROM @groups)

		-- update their capacities
		UPDATE @customers SET capacity = b.Capacity
			FROM @customers as a inner join dbo.CustomerCapacity as b 
				ON (a.customerID = b.IDCustomer AND b.IDAnimationType = @animationTypeID AND b.IDPriority = @priorityID
					AND b.IDItemType = @itemTypeId)

		-- sum of all stores capacities
		SELECT @capacityWithinAllStores = SUM(ISNULL(capacity,0)) FROM @customers		
	

		IF @capacityWithinAllStores < @sumOfFixed
		BEGIN
			SET @errorMsg = 'Sum of fixed allocations exceeds maximum capacity' + @newLineChar
			 + '  Capacity: ' + cast(@capacityWithinAllStores as nvarchar(50)) + @newLineChar
			 + '  Sum of Fixed: ' + cast(@sumOfFixed as nvarchar(50)) 
			RAISERROR (@errorMsg, 16, 8)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END
GO

IF OBJECT_ID ('tr_CustomerGroupAllocation_DivisibilityOfFixedAllocation', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerGroupAllocation_DivisibilityOfFixedAllocation;

GO

CREATE TRIGGER tr_CustomerGroupAllocation_DivisibilityOfFixedAllocation
ON CustomerGroupAllocation
AFTER UPDATE, INSERT 
AS 
BEGIN
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
			SET @errorMsg = 'Fixed quantity is not divisible by product multiple' + @newLineChar
			 + '  Fixed: ' + cast(@fixedAllocation as nvarchar(50)) + @newLineChar
			 + '  Multiple: ' + cast(@multiple as nvarchar(50)) + CASE WHEN @isWarehouse = 1 THEN ' (Warehouse account)' ELSE ' (Normal account)' END
			RAISERROR (@errorMsg, 16, 4)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END
GO


IF OBJECT_ID ('tr_CustomerGroupAllocation_CheckFixedAgainstAllocationQuantity', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerGroupAllocation_CheckFixedAgainstAllocationQuantity;
GO

CREATE TRIGGER tr_CustomerGroupAllocation_CheckFixedAgainstAllocationQuantity
ON CustomerGroupAllocation
AFTER UPDATE, INSERT 
AS 
BEGIN
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
			SET @errorMsg = 'Sum of group fixed allocations exceeds allocation quantity' + @newLineChar
			 + '  Quantity: ' + cast(@allocationQuantity as nvarchar(50)) + @newLineChar
			 + '  Sum of Fixed: ' + cast(@sumOfFixed as nvarchar(50)) 
			RAISERROR (@errorMsg, 16, 6)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END

GO

IF OBJECT_ID ('tr_CustomerGroupAllocation_CheckFixedAgainstCapacity', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerGroupAllocation_CheckFixedAgainstCapacity;
GO

CREATE TRIGGER tr_CustomerGroupAllocation_CheckFixedAgainstCapacity
ON CustomerGroupAllocation
AFTER UPDATE, INSERT 
AS 
BEGIN
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
		DECLARE @capacityWithinAllStores int
		DECLARE @sumOfFixed int		
		DECLARE @animationTypeID uniqueidentifier	
		DECLARE @priorityID uniqueidentifier

		
		SELECT @animationProductDetailId = IDAnimationProductDetail FROM inserted
		SELECT @animationProductId = IDAnimationProduct FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId
		SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId	
		SELECT @groupID = IDCustomerGroup FROM inserted
		SELECT @animationTypeID = IDAnimationType FROM dbo.Animation WHERE ID = @animationID
		SELECT @priorityID = IDPriority FROM dbo.Animation WHERE ID = @animationID
		SELECT @itemTypeID = IDItemType FROM dbo.AnimationProduct WHERE ID = @animationProductId


		-- get customer groups 
		DECLARE @groups TABLE (groupID uniqueidentifier, fixed int)

		INSERT INTO @groups (groupID)
			SELECT IDCustomerGroup FROM dbo.AnimationCustomerGroup WHERE IDAnimation = @animationID

		UPDATE @groups SET fixed = b.ManualFixedAllocation
			FROM @groups as a inner join dbo.CustomerGroupAllocation as b on (a.groupID = b.IDCustomerGroup
				AND b.IDAnimationProductDetail = @animationProductDetailId)

		-- get customers and their capaties
		DECLARE @customers TABLE (customerID uniqueidentifier, capacity int)

		INSERT INTO @customers (customerID)
			SELECT ID FROM Customer
				WHERE IDCustomerGroup IN (SELECT groupID FROM @groups)

		-- update their capacities
		UPDATE @customers SET capacity = b.Capacity
			FROM @customers as a inner join dbo.CustomerCapacity as b 
				ON (a.customerID = b.IDCustomer AND b.IDAnimationType = @animationTypeID AND b.IDPriority = @priorityID
					AND b.IDItemType = @itemTypeId)

		-- sum of all stores capacities
		SELECT @capacityWithinAllStores = SUM(ISNULL(capacity,0)) FROM @customers

		-- sum fixed allocation
		SELECT @sumOfFixed = SUM(ISNULL(fixed,0)) FROM @groups
		
		IF @capacityWithinAllStores < @sumOfFixed
		BEGIN
			SET @errorMsg = 'Sum of group fixed allocations exceeds maximum capacity' + @newLineChar
			 + '  Capacity: ' + cast(@capacityWithinAllStores as nvarchar(50)) + @newLineChar
			 + '  Sum of Fixed: ' + cast(@sumOfFixed as nvarchar(50)) 
			RAISERROR (@errorMsg, 16, 7)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END
GO