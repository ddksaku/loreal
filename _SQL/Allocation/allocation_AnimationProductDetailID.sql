IF OBJECT_ID (N'uf_allocate_anPrDeID') IS NOT NULL
    DROP procedure uf_allocate_anPrDeID;
GO

-- *****************************************
-- ** ALLOCATION (AnimationProductDetailId)
-- **
-- **
-- *****************************************

CREATE procedure [dbo].[uf_allocate_anPrDeID] 
	(
	  @animationProductDetailId uniqueidentifier
	 ,@quantity int	 
	 ,@logId uniqueidentifier
	 ,@jeZbytek bit = 0
	 ,@allocated int output
	 ,@zbytekNotAllocated int output
	 ,@includeWarehose bit = 1
	 )
AS
BEGIN

	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)
	DECLARE @logString nvarchar(max)
	DECLARE @alreadyAllocated int
	SET @alreadyAllocated = 0

	DECLARE @animationProductId uniqueidentifier
	SELECT @animationProductId = IDAnimationProduct FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId

	DECLARE @animationId uniqueidentifier
	SELECT @animationId = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId

	DECLARE @brandAxeId uniqueidentifier
	SELECT @brandAxeId = IDBrandAxe  FROM AnimationProduct WHERE ID = 
		(select IDAnimationProduct FROM AnimationProductDetail WHERE ID = @animationProductDetailId)

	DECLARE @salesAreaId uniqueidentifier
	SELECT @salesAreaId = IDSalesArea FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId

	-- item type
	DECLARE @itemTypeId uniqueidentifier
	SELECT @itemTypeId = IDItemType FROM AnimationProduct WHERE ID = @animationProductId

	DECLARE @signatureId uniqueidentifier
	SELECT @signatureId = IDSignature FROM AnimationProduct WHERE ID = @animationProductId

	-- brandAxe in animation is empty
	IF @brandAxeId IS NULL
	BEGIN			
		SELECT @brandAxeId = ID FROM BrandAxe WHERE IDSignature = @signatureId AND Brand = 1
	END

	DECLARE @multipleNormal int
	DECLARE @multipleWarehouse int

	SELECT @multipleNormal = [Value] FROM Multiple 	WHERE ID = (SELECT IDMultipleNormal FROM AnimationProduct WHERE ID = @animationProductId)
	SELECT @multipleWarehouse = [Value] FROM Multiple WHERE ID = (SELECT IDMutlipleWarehouse FROM AnimationProduct WHERE ID = @animationProductId)
	
	-- allocation table
	declare @allocationTable table
	(
		 groupId uniqueidentifier
		,fixedQuantity int -- group demand
		,alreadyAllocated int
		,allocated int -- will be counted 
		,groupSale float
		,warehouseAllocation bit
		,itemTypeId uniqueidentifier
		,tempID uniqueidentifier
		,zbytek int
	)


	-- insert groups
	INSERT INTO @allocationTable (groupId)
		SELECT IDCustomerGroup FROM AnimationCustomerGroup as a inner join dbo.CustomerGroup as b on (a.IDCustomerGroup = b.ID)
			WHERE IDAnimation = @animationId AND b.IncludeInSystem = 1


		
	-- fixed quantity
	UPDATE @allocationTable SET fixedQuantity = ISNULL(b.ManualFixedAllocation, b.SystemFixedAllocation)
		FROM @allocationTable as a inner join CustomerGroupAllocation as b
			ON (@animationProductDetailId = b.IDAnimationProductDetail AND a.groupId = b.IDCustomerGroup)

	

	-- count sales
	UPDATE @allocationTable SET groupSale = dbo.countSale(NULL, groupId, @brandAxeId ,@salesAreaId, @animationProductDetailId, @signatureId)
	
	

	-- warehouse allocation?
	UPDATE @allocationTable SET warehouseAllocation = b.WarehouseAllocation
		FROM @allocationTable as a inner join CustomerGroupItemType as b on (a.groupId = b.IDCustomerGroup AND @itemTypeId = b.IDItemType) 



	IF @includeWarehose = 0
		BEGIN
			--exclude groups with warehouse account
			DECLARE @groupsWarehouse2 TABLE
				( groupId uniqueidentifier	)

				-- find out already existing records in CustomerGroupAllocation table
				INSERT INTO @groupsWarehouse2
					SELECT a.groupId FROM @allocationTable as a left join CustomerGroupAllocation as b 
						ON(@animationProductDetailId = b.IDAnimationProductDetail AND a.groupId = b.IDCustomerGroup)
							WHERE wareHouseAllocation = 1				

				DELETE FROM @allocationTable WHERE groupID IN (SELECT groupID FROM @groupsWarehouse2)
				
		END

	-- generate temp GUID
	UPDATE @allocationTable SET tempID = NEWID()


	-- **** CUSTOM GROUPY S FIXED ALLOCATION ******************************************************************
	DECLARE customerGroupCursor CURSOR
	FOR select groupId, groupSale, fixedQuantity from @allocationTable 
		where fixedQuantity > 0 
		ORDER BY groupSale desc

	DECLARE @kurzorCustomerGroupId uniqueidentifier
	DECLARE @kurzorFixedQunatity int
	DECLARE @kurzorSale float
	DECLARE @toAllocate int
	DECLARE @output int
	DECLARE @zbytek int
	DECLARE @zbytekCG int
	DECLARE @allocatedWithinCG int
	SET @zbytekCG = 0

	OPEN customerGroupCursor
    FETCH NEXT FROM customerGroupCursor INTO @kurzorCustomerGroupId, @kurzorSale, @kurzorFixedQunatity;
	
    WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @quantity - @alreadyAllocated < @multipleNormal BREAK		

		-- just log
		SET @logString = @newLineChar + @newLineChar + '====================' + @newLineChar + 'CustomerGroupID: '
			 + cast(@kurzorCustomerGroupId as nvarchar(50)) + @newLineChar +  ' AnimationProductDetailId: ' + cast(@animationProductDetailId as nvarchar(50))+ @newLineChar + ' To allocate: ' 
			 + cast(@quantity - @alreadyAllocated as nvarchar(25)) + @newLineChar + '===================='
		EXEC [logFile]
			@logString,
			@logId

		-- alokuju fixed alllocation pokud je dostupna	
	
		IF @jeZbytek = 1
		BEGIN
			SELECT @allocatedWithinCG = SUM(ISNULL(CalculatedAllocation,0)) FROM dbo.CustomerAllocation 
				WHERE IDAnimationProductDetail = @animationProductDetailId AND IDCustomer in 
				(SELECT ID FROM Customer WHERE IDCustomerGroup = @kurzorCustomerGroupId)	
			SET @toAllocate = @kurzorFixedQunatity - @allocatedWithinCG
			SET @logString = @newLineChar + 'Allocated already within custom group: ' + cast(@allocatedWithinCG as nvarchar(50))
			EXEC [logFile]
				@logString,
				@logId
		END
		ELSE
		BEGIN
			SET @toAllocate = case when @quantity - @alreadyAllocated > @kurzorFixedQunatity 
				THEN @kurzorFixedQunatity else @quantity - @alreadyAllocated end	
		END

		SET @logString = @newLineChar + 'To allocate: ' + cast(@toAllocate as nvarchar(50))
		EXEC [logFile]
			@logString,
			@logId
		


		-- allocation
		execute uf_allocate_anPrDeIDcuGrID 
			@kurzorCustomerGroupId
		   ,NULL
		   ,@animationProductDetailId
		   ,@logId
		   ,@toAllocate
		   ,@output OUTPUT		
		   ,@zbytek OUTPUT
		   ,@jeZbytek
		   ,@includeWarehose
			
		-- increase already allocated qunatity (by fixed allocation)
		SET @alreadyAllocated = @alreadyAllocated + @toAllocate--ISNULL(@output, 0)

		-- zbytek z alokace a alokovane mnozstvi
		UPDATE @allocationTable SET zbytek = @zbytek, allocated = ISNULL(@output,0) WHERE groupID = @kurzorCustomerGroupId 
		
		-- zbytek
		SET @zbytekCG = @zbytekCG + @zbytek

		-- just log
		SET @logString = @newLineChar + 'Custom Group: ' + cast(@kurzorCustomerGroupId as nvarchar(50)) + ' ; Allocated: ' + cast(@output as nvarchar(50)) + '; Zbytek: ' + cast(@zbytek as nvarchar(50)) + @newLineChar
		EXEC [logFile]
			@logString,
			@logId

		FETCH NEXT FROM customerGroupCursor INTO @kurzorCustomerGroupId, @kurzorSale, @kurzorFixedQunatity;
	END
	CLOSE customerGroupCursor;
    DEALLOCATE customerGroupCursor;
	-- *********************************************************************************************************	

	-- *** CUSTOMER GROUPS BEZ FIXED ************************************
	-- there is something to allocate
	IF @quantity - @alreadyAllocated > @multipleNormal
	BEGIN
		-- qunatity left to allocate
		SET @toAllocate = @quantity - @alreadyAllocated
		
		-- just log
		SET @logString = @newLineChar + @newLineChar + '====='+'Customers from Customer Groups without fixed allocation'+'===='
		EXEC [logFile]
			@logString,
			@logId		
	
		
		-- allocation
		execute uf_allocate_anPrDeIDcuGrID 
			NULL
		   ,@animationId
		   ,@animationProductDetailId
		   ,@logId
		   ,@toAllocate
		   ,@output OUTPUT
		   ,@zbytek OUTPUT	
		   ,@jeZbytek
		   ,@includeWarehose
		   
	
		-- increase already allocated quantity
		SET @alreadyAllocated = @alreadyAllocated + ISNULL(@output, 0)

		-- zbytek
		SET @zbytekCG = @zbytekCG + @zbytek

	END

	-- UPDATE / INSERT CustomerGroupAllocation
		-- CASE 1: Customer Group dont't have warehouse allocation => finihs the allocation
		DECLARE @customerAllocationAlreadyIn TABLE 
		(tempID uniqueidentifier, 
		 ID uniqueidentifier,
		 groupID uniqueidentifier, 
		 calculatedAllocation int)
		
		-- find out already existing records in CustomerGroupAllocation table
		INSERT INTO @customerAllocationAlreadyIn (tempID, ID, groupID)
			SELECT tempID, b.ID, a.groupId FROM @allocationTable as a left join CustomerGroupAllocation as b 
				ON(@animationProductDetailId = b.IDAnimationProductDetail AND a.groupId = b.IDCustomerGroup)
					WHERE (wareHouseAllocation = 0 OR wareHouseAllocation IS NULL)		
		
		-- count sum of allocated 
		UPDATE @customerAllocationAlreadyIn SET calculatedAllocation = 
			(select sum(ISNULL(CalculatedAllocation,0)) from CustomerAllocation WHERE IDAnimationProductDetail = @animationProductDetailId 
			  AND IDCustomer IN (SELECT ID FROM Customer WHERE IDCustomerGroup = groupID))

		-- update existing
		UPDATE CustomerGroupAllocation SET CalculatedAllocation = ISNULL(a.calculatedAllocation,0), ModifiedDate = GETDATE(), ModifiedBy = 'allocation script'
			FROM @customerAllocationAlreadyIn as a inner join CustomerGroupAllocation as b on (a.ID = b.ID)

		-- insert new ones
		INSERT INTO CustomerGroupAllocation (IDCustomerGroup, IDAnimationProductDetail, CalculatedAllocation, ModifiedBy, ModifiedDate)
			SELECT groupId, @animationProductDetailId, calculatedAllocation, 'allocation script', GETDATE()
				FROM @customerAllocationAlreadyIn
					WHERE ID IS NULL




		IF @includeWarehose = 1
		BEGIN
			-- CASE 2: Custom Group got warehouse allocation => recalculate		
			-- groupy
			DECLARE @groupsWarehouse TABLE
				( groupId uniqueidentifier,
				  ID uniqueidentifier,
				  tempId uniqueidentifier,
				  calculatedAllocationX int,
				  fixedAllocation int,
				  groupSale int
				)

			-- find out already existing records in CustomerGroupAllocation table
			INSERT INTO @groupsWarehouse (tempID, ID, groupID, fixedAllocation, groupSale)
				SELECT tempID, b.ID, a.groupId, a.fixedQuantity, a.groupSale FROM @allocationTable as a left join CustomerGroupAllocation as b 
					ON(@animationProductDetailId = b.IDAnimationProductDetail AND a.groupId = b.IDCustomerGroup)
						WHERE wareHouseAllocation = 1
		

			IF (SELECT COUNT(*) FROM @groupsWarehouse) > 0 
			BEGIN

				DECLARE groupsWarehouseCursor CURSOR	
				FOR select groupId, fixedAllocation from @groupsWarehouse order by groupSale desc			

				DECLARE @cursorGroup uniqueidentifier
				DECLARE @cursorQuatity int
				DECLARE @cursorFixed int
				DECLARE @sqlString nvarchar(max)				

				SET @alreadyAllocated = 0

				OPEN groupsWarehouseCursor
				FETCH NEXT FROM groupsWarehouseCursor INTO @cursorGroup, @cursorFixed;
				WHILE @@FETCH_STATUS = 0
				BEGIN	
						
					SELECT @cursorQuatity = sum(ISNULL(CalculatedAllocation,0)) from CustomerAllocation WHERE IDAnimationProductDetail = @animationProductDetailID 
					  AND IDCustomer IN (SELECT ID FROM Customer WHERE IDCustomerGroup = @cursorGroup)
			
					SET @logString = '=====' +  @newLineChar + 'Warehouse multiple: ' + cast(@multipleWarehouse as nvarchar(50))
							+ ' Quantity: ' + cast(ISNULL(@cursorQuatity, 0) as nvarchar(50))
					EXEC [logFile]
						@logString,
						@logId

					-- check quantity against capacity and determine the quantity to allocate
					IF dbo.countGroupCapacity(@animationProductID, @cursorGroup) <  dbo.RoundWarehouseMultiple(@cursorQuatity, @multipleWarehouse)
						SET @toAllocate = dbo.RoundWarehouseMultiple(@cursorQuatity, @multipleWarehouse) - @multipleWarehouse
					ELSE
						SET @toAllocate = dbo.RoundWarehouseMultiple(@cursorQuatity, @multipleWarehouse)				
						
					SET @toAllocate = @toAllocate * @multipleWarehouse;

					
					-- check quantity against customer group fixed allocation
					IF @cursorFixed < @toAllocate
						SET @toAllocate = @toAllocate - @multipleWarehouse;

					-- check quantity against qunatity to allocate
					IF @quantity - @alreadyAllocated < @toAllocate
						SET @toAllocate = @quantity - @alreadyAllocated

					
					SET @logString = @newLineChar + 'To Allocate ' + cast(@toAllocate as nvarchar(50));
					EXEC [logFile]
						@logString,
						@logId

					-- reallocation
					execute uf_allocate_anPrDeIDcuGrID 
						@cursorGroup
					   ,NULL
					   ,@animationProductDetailID
					   ,@logId
					   ,@toAllocate
					   ,@output OUTPUT
					   ,@zbytek OUTPUT
					   ,0
					   ,1

					 -- zbytek z alokace
					 UPDATE @allocationTable SET zbytek = @zbytek WHERE groupID = @kurzorCustomerGroupId 

					--
					 SET @alreadyAllocated  = @alreadyAllocated + @output			
					
					 -- set calculated quantity
					 UPDATE @groupsWarehouse SET calculatedAllocationX = @output /*@toAllocate*/ WHERE groupId = @cursorGroup
		 
					 FETCH NEXT FROM groupsWarehouseCursor INTO @cursorGroup, @cursorFixed;
				END
				CLOSE groupsWarehouseCursor
				DEALLOCATE groupsWarehouseCursor

				-- recalculate other then warehouse accounts
				SET @toAllocate = @quantity - @alreadyAllocated;
				
				SET @logString = @newLineChar + 'TO_ALLOCATE_NO_WAREHOUSE' + cast(@toAllocate as nvarchar(50));
				EXEC [logFile]
					@logString,
					@logId
				
				IF @toAllocate > 0
				BEGIN
					EXEC uf_allocate_anPrDeID 			
					  @animationProductDetailId 
					 ,@toAllocate 	 
					 ,@logId 
					 ,0
					 ,@allocated  output
					 ,@zbytekNotAllocated  output
					 ,0			
				END
				ELSE
				BEGIN
					 -- zero allocation for all customers from nowarehouse groups
					DECLARE @groupsNoWarehouse TABLE
				   ( groupId uniqueidentifier )

					-- find out already existing records in CustomerGroupAllocation table
					INSERT INTO @groupsNoWarehouse
						SELECT groupId FROM @allocationTable WHERE wareHouseAllocation IS NULL OR wareHouseAllocation  <> 1

				
					-- update customer allocation
					UPDATE CustomerAllocation SET CalculatedAllocation = 0
						WHERE IDAnimationProductDetail = @animationProductDetailId AND IDCustomer IN 
						(SELECT ID FROM Customer WHERE IDCustomerGroup IN (SELECT groupId FROM @groupsNoWarehouse))
			
				END	

			END
		END
			
		-- INSERT / UPDATE CutomerGroupAllocation
		-- recalculated values
