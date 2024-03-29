IF OBJECT_ID (N'uf_allocate_anPrDeIDcuGrID') IS NOT NULL
    DROP procedure uf_allocate_anPrDeIDcuGrID;
GO

-- *****************************************
-- ** ALLOCATION TO CUSTOMERS
-- **
-- ** parametres: just one from @customerGroupId AND @animationParaId
-- **             - @animationParaId is for case, when we are allocating throw all customers within customer groups 
-- **				without fixed allocation
-- **             - @animationParaId will change @customers table
-- **			  - actually @animationParaId is not used anywhere
-- **
-- **************************************************************

create procedure [dbo].[uf_allocate_anPrDeIDcuGrID] 
	(
	  @customerGroupId uniqueidentifier
	 ,@animationParaId uniqueidentifier	 
	 ,@animationProductDetailId uniqueidentifier
	 ,@logId uniqueidentifier
	 ,@allocationWarehouseQuantity int = NULL
	 ,@alreadyAllocated int output
	 ,@zbytekNotAllocated int output
	 ,@jeZbytek int = 0
	 ,@includeWarehouse bit = 1
	 )

AS
	BEGIN	

		-- for log		
		DECLARE @logString nvarchar(max)	
		

		SET @logString = 'CustomGroupId: ' + ISNULL(CONVERT(nvarchar(max),@customerGroupId), 'not inserted') + ' ('+ (select Name from CustomerGroup where ID = @customerGroupId ) +')'
		EXEC [logFile]
			@logString,
			@logId

		SET @logString = 'AnimationProductDetailId: ' + ISNULL(CONVERT(nvarchar(max),@animationProductDetailId), 'not inserted')
		EXEC [logFile]
			@logString,
			@logId

		EXEC [logFile]
			'------------------',
			@logId

		
		-- allocation quantity depends on input parametres
		DECLARE @allocationQuantity INT
		IF @allocationWarehouseQuantity IS NOT NULL
		BEGIN
			SET @allocationQuantity = @allocationWarehouseQuantity
			-- log
			SET @logString = '@allocationQuantity: ' + ISNULL(CONVERT(nvarchar(max),@allocationQuantity), 'not inserted') + ' (Using warehousemultiple)'
			EXEC [logFile]
				@logString,
				@logId
		END
		ELSE
		BEGIN
			SELECT @allocationQuantity = AllocationQuantity FROM AnimationProductDetail WHERE ID = @animationProductDetailId
			-- log
			SET @logString = '@allocationQuantity: ' + ISNULL(CONVERT(nvarchar(max),@allocationQuantity), 'not inserted')
			EXEC [logFile]
				@logString,
				@logId
		END


		-- derived variable
		DECLARE @animationProductId uniqueidentifier
		SELECT @animationProductId = IDAnimationProduct FROM AnimationProductDetail WHERE ID = @animationProductDetailId
		-- log
		SET @logString = '@animationProductId: ' + ISNULL(CONVERT(nvarchar(max),@animationProductId), 'not inserted')
		EXEC [logFile]
			@logString,
			@logId


		DECLARE @productId uniqueidentifier
		SELECT @productId = IDProduct FROM AnimationProduct WHERE ID = @animationProductId
		-- log
		SET @logString = '@productId: ' + ISNULL(CONVERT(nvarchar(max),@productId), 'not inserted')  + ' ('+ (select MaterialCode from Product where ID = @productId ) +')'
		EXEC [logFile]
			@logString,
			@logId


		DECLARE @salesAreaId uniqueidentifier
		SELECT @salesAreaId = IDSalesArea FROM CustomerGroup WHERE ID = @customerGroupId
		-- log
		SET @logString = '@salesAreaId: ' + ISNULL(CONVERT(nvarchar(max),@salesAreaId), 'not inserted')  + ' ('+ (select Name from SalesArea where ID = @salesAreaId ) +')'
		EXEC [logFile]
			@logString,
			@logId


		DECLARE @retailMultiplier float
		SELECT @retailMultiplier = RetailMultiplier FROM SalesArea WHERE ID = @salesAreaId
		-- log
		SET @logString = '@retailMultiplier: ' + ISNULL(CONVERT(nvarchar(max),@retailMultiplier), 'not inserted')
		EXEC [logFile]
			@logString,
			@logId


		DECLARE @animationId uniqueidentifier
		SELECT @animationId = IDAnimation FROM AnimationProduct WHERE ID = @animationProductId
		-- log
		SET @logString = '@animationId: ' + ISNULL(CONVERT(nvarchar(max),@animationId), 'not inserted')  + ' ('+ (select Name from Animation where ID = @animationId ) +')'
		EXEC [logFile]
			@logString,
			@logId


		DECLARE @animationTypeId uniqueidentifier
		SELECT @animationTypeId = IDAnimationType FROM Animation WHERE ID = @animationId
		-- log
		SET @logString = '@animationTypeId: ' + ISNULL(CONVERT(nvarchar(max),@animationTypeId), 'not inserted')  + ' ('+ (select Name from AnimationType where ID = @animationTypeId ) +')'
		EXEC [logFile]
			@logString,
			@logId


		DECLARE @itemTypeId uniqueidentifier
		SELECT @itemTypeId = IDItemType FROM AnimationProduct WHERE ID = @animationProductId
		-- log
		SET @logString = '@itemTypeId: ' + ISNULL(CONVERT(nvarchar(max),@itemTypeId), 'not inserted') + ' ('+ (select Name from ItemType where ID = @itemTypeId ) +')'
		EXEC [logFile]
			@logString,
			@logId


		DECLARE @brandAxeId uniqueidentifier
		SELECT @brandAxeId = IDBrandAxe FROM AnimationProduct WHERE ID = @animationProductId
		-- log
		SET @logString = '@brandAxeId: ' + ISNULL(CONVERT(nvarchar(max),@brandAxeId), 'not inserted') + ' ('+ (select Name from BrandAxe where ID = @brandAxeId ) +')'
		EXEC [logFile]
			@logString,
			@logId


		DECLARE @priorityId  uniqueidentifier
		SELECT @priorityId = IDPriority FROM Animation WHERE ID = @animationId
		-- log
		SET @logString = '@priorityId: ' + ISNULL(CONVERT(nvarchar(max),@priorityId), 'not inserted') + ' ('+ (select Name from Priority where ID = @priorityId ) +')'
		EXEC [logFile]
			@logString,
			@logId


		DECLARE @multipleNormal int
		SELECT @multipleNormal = [Value] FROM Multiple WHERE ID = (SELECT IDMultipleNormal FROM AnimationProduct WHERE ID = @animationProductId)
		-- log
		SET @logString = '@multipleNormal: ' + ISNULL(CONVERT(nvarchar(max),@multipleNormal), 'not inserted')
		EXEC [logFile]
			@logString,
			@logId


		DECLARE @multipleWarehouse int
		SELECT @multipleWarehouse = [Value] FROM Multiple WHERE ID = (SELECT IDMutlipleWarehouse FROM AnimationProduct WHERE ID = @animationProductId)
		-- log
		SET @logString = '@multipleWarehouse: ' + ISNULL(CONVERT(nvarchar(max),@multipleWarehouse), 'not inserted')
		EXEC [logFile]
			@logString,
			@logId

		DECLARE @customerGroupRetailUplift float
		SELECT @customerGroupRetailUplift = RetailUplift FROM CustomerGroupAllocation WHERE IDCustomerGroup = @customerGroupId
		-- log
		SET @logString = '@customerGroupRetailUplift: ' + ISNULL(CONVERT(nvarchar(max),@customerGroupRetailUplift), 'not inserted')
		EXEC [logFile]
			@logString,
			@logId

		DECLARE @category uniqueidentifier
		SELECT @category = IDCategory FROM dbo.AnimationProduct WHERE ID = @animationProductId
		-- log
		SET @logString = '@category: ' + ISNULL(CONVERT(nvarchar(max),@category), 'not inserted')
		EXEC [logFile]
			@logString,
			@logId

		DECLARE @signatureId uniqueidentifier
		SELECT @signatureId = IDSignature FROM dbo.AnimationProduct WHERE ID = @animationProductId


		EXEC [logFile]
			'------------------',
			@logId


		-- customers temp table
		DECLARE @customers TABLE 
			(
			 customerId uniqueidentifier
			,customerGroupId uniqueidentifier
			,customerName nvarchar(100)
			,salesAreaId uniqueidentifier
			,fixedAllocation int
			,calculatedAllocationX int
			,retailUplift float
			,saleOrderValue float
			,capacity int
			,brandExclusion bit
			,tempID uniqueidentifier
			)

		IF @customerGroupId IS NOT NULL
		BEGIN
			-- insert customers from customerGroup
			INSERT INTO @customers (customerId, customerGroupId, customerName, salesAreaId)
				SELECT ID, IDCustomerGroup, Name, IDSalesArea_AllocationSalesArea
					FROM Customer
						WHERE IDCustomerGroup = @customerGroupId AND IncludeInSystem = 1
								AND Deleted = 0
			
			SELECT @salesAreaID = IDSalesArea	FROM CustomerGroup 	WHERE ID = @customerGroupId

			UPDATE @customers SET salesAreaId = @salesAreaID WHERE salesAreaId IS NULL
		END
		ELSE IF @animationParaId IS NOT NULL
		BEGIN
			-- insert all customers from customer groups without fixed allocation @animationProductDetailId
			INSERT INTO @customers (customerId, customerName, salesAreaId, customerGroupId)
				SELECT ID, Name, IDSalesArea_AllocationSalesArea, IDCustomerGroup FROM Customer WHERE Deleted = 0 AND IncludeInSystem = 1 AND IDCustomerGroup IN 
					(SELECT a.IDCustomerGroup FROM [dbo].[AnimationCustomerGroup] as a left join CustomerGroupAllocation as b 
					on (a.IDCustomerGroup = b.IDCustomerGroup AND @animationProductDetailId = b.IDAnimationProductDetail)
					WHERE ManualFixedAllocation IS NULL AND SystemFixedAllocation IS NULL AND IncludeInAllocation = 1 AND Deleted = 0 AND IDAnimation = @animationId)

			-- exclude customers from customer groups with warehouse allocation
			IF @includeWarehouse = 0
			BEGIN
				declare @groupsWa table 
					(groupId uniqueidentifier)
				INSERT INTO @groupsWa SELECT IDCustomerGroup
				FROM  CustomerGroupItemType where IDItemType = @itemTypeId AND WarehouseAllocation = 1

				DELETE FROM @customers WHERE customerGroupId IN (SELECT groupId FROM @groupsWa)
				
			END

			-- get sales area
			UPDATE @customers SET salesAreaId = b.IDSalesArea
				FROM @customers as a inner join CustomerGroup as b on (a.customerGroupId = b.ID)
				WHERE salesAreaId IS NULL
		
		END

		-- delete customers from groups not to be included in system
		DELETE FROM @customers
			WHERE customerGroupId IN (SELECT ID FROM dbo.CustomerGroup WHERE IncludeInSystem = 0)

		-- delete customers with different sales area
		DELETE FROM @customers WHERE salesAreaId <> @salesAreaId

		-- delete customers with different Category
		IF @category IS NOT NULL
		BEGIN
			DELETE FROM @customers WHERE customerID not in 
				(SELECT IDCustomer FROM dbo.CustomerCategory WHERE IDCategory = @category)

			-- update 
			UPDATE CustomerAllocation SET CalculatedAllocation =  0, ModifiedDate = GETDATE(), ModifiedBy = 'allocation script d'
				where IDAnimationProductDetail = @animationProductDetailId
					and IDCustomer not in (SELECT IDCustomer FROM dbo.CustomerCategory WHERE IDCategory = @category)
			
			
		END
		

		UPDATE @customers SET tempID = NEWID()

		-- update allocations
		UPDATE @customers SET fixedAllocation = b.FixedAllocation, retailUplift = ISNULL(b.RetailUplift, ISNULL(@customerGroupRetailUplift,1))
			FROM @customers as a inner join CustomerAllocation as b on (a.customerId = b.IDCustomer) where b.IDAnimationProductDetail = @animationProductDetailId


		-- update capacity
		UPDATE @customers SET capacity = b.Capacity
			FROM @customers as a inner join CustomerCapacity as b 
				on (a.customerID = b.IDCustomer AND @animationTypeId = b.IDAnimationType 
				AND @itemTypeId = b.IDItemType AND @priorityId = b.IDPriority)

		-- update brandExclusion
		UPDATE @customers SET brandExclusion = b.Excluded
			FROM @customers as a inner join CustomerBrandExclusion as b 
				on (a.customerId = b.IDCustomer AND @brandAxeId = b.IDBrandAxe)

		


		SELECT  @logString = 'Customers: ' + ISNULL(
		 STUFF  
		 (  
			 (  
			 SELECT ', || GUID: ' + ISNULL(cast(customerId AS VARCHAR(max)),'') + '('+customerName+') FIXED: ' + ISNULL(cast(fixedAllocation as varchar(50)),'NULL') + ' RETAIL: ' + ISNULL(cast(retailUplift as varchar(50)),'') + ' CAPACITY: ' + isnull(cast(capacity as varchar(50)),'0')
			 FROM @customers        
			 FOR XML PATH ('')  
			 ),1,1,''  
		 ),'-- none--')

		EXEC [logFile]
			@logString,
			@logId	



		-- temp table for "foreach" loop
		declare @tempSalesOrderValue table  ( 
											idx int IDENTITY (1, 1) NOT NULL, 
											customerId uniqueidentifier,										
											retailUplift float
											)

		insert into @tempSalesOrderValue (customerId, retailUplift)
			select customerId, retailUplift from @customers
				where brandExclusion IS NULL OR brandExclusion <> 1
		


		-- compute salesOrderValue
		DECLARE @i int
		DECLARE @numrows int
		DECLARE @customerSalesId uniqueidentifier
		DECLARE @retailUplift float
		DECLARE @suma float
		DECLARE @customerName nvarchar(100)

		SET @i = 1
		SET @numrows = (SELECT COUNT(*) FROM @tempSalesOrderValue)
		IF @numrows > 0
			WHILE (@i <= (SELECT MAX(idx) FROM @tempSalesOrderValue))
			BEGIN

				SET @customerSalesId = (SELECT customerId FROM @tempSalesOrderValue WHERE idx = @i)
				SET @retailUplift = (SELECT retailUplift FROM 	@tempSalesOrderValue WHERE idx = @i)	

				
				SET @logString = cast(@customerSalesId as nvarchar(50)) +' ' +cast(@brandAxeId as nvarchar(50)) + ' '+cast(@animationProductDetailId as nvarchar(50))
				EXEC [logFile]
					@logString,
					@logId

				SELECT @suma = dbo.countSale(@customerSalesId, NULL, @brandAxeId, NULL, @animationProductDetailId, @signatureId)				
			

				UPDATE @customers SET saleOrderValue = @suma WHERE customerId = @customerSalesId

				SET @i = @i + 1
			END



		EXEC [logFile]
			'------------------',
			@logId

		SELECT @logString = 'SALES COMPUTED: ' + 
		+ ISNULL( STUFF  (   (  
				 SELECT ', || GUID: ' + ISNULL(cast(customerId AS VARCHAR(max)),'') + ' SALES: ' + ISNULL(cast(saleOrderValue as varchar(50)),'0')
				 FROM @customers        
				 FOR XML PATH ('')  
				 ),1,1,''  
			 ),'-- none--')
		EXEC [logFile]
			@logString,
			@logId

		EXEC [logFile]
			'------------------',
			@logId



		-- compute allocation
		--DECLARE @alreadyAllocated int
		SET @alreadyAllocated = 0
		DECLARE @kurzorCustomer uniqueidentifier
		DECLARE @kurzorKapacita int
		DECLARE @kurzorFixed int
		DECLARE @quantity int
		DECLARE @kurzorSales float
		DECLARE @salesWithoutFixedSum float
		DECLARE @modifiedSales float
		DECLARE @zbytek int
		DECLARE @allocatedFromPrevious int
		DECLARE @allocatedWithinFixedCustomers int
		

		-- reset counter
		SET @allocatedWithinFixedCustomers = 0


		-- ****************************
		-- 1. with fixed allocation set
		DECLARE customersWithFixed CURSOR
		FOR SELECT customerId, capacity, fixedAllocation, customerName FROM @customers WHERE fixedAllocation >= 0 AND brandExclusion IS NULL OR brandExclusion <> 1 order by saleOrderValue desc
		OPEN customersWithFixed
		FETCH NEXT FROM customersWithFixed into @kurzorCustomer, @kurzorKapacita, @kurzorFixed, @customerName
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
			
			
			--- stop loop when left quantity is smaller then multiple
			IF @allocationQuantity - @alreadyAllocated < @multipleNormal BREAK			
			
			-- find the quantity to allocate
			IF @jeZbytek = 0
			BEGIN
		SET @logString = '@allocationQuantity: ' +cast(@allocationQuantity as nvarchar(50))+ '; @alreadyAllocated: ' + cast(@alreadyAllocated as nvarchar(50))
				EXEC [logFile]
					@logString,
					@logId
				SET @quantity = case when @kurzorKapacita > @kurzorFixed then @kurzorFixed else @kurzorKapacita end

				SET @quantity = case when @quantity > @allocationQuantity - @alreadyAllocated then @allocationQuantity - @alreadyAllocated else @quantity end

				SET @quantity = dbo.RoundMultiple(@quantity, @multipleNormal) * @multipleNormal 

			END
			ELSE
			BEGIN
				
				SELECT @allocatedFromPrevious = ISNULL(CalculatedAllocation,0) FROM dbo.CustomerAllocation WHERE IDCustomer = @kurzorCustomer AND IDAnimationProductDetail = @animationProductDetailId
				SET @quantity = case when @kurzorKapacita - @allocatedFromPrevious > @kurzorFixed then @kurzorFixed - @allocatedFromPrevious else @kurzorKapacita - @allocatedFromPrevious end
				SET @quantity = dbo.RoundMultipleUp(@quantity, @multipleNormal) * @multipleNormal
				
			END

			SELECT @logString = 'FIX ALLOCATION || AlreadyAllocated: ' + 
				ISNULL(cast(@alreadyAllocated as nvarchar(50)),'--') + ' Customer: ' +
				ISNULL(cast(@kurzorCustomer as nvarchar(50)),'--') +' ('+@customerName+')' + ' Capacity: ' + 
				ISNULL(cast(@kurzorKapacita as nvarchar(50)),'--') + ' Fixed Allocation: ' + 
				ISNULL(cast(@kurzorFixed as nvarchar(50)),'--') + ' QUANTITY TO ALLOCATE:  ' + 
				ISNULL(cast(@quantity as nvarchar(50)),'--') 
		
			EXEC [logFile]
				@logString,
				@logId


			-- allocate
			UPDATE @customers SET calculatedAllocationX = @quantity WHERE customerId = @kurzorCustomer

			-- increase already allocated
			SET @alreadyAllocated = @alreadyAllocated + @quantity
			SET @allocatedWithinFixedCustomers = @allocatedWithinFixedCustomers + @quantity
	
			FETCH NEXT FROM customersWithFixed into @kurzorCustomer, @kurzorKapacita, @kurzorFixed, @customerName
		END

		CLOSE customersWithFixed
		DEALLOCATE customersWithFixed



		-- *****************************
		-- 2. without fixed allocation

		-- sum of salesData
		SELECT @salesWithoutFixedSum = SUM(ISNULL(saleOrderValue,0)) FROM @customers WHERE fixedAllocation is null AND (brandExclusion <> 1 OR brandExclusion IS NULL)
		SET @modifiedSales = @salesWithoutFixedSum
		
		-- check if there is something to allocate 
		IF @allocationQuantity - @alreadyAllocated > @multipleNormal 
		BEGIN

			SET @logString = 'Allocated within fixed customers: ' + CAST(@allocatedWithinFixedCustomers as nvarchar(50))
			EXEC [logFile]
				@logString,
				@logId


			DECLARE customersWitouthFixed CURSOR
			FOR SELECT customerId, capacity, saleOrderValue, customerName FROM @customers WHERE fixedAllocation is null AND (brandExclusion <> 1 OR brandExclusion IS NULL) order by saleOrderValue desc
			OPEN customersWitouthFixed
			FETCH NEXT FROM customersWitouthFixed into @kurzorCustomer, @kurzorKapacita, @kurzorSales, @customerName
			
			WHILE @@FETCH_STATUS = 0
			BEGIN				
				
				--- stop loop when left quantity is smaller then multiple
				IF @allocationQuantity - @alreadyAllocated < @multipleNormal BREAK				

				-- no sales data or negative sales data => no allocation
				IF @kurzorSales IS NULL OR @kurzorSales <= 0
				BEGIN
					-- allocate
					UPDATE @customers SET calculatedAllocationX = 0 WHERE customerId = @kurzorCustomer
					
					SELECT @logString = 'NO FIX ALLOCATION || AlreadyAllocated: ' + 
						ISNULL(cast(ISNULL(@alreadyAllocated,0) as nvarchar(50)),'--') + ' Customer: ' +								
						ISNULL(cast(@kurzorCustomer as nvarchar(50)),'--') +' ('+@customerName+')' +  '  QUANTITY TO ALLOCATE: 0 (No Sales Data) ' 
		
					EXEC [logFile]
						@logString,
						@logId
					
				END
				ELSE
				BEGIN
					-- proportional allocation
					--SET @quantity = (@allocationQuantity - @alreadyAllocated) * ISNULL((@kurzorSales/@modifiedSales),1)
							
				
					-- find the quantity to allocate
					IF @jeZbytek = 0
					BEGIN
						SET @quantity = (@allocationQuantity - @allocatedWithinFixedCustomers) * ISNULL((@kurzorSales/@salesWithoutFixedSum),1)
						
						SET @quantity = case when @kurzorKapacita > @quantity then dbo.RoundMultiple(@quantity, @multipleNormal) else dbo.RoundMultiple(@kurzorKapacita, @multipleNormal) end
						
						SET @quantity = @quantity * @multipleNormal	

						-- check Complete allocation quantity
						
					
					END
					ELSE
					BEGIN
						SELECT @allocatedFromPrevious = ISNULL(CalculatedAllocation,0) FROM dbo.CustomerAllocation WHERE IDCustomer = @kurzorCustomer AND IDAnimationProductDetail = @animationProductDetailId
						SET @quantity = (@allocationQuantity -@allocatedWithinFixedCustomers ) * ISNULL((@kurzorSales/@salesWithoutFixedSum),1)
						
						SET @quantity = case when @kurzorKapacita - @allocatedFromPrevious > @quantity then dbo.RoundMultiple(@quantity, @multipleNormal) else dbo.RoundMultiple(@kurzorKapacita - @allocatedFromPrevious, @multipleNormal) end
						SET @quantity = @quantity * @multipleNormal	
					END
									

					SELECT @logString = 'NO FIX ALLOCATION || AlreadyAllocated: ' + 
					ISNULL(cast(ISNULL(@alreadyAllocated,0) as nvarchar(50)),'--') + ' Customer: ' +
					ISNULL(cast(@kurzorCustomer as nvarchar(50)),'--') +' ('+@customerName+')' +   ' Capacity: ' + 
					ISNULL(cast(@kurzorKapacita as nvarchar(50)),'--') + ' QUANTITY TO ALLOCATE:  ' + 
					ISNULL(cast(@quantity as nvarchar(50)),'--') 
			
					EXEC [logFile]
						@logString,
						@logId

					-- allocate
					UPDATE @customers SET calculatedAllocationX = @quantity WHERE customerId = @kurzorCustomer
					
					-- increase already allocated
					SET @alreadyAllocated = @alreadyAllocated + @quantity
					
				END

				FETCH NEXT FROM customersWitouthFixed into @kurzorCustomer, @kurzorKapacita, @kurzorSales, @customerName
			END

			CLOSE customersWitouthFixed
			DEALLOCATE customersWitouthFixed

			DECLARE @zbytekAllocated int
			SET @zbytekAllocated = 0
			SET @modifiedSales = @salesWithoutFixedSum

			-- zbytek
			SET @zbytek = @allocationQuantity - @alreadyAllocated 			
			
	

			While @zbytek - @zbytekAllocated >= @multipleNormal 
			BEGIN
				if (SELECT count(*) FROM @customers WHERE fixedAllocation is null AND capacity - calculatedAllocationX >= @multipleNormal AND (brandExclusion <> 1 OR brandExclusion IS NULL) AND saleOrderValue > 0) = 0
					break

				DECLARE zbytek CURSOR
				FOR SELECT customerId, capacity - calculatedAllocationX, saleOrderValue, customerName FROM @customers WHERE fixedAllocation is null AND capacity - calculatedAllocationX >= @multipleNormal AND (brandExclusion <> 1 OR brandExclusion IS NULL) AND saleOrderValue > 0 order by saleOrderValue desc, customerName asc
				OPEN zbytek
				FETCH NEXT FROM zbytek into @kurzorCustomer, @kurzorKapacita, @kurzorSales, @customerName
				
				
				set @logString = 'Remainder: ' + cast(@zbytek as nvarchar(25)) + ' -> remainder allocation'
				EXEC [logFile]
						@logString,
						@logId
	
				WHILE @@FETCH_STATUS = 0
				BEGIN	
					
					IF 	@zbytek - @zbytekAllocated < @multipleNormal BREAK					
				
					IF @kurzorSales IS NULL OR @kurzorSales = 0
					BEGIN
						UPDATE @customers SET calculatedAllocationX = 0 WHERE customerId = @kurzorCustomer
					END
					ELSE
					BEGIN	
						IF @jeZbytek = 0
						BEGIN	
							--SET @quantity = @zbytek * ISNULL((@kurzorSales/@modifiedSales),1)
							SET @quantity = ceiling((@zbytek) * ISNULL((cast(@kurzorSales as float)/cast(@salesWithoutFixedSum as float)),1))
							set @logString = cast(@quantity as nvarchar(50)) EXEC [logFile]	@logString,	@logId
							SET @quantity = case when @kurzorKapacita > @quantity then dbo.RoundMultipleUp(@quantity, @multipleNormal) else dbo.RoundMultipleUp(@kurzorKapacita,@multipleNormal) end
							set @logString = cast(@quantity as nvarchar(50)) EXEC [logFile]	@logString,	@logId
							SET @quantity = @quantity * @multipleNormal
							set @logString = cast(@quantity as nvarchar(50)) EXEC [logFile]	@logString,	@logId
							set @quantity = case when @quantity > @zbytek - @zbytekAllocated then @quantity - @multipleNormal else @quantity end
						END
						ELSE
						BEGIN
							SELECT @allocatedFromPrevious = ISNULL(CalculatedAllocation,0) FROM dbo.CustomerAllocation WHERE IDCustomer = @kurzorCustomer AND IDAnimationProductDetail = @animationProductDetailId
							SET @quantity = ceiling((@zbytek - @allocatedWithinFixedCustomers) * ISNULL((cast(@kurzorSales as float)/cast(@salesWithoutFixedSum as float)),1))
							--SET @quantity = (@zbytek - @allocatedWithinFixedCustomers ) * ISNULL((dbo.RoundMultipleUp(@kurzorSales,@salesWithoutFixedSum)),1)
							set @logString = cast(@quantity as nvarchar(50)) EXEC [logFile]	@logString,	@logId
							SET @quantity = case when @kurzorKapacita - @allocatedFromPrevious > @quantity then dbo.RoundMultipleUp(@quantity, @multipleNormal) else dbo.RoundMultipleUp(@kurzorKapacita - @allocatedFromPrevious, @multipleNormal) end
							set @logString = cast(@quantity as nvarchar(50)) EXEC [logFile]	@logString,	@logId
							SET @quantity = @quantity * @multipleNormal	
							set @logString = cast(@quantity as nvarchar(50)) EXEC [logFile]	@logString,	@logId
						END
						

						SELECT @logString = 'FROM REMAINDER || AlreadyAllocated: ' + 
						ISNULL(cast(ISNULL(@alreadyAllocated,0) as nvarchar(50)),'--') + ' Customer: ' +
						ISNULL(cast(@kurzorCustomer as nvarchar(50)),'--') +' ('+@customerName+')' +   ' Capacity: ' + 
						ISNULL(cast(@kurzorKapacita as nvarchar(50)),'--') + ' QUANTITY TO ALLOCATE:  ' + 
						ISNULL(cast(@quantity as nvarchar(50)),'--') 
				
						EXEC [logFile]
							@logString,
							@logId

						-- allocate
						UPDATE @customers SET calculatedAllocationX = calculatedAllocationX + @quantity WHERE customerId = @kurzorCustomer
						
						-- increase already allocated
						SET @zbytekAllocated = @zbytekAllocated + @quantity						
						SET @alreadyAllocated = @alreadyAllocated + @quantity		
						SET @modifiedSales = @modifiedSales - @kurzorSales	
										
						
					END
					FETCH NEXT FROM zbytek into @kurzorCustomer, @kurzorKapacita, @kurzorSales, @customerName
				END

				CLOSE zbytek
				DEALLOCATE zbytek

			 
			END
		END

		
		-- create table #temp
		DECLARE @tempCustomerAllocationAlreadyIn table
			( tempID uniqueidentifier			
			)

		-- records already exist
		INSERT INTO @tempCustomerAllocationAlreadyIn 
			SELECT tempId
				FROM @customers as a inner join CustomerAllocation as b on (a.customerId = b.IDCustomer AND @animationProductDetailId = b.IDAnimationProductDetail)



		IF @jeZbytek = 0
		BEGIN
			-- update 				
			UPDATE CustomerAllocation SET CalculatedAllocation =  ISNULL(a.calculatedAllocationX,0), ModifiedDate = GETDATE(), ModifiedBy = 'allocation script e'
				FROM CustomerAllocation  as b inner join @customers as a ON (a.customerId = b.IDCustomer AND b.IDAnimationProductDetail = @animationProductDetailId)
					
				
			INSERT INTO CustomerAllocation (IDCustomer, IDAnimationProductDetail, CalculatedAllocation, ModifiedBy, ModifiedDate)
				SELECT	customerId, @animationProductDetailId, ISNULL(calculatedAllocationX,0), 'allocation script c', getdate()
					FROM @customers
						WHERE tempID not in (select tempID FROM @tempCustomerAllocationAlreadyIn)
		END
		ELSE
		BEGIN
			IF @includeWarehouse = 0
			BEGIN
				-- update 
				UPDATE CustomerAllocation SET CalculatedAllocation = ISNULL(CalculatedAllocation,0) + ISNULL(a.calculatedAllocationX,0), ModifiedDate = GETDATE(), ModifiedBy = 'allocation script b'
					FROM  @customers as a inner join CustomerAllocation as b ON (a.customerId = b.IDCustomer AND b.IDAnimationProductDetail = @animationProductDetailId)
			END
			ELSE
			BEGIN
					-- update 
				UPDATE CustomerAllocation SET CalculatedAllocation = ISNULL(a.calculatedAllocationX,0), ModifiedDate = GETDATE(), ModifiedBy = 'allocation script b'
					FROM  @customers as a inner join CustomerAllocation as b ON (a.customerId = b.IDCustomer AND b.IDAnimationProductDetail = @animationProductDetailId)
			END
		END



	SET @zbytekNotAllocated = @zbytek - @zbytekAllocated
	SET @alreadyAllocated = ISNULL(@alreadyAllocated, 0)
		
END