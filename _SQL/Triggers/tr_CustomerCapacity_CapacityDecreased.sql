IF OBJECT_ID ('tr_CustomerCapacity_CapacityDecreased', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerCapacity_CapacityDecreased;
GO

CREATE TRIGGER tr_CustomerCapacity_CapacityDecreased
ON CustomerCapacity
AFTER UPDATE 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	declare @logId uniqueidentifier
	set @logId = newid()

	DECLARE @capacityID uniqueidentifier
	SELECT @capacityID = ID from inserted

	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)

	DECLARE @alertMsg nvarchar(max)

	DECLARE @oldValue int
	DECLARE @newValue int

	SELECT @oldValue = Capacity FROM deleted
	SELECT @newValue = Capacity FROM inserted
	
	IF @oldValue > @newValue
	BEGIN
	
		declare @decrease int
		set @decrease = @oldValue - @newValue

		-- find potentional impacted animations
		DECLARE @animations TABLE (animationID uniqueidentifier, numberOfGroups int, numberOfDetails int)
		DECLARE @priorityID uniqueidentifier
		DECLARE @itemTypeID uniqueidentifier
		DECLARE @animationTypeID uniqueidentifier
		DECLARE @customerID uniqueidentifier
		DECLARE @salesAreaID uniqueidentifier
		DECLARE @customerGroupID uniqueidentifier
		
		-- variables for use in alert message
		declare @customerName nvarchar(255)	
		declare @customerCode nvarchar(255)
		declare @originalValue int
		declare @newComputedValue int
		declare @groupName nvarchar(255)
		declare @groupCode nvarchar(255)
		declare @materialCode nvarchar(255)
		declare @itemGroup nvarchar(255)
		declare @salesAreaname nvarchar(255)	
		declare @animationName nvarchar(255)	
		
		
		SELECT @priorityID = IDPriority, @itemTypeID = IDItemType, @animationTypeID = IDAnimationType FROM inserted
		SELECT @customerID = IDCustomer FROM inserted
		SELECT @customerGroupID = IDCustomerGroup FROM dbo.Customer WHERE ID = @customerID
		SELECT @salesAreaID = IDSalesArea_AllocationSalesArea FROM dbo.Customer WHERE ID = @customerID
		IF @salesAreaID IS NULL
		BEGIN
			DECLARE @groupID uniqueidentifier
			SELECT @groupID = IDCustomerGroup FROM dbo.Customer WHERE ID = @customerID
			SELECT @salesAreaID = IDSalesArea FROM dbo.CustomerGroup WHERE ID = @groupID
		END
		
		
		
		-- find out multiple
		declare @multipleID uniqueidentifier
		declare @multiple int
		declare @isWarehouse bit	
		
		select @isWarehouse = WarehouseAllocation from dbo.CustomerGroupItemType
			where IDCustomerGroup = @customerGroupID and IDItemType = @itemTypeID			
		


		-- find active animations
		INSERT INTO @animations (animationID)
			SELECT ID FROM dbo.Animation
				WHERE IDPriority = @priorityID AND IDAnimationType = @animationTypeID AND [Status] <> 5 AND [Status] <> 6
	
		-- find count of groups attached	
		UPDATE @animations SET numberOfGroups = 
			 (SELECT count(*) as pocet from  dbo.AnimationCustomerGroup WHERE IDAnimation = animationID)

		-- remove those without customer groups attached
		DELETE FROM @animations WHERE numberOfGroups = 0


		-- find count of animation product details attached
		UPDATE @animations SET numberOfDetails = 
			(SELECT count(*) from dbo.AnimationProductDetail WHERE IDSalesArea = @salesAreaID AND IDAnimationProduct in (SELECT ID FROM dbo.AnimationProduct WHERE IDAnimation = animationID AND IDItemType = @itemTypeID))

		-- remove those without animation product detail attached
		DELETE FROM @animations WHERE numberOfDetails = 0

		-- *190* - update rest of animations and set them to be active 
		UPDATE dbo.Animation SET [Status] = 1 , ModifiedDate = getdate(), ModifiedBy = 'Trigger - capacity decreased' WHERE ID IN (SELECT animationID FROM @animations)

		-- find animation product details possible impacted
		DECLARE @animationProductDetails TABLE (ID uniqueidentifier, allocationQuantity int)

		INSERT INTO @animationProductDetails (ID)
			SELECT ID from dbo.AnimationProductDetail WHERE /*IDSalesArea = @salesAreaID AND*/ IDAnimationProduct in (SELECT ID FROM dbo.AnimationProduct WHERE IDAnimation IN (SELECT animationID FROM @animations) AND IDItemType = @itemTypeID)

		-- exceeds now customer fixed allocation this new capacity?
		DECLARE @customerAllocations TABLE (ID uniqueidentifier)

		INSERT INTO @customerAllocations (ID)
			SELECT ID FROM dbo.CustomerAllocation WHERE IDCustomer = @customerID AND IDAnimationProductDetail IN (SELECT ID FROM @animationProductDetails)
		
		DECLARE @kurzorCustomerAllocation uniqueidentifier
		DECLARE @fixedAllocation int
		DECLARE @animationProductDetailID uniqueidentifier
		DECLARE kurzor CURSOR
		FOR SELECT ID FROM @customerAllocations
		
		OPEN kurzor

		FETCH NEXT FROM kurzor
		INTO @kurzorCustomerAllocation		
		
		SET @alertMsg = ''	
		declare @logstring nvarchar(max)
			
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SELECT @fixedAllocation = FixedAllocation, @animationProductDetailID = IDAnimationProductDetail FROM dbo.CustomerAllocation WHERE ID = @kurzorCustomerAllocation
	
			IF @fixedAllocation > @newValue
			BEGIN						
				
				select @logstring = cast(@kurzorCustomerAllocation as nvarchar(255))
				execute dbo.logFile @logstring, @logId
				
				select @customerName = b.Name, @customerCode = b.AccountNumber, @originalValue = a.FixedAllocation
					from dbo.CustomerAllocation as a inner join dbo.Customer as b on (a.IDCustomer = b.ID)		
					
				select @animationName = c.Name from dbo.AnimationProductDetail a 
					inner join dbo.AnimationProduct b on (a.IDAnimationProduct = b.ID)
					inner join dbo.Animation c on (b.IDAnimation = c.ID)					
					where a.ID = @animationProductDetailID
				
				-- get multiple --------------
				if 	@isWarehouse = 1
				begin
					select @multipleID = IDMutlipleWarehouse from dbo.AnimationProduct where ID = (select IDAnimationProduct from dbo.AnimationProductDetail where ID = @animationProductDetailID)
				end 
				else
				begin
					select @multipleID = IDMultipleNormal from dbo.AnimationProduct where ID = (select IDAnimationProduct from dbo.AnimationProductDetail where ID = @animationProductDetailID)
				end
				
				select @multiple = [Value] from dbo.Multiple WHERE ID = @multipleID
				select @multiple = ISNULL(@multiple, 1)
				-------------------------------
				
				
				
				-- get value for update (use multiple logic) ----------------------------
				--select @newComputedValue = @originalValue - (@fixedAllocation - @newValue)
				
				select @newComputedValue = @fixedAllocation - 
				(@multiple * ceiling((cast(@fixedAllocation - @newValue as float)) / cast(@multiple as float)))
				
				
				-------------------------------------------------------------------------
						
			
				SET @alertMsg = @alertMsg + 'Animation '+ @animationName + ': Customer Fixed Allocation Changed: ' + isnull(@customerName,'') + ' (' + isnull(@customerCode,'') + ')' +' ... decreased from ' + isnull(cast(@fixedAllocation as nvarchar(55)), 'null') + ' to ' + isnull(cast(@newComputedValue as nvarchar(55)), 'null') + @newLineChar
							
				-- decrease customer Fixed Allocation
				UPDATE dbo.CustomerAllocation SET FixedAllocation = @newComputedValue, ModifiedDate = getdate(), ModifiedBy = 'Trigger - capacity decreased'
					WHERE ID = @kurzorCustomerAllocation	
				
			END	

			FETCH NEXT FROM kurzor
			INTO @kurzorCustomerAllocation
		END

		CLOSE kurzor
		DEALLOCATE kurzor


		-- exceeds now some customer group allocation this new capacity?
		DECLARE @customerGroupAllocation table (ID uniqueidentifier, cusGroup uniqueidentifier, capacity int, animationProductDetail uniqueidentifier)		

		INSERT INTO @customerGroupAllocation (ID, animationProductDetail)
			SELECT ID, IDAnimationProductDetail
				FROM dbo.CustomerGroupAllocation
					WHERE IDAnimationProductDetail IN (SELECT ID FROM @animationProductDetails)
						AND IDCustomerGroup = @customerGroupID

		DECLARE @customerGroupFixedAllocationSys int
		DECLARE @customerGroupFixedAllocationMan int	

			
		DECLARE @kurzorID uniqueidentifier		
		DECLARE @kurzorAnimProDetail uniqueidentifier
		DECLARE @allocationQuantity int
		DECLARE @capacityWithinAllStores int
		DECLARE @capacityWithinAllStoresWithinWholeAnimation int	
		DECLARE @animationProductID uniqueidentifier

		DECLARE @customers TABLE (customerID uniqueidentifier, capacity int)	
		INSERT INTO @customers (customerID)
			SELECT ID FROM Customer
				WHERE IDCustomerGroup = @customerGroupID

		-- update their capacities
		UPDATE @customers SET capacity = b.Capacity
			FROM @customers as a inner join dbo.CustomerCapacity as b 
				ON (a.customerID = b.IDCustomer AND b.IDAnimationType = @animationTypeID AND b.IDPriority = @priorityID
					AND b.IDItemType = @itemTypeId)

		-- sum of all stores capacities
		SELECT @capacityWithinAllStores = SUM(ISNULL(capacity,0)) FROM @customers
		SET @capacityWithinAllStores = ISNULL(@capacityWithinAllStores, 0)

		DECLARE kurzor2 CURSOR
		FOR SELECT ID, animationProductDetail FROM @customerGroupAllocation	

		OPEN kurzor2

		FETCH NEXT FROM kurzor2
		INTO @kurzorID, @kurzorAnimProDetail

		WHILE @@FETCH_STATUS = 0
		BEGIN				
			
			SELECT @customerGroupFixedAllocationSys = SystemFixedAllocation FROM dbo.CustomerGroupAllocation
				WHERE ID = @kurzorID

			SELECT @customerGroupFixedAllocationMan = ManualFixedAllocation FROM dbo.CustomerGroupAllocation
				WHERE ID = @kurzorID
				
			select @groupName = b.Name, @groupCode = b.Code
					from dbo.CustomerGroupAllocation as a inner join dbo.CustomerGroup as b on (a.IDCustomerGroup = b.ID)	
			
			--- get multiple -------------------
			if 	@isWarehouse = 1
			begin
				select @multipleID = IDMutlipleWarehouse from dbo.AnimationProduct where ID = (select IDAnimationProduct from dbo.AnimationProductDetail where ID = @kurzorAnimProDetail) 
			end 
			else
			begin
				select @multipleID = IDMultipleNormal from dbo.AnimationProduct where ID = (select IDAnimationProduct from dbo.AnimationProductDetail where ID = @kurzorAnimProDetail) 
			end
			
			select @multiple = [Value] from dbo.Multiple WHERE ID = @multipleID
			select @multiple = ISNULL(@multiple, 1)		
			-------------------------------------
			
			--- get animation name
			select @animationName = c.Name from dbo.AnimationProductDetail a 
					inner join dbo.AnimationProduct b on (a.IDAnimationProduct = b.ID)
					inner join dbo.Animation c on (b.IDAnimation = c.ID)					
					where a.ID = @kurzorAnimProDetail
					
			IF @customerGroupFixedAllocationMan > @capacityWithinAllStores
			BEGIN				
				---- get newly computed value ---------------
				select @newComputedValue = @customerGroupFixedAllocationMan - 
				(@multiple * ceiling((cast(@customerGroupFixedAllocationMan - @capacityWithinAllStores as float)) / cast(@multiple as float)))
				----------------------------------------------
				
				--select @newComputedValue = @customerGroupFixedAllocationMan - (@customerGroupFixedAllocationMan - @capacityWithinAllStores)
			
				SET @alertMsg = @alertMsg +'Animation ' + @animationName + ': Group manual fixed changed: '+ isnull(@groupName,'') + ' (' + isnull(@groupCode,'') + ')' +' ... decreased from ' + isnull(cast(@customerGroupFixedAllocationMan as nvarchar(55)), 'null') + ' to ' + isnull(cast(@newComputedValue as nvarchar(55)), 'null') + @newLineChar
			
				
				-- decrease group manual fixed allocation
				UPDATE dbo.CustomerGroupAllocation SET ManualFixedAllocation = @newComputedValue, ModifiedDate = getdate(), ModifiedBy = 'Trigger - capacity decreased'
					where ID = @kurzorID				
				
			END	
			
				
						
			IF @customerGroupFixedAllocationSys > @capacityWithinAllStores
			BEGIN			
				
				--select @newComputedValue = @customerGroupFixedAllocationSys - (@customerGroupFixedAllocationSys - @capacityWithinAllStores)
				---- get newly computed value ---------------
				select @newComputedValue = @customerGroupFixedAllocationSys - 
				(@multiple * ceiling((cast(@customerGroupFixedAllocationSys - @capacityWithinAllStores as float)) / cast(@multiple as float)))
				----------------------------------------------			
				
			
				SELECT @alertMsg = @alertMsg +'Animation ' + @animationName + ': Group system fixed changed: ' + isnull(@groupName,'') + ' (' + isnull(@groupCode,'') + ')' +' ... decreased from ' + isnull(cast(@customerGroupFixedAllocationSys as nvarchar(55)),'null') + ' to ' + isnull(cast(@newComputedValue as nvarchar(55)),'null') + @newLineChar
							
				-- decrease group system Fixed Allocation
				UPDATE dbo.CustomerGroupAllocation SET SystemFixedAllocation = @newComputedValue, ModifiedDate = getdate(), ModifiedBy = 'Trigger - capacity decreased'
					where ID = @kurzorID
			END					

			
			SELECT @allocationQuantity = AllocationQuantity FROM dbo.AnimationProductDetail
				WHERE ID = @kurzorAnimProDetail	
				
			select @materialCode = c.MaterialCode, @itemGroup = e.Name, @salesAreaname = d.Name
				from dbo.AnimationProductDetail	as a inner join dbo.AnimationProduct as b on (a.IDAnimationProduct = b.ID)
					inner join dbo.Product as c on (b.IDProduct = c.ID)
					inner join dbo.SalesArea as d on (a.IDSalesArea = d.ID)
					inner join dbo.ItemGroup as e on (b.IDItemGroup = e.ID)
					WHERE a.ID = @kurzorAnimProDetail	
				
			select @animationProductID = IDAnimationProduct FROM dbo.AnimationProductDetail where ID = @kurzorAnimProDetail		
			select @capacityWithinAllStoresWithinWholeAnimation = dbo.calculate_TotalCapacity(@animationProductID);		
			
			IF @allocationQuantity > @capacityWithinAllStoresWithinWholeAnimation
			BEGIN				
				
				---- get newly computed value ---------------
				select @newComputedValue = @allocationQuantity - 
				(@multiple * ceiling((cast(@allocationQuantity - @capacityWithinAllStoresWithinWholeAnimation as float)) / cast(@multiple as float)))
				----------------------------------------------			
				
				--select @newComputedValue = @allocationQuantity - (@allocationQuantity - @capacityWithinAllStores)
				
				SET @alertMsg = @alertMsg +'Animation ' + @animationName+ ': Allocation quantity changed : '+ isnull(@materialCode,'') + '-' + isnull(@itemGroup,'') + '-' +isnull(@salesAreaname,'') + ' ... decreased from ' + isnull(cast(@allocationQuantity as nvarchar(55)),'null') + ' to ' + isnull(cast(@newComputedValue as nvarchar(55)),'null') + @newLineChar
				
				
				-- decrease alloation qunatity
				UPDATE dbo.AnimationProductDetail SET AllocationQuantity = @newComputedValue, ModifiedDate = getdate(), ModifiedBy = 'Trigger - capacity decreased'
					where ID = @kurzorAnimProDetail
			END		
			
			DECLARE @forecastQuantity int
			
			SELECT @forecastQuantity = ForecastProcQuantity FROM dbo.AnimationProductDetail
			WHERE ID = @kurzorAnimProDetail		
			
			IF @forecastQuantity > @capacityWithinAllStoresWithinWholeAnimation
			BEGIN
			
				select @newComputedValue = @forecastQuantity - (@forecastQuantity - @capacityWithinAllStoresWithinWholeAnimation)
				
				SET @alertMsg = @alertMsg +'Animation ' + @animationName+ ': Forecast quantity changed : '+ isnull(@materialCode,'') + '-' + isnull(@itemGroup,'') + '-' +@salesAreaname + ' ... decreased from ' + isnull(cast(@forecastQuantity as nvarchar(55)),'null') + ' to ' + isnull(cast(@newComputedValue as nvarchar(55)),'null') + @newLineChar
				
			
				-- decrease forecast quantity
				UPDATE dbo.AnimationProductDetail SET ForecastProcQuantity = @newComputedValue, ModifiedDate = getdate(), ModifiedBy = 'Trigger - capacity decreased'
					where ID = @kurzorAnimProDetail
			
			END

			
			FETCH NEXT FROM kurzor2
			INTO @kurzorID, @kurzorAnimProDetail
		END			

		CLOSE kurzor2
		DEALLOCATE kurzor2	

	END	
	
	
	if @alertMsg is not null AND @alertMsg != ''
		RAISERROR (@alertMsg, 1, 26)
	
	
	RETURN
END