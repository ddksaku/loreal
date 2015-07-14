USE [LorealOptimiseClean]
GO
/****** Object:  Trigger [dbo].[tr_CustomerGroupAllocation_CheckFixedAgainstCapacity]    Script Date: 06/30/2010 16:41:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[tr_CustomerGroupAllocation_CheckFixedAgainstCapacity]
ON [dbo].[CustomerGroupAllocation]
AFTER UPDATE, INSERT 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @fixedAllocation int
	SELECT @fixedAllocation = ManualFixedAllocation FROM inserted
	
	DECLARE @customerGroupAllocationID uniqueidentifier
	SELECT @customerGroupAllocationID = ID from inserted

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
		SELECT @capacityWithinAllStores = SUM(ISNULL(capacity,0)) FROM @customers
		*/
		SELECT @capacityWithinAllStores = dbo.calculate_TotalCapacityCustomerGroup(@customerGroupAllocationID)		
		

		-- sum fixed allocation
		SELECT @sumOfFixed = SUM(ISNULL(fixed,0)) FROM @groups
		
		IF @capacityWithinAllStores < @sumOfFixed
		BEGIN
			/*SET @errorMsg = 'Sum of group fixed allocations exceeds maximum capacity' + @newLineChar
			 + '  Capacity: ' + cast(@capacityWithinAllStores as nvarchar(50)) + @newLineChar
			 + '  Sum of Fixed: ' + cast(@sumOfFixed as nvarchar(50)) */
			SET @errorMsg = dbo.uf_getSystemMessage('tr_CustomerGroupAllocation_CheckFixedAgainstCapacity', cast(@capacityWithinAllStores as nvarchar(50)) , cast(@sumOfFixed as nvarchar(50)), null, null, null, null, null, null)
			RAISERROR (@errorMsg, 16, 7)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END