/*
		UPDATE @groupsWarehouse SET calculatedAllocationX = 
			(select sum(ISNULL(CalculatedAllocation,0)) from CustomerAllocation WHERE IDAnimationProductDetail = @animationProductDetailID 
			  AND IDCustomer IN (SELECT ID FROM Customer WHERE IDCustomerGroup = groupID))		
*/
	--	IF @jeZbytek = 0
		--BEGIN
			-- update existing
			UPDATE CustomerGroupAllocation SET CalculatedAllocation = ISNULL(a.calculatedAllocationX,0), ModifiedDate = GETDATE(), ModifiedBy = 'allocation script'
				FROM @groupsWarehouse as a inner join CustomerGroupAllocation as b on (a.ID = b.ID)

			-- insert new ones
			INSERT INTO CustomerGroupAllocation (IDCustomerGroup, IDAnimationProductDetail, CalculatedAllocation, ModifiedBy, ModifiedDate)
				SELECT groupId, @animationProductDetailID, ISNULL(calculatedAllocationX,0), 'allocation script', GETDATE()
					FROM @groupsWarehouse
						WHERE ID IS NULL		

		-- output
		SET @allocated = @alreadyAllocated
		SET @zbytekNotAllocated = @zbytekCG

		-- pokud je zbytek mensi nez multiple, nastavime ho jako nulu, cimz se vyhneme dalsimu behu cyklu
		IF @zbytekNotAllocated < @multipleNormal SET @zbytekNotAllocated = 0



	-- insert not existing customerAllocations
	DECLARE @allCustomers TABLE 
	(customerId uniqueidentifier)


    INSERT INTO @allCustomers (customerId)
		SELECT [ID] FROM Customer WHERE IncludeInSystem = 1 AND Deleted = 0 AND IDCustomerGroup IN 
		(SELECT a.IDCustomerGroup FROM [dbo].[AnimationCustomerGroup] as a inner join CustomerGroupAllocation as b 
		on (a.IDCustomerGroup = b.IDCustomerGroup AND @animationProductDetailId = b.IDAnimationProductDetail))					


		INSERT INTO CustomerAllocation (IDCustomer, IDAnimationProductDetail, CalculatedAllocation, ModifiedBy, ModifiedDate)
			select customerId, @animationProductDetailId, 0, 'allocation script a', getdate() from @allCustomers where customerId not in (select IDCustomer from dbo.CustomerAllocation where IDAnimationProductDetail = @animationProductDetailId)
	
		

END