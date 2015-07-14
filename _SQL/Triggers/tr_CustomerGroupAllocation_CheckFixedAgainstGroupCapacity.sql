IF OBJECT_ID ('tr_CustomerGroupAllocation_CheckFixedAgainstGroupCapacity', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerGroupAllocation_CheckFixedAgainstGroupCapacity;
GO

CREATE TRIGGER tr_CustomerGroupAllocation_CheckFixedAgainstGroupCapacity
ON CustomerGroupAllocation
AFTER UPDATE, INSERT 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @fixedAllocation int
	SELECT @fixedAllocation = ManualFixedAllocation FROM inserted

	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)
	
	DECLARE @groupID uniqueidentifier
	SELECT @groupID = IDCustomerGroup FROM inserted

	IF @fixedAllocation IS NOT NULL
	BEGIN
		
		DECLARE @animationProductDetailId uniqueidentifier
		DECLARE @animationProductId uniqueidentifier
		DECLARE @animationId uniqueidentifier	
		DECLARE @itemTypeID uniqueidentifier		
		DECLARE @errorMsg nvarchar(max)
		DECLARE @capacityWithinAllStores int
		DECLARE @sumOfFixed int		
		DECLARE @animationTypeID uniqueidentifier	
		DECLARE @priorityID uniqueidentifier

		
		SELECT @animationProductDetailId = IDAnimationProductDetail FROM inserted
		SELECT @animationProductId = IDAnimationProduct FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId
		SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId			
		SELECT @animationTypeID = IDAnimationType FROM dbo.Animation WHERE ID = @animationID
		SELECT @priorityID = IDPriority FROM dbo.Animation WHERE ID = @animationID
		SELECT @itemTypeID = IDItemType FROM dbo.AnimationProduct WHERE ID = @animationProductId
		

		-- get customers and their capaties
		DECLARE @customers TABLE (customerID uniqueidentifier, capacity int)

		INSERT INTO @customers (customerID)
			SELECT ID FROM Customer
				WHERE IDCustomerGroup = @groupID
				
		-- update their capacities
		/*UPDATE @customers SET capacity = b.Capacity
			FROM @customers as a inner join dbo.CustomerCapacity as b 
				ON (a.customerID = b.IDCustomer AND b.IDAnimationType = @animationTypeID AND b.IDPriority = @priorityID
					AND b.IDItemType = @itemTypeId)*/
		UPDATE @customers SET capacity = dbo.calculate_ProductDetailCustomerTotalCapacity(@animationProductDetailId, customerID)			

		-- sum of all stores capacities
		SELECT @capacityWithinAllStores = SUM(ISNULL(capacity,0)) FROM @customers		
		
		IF @capacityWithinAllStores < @fixedAllocation
		BEGIN
			/*SET @errorMsg = 'Group fixed allocation exceeds maximum group capacity' + @newLineChar
			 + '  Capacity: ' + cast(@capacityWithinAllStores as nvarchar(50)) + @newLineChar
			 + '  Fixed: ' + cast(@fixedAllocation as nvarchar(50)) */
			SET @errorMsg =  dbo.uf_getSystemMessage('tr_CustomerGroupAllocation_CheckFixedAgainstGroupCapacity',  cast(@capacityWithinAllStores as nvarchar(50)), cast(@fixedAllocation as nvarchar(50)) , null, null, null, null, null, null )
			RAISERROR (@errorMsg, 16, 19)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END