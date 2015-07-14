if object_id('tr_animation_capacityChanged') > 0
	drop trigger tr_animation_capacityChanged
go

create trigger tr_animation_capacityChanged
on Animation
after update
as
begin

	IF @@ROWCOUNT = 0 RETURN 

	declare @animationTypeOld uniqueidentifier
	declare @animaitonTypeNew uniqueidentifier
	declare @priorityOld uniqueidentifier
	declare @priorityNew uniqueidentifier
	declare @animationID uniqueidentifier
	DECLARE @itemTypeID uniqueidentifier		
	DECLARE @animationTypeID uniqueidentifier	
	DECLARE @priorityID uniqueidentifier	
	DECLARE @errorMsg nvarchar(max)
	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)

	select @animationTypeOld = IDAnimationType from deleted
	select @animaitonTypeNew = IDAnimationType from inserted
	select @priorityOld = IDPriority from deleted
	select @priorityNew = IDPriority from inserted
	
	select @animationID = ID from inserted		

	if @animationTypeOld = @animaitonTypeNew AND @priorityOld = @priorityNew
	begin
		-- nothing interestring happened
		
		return
	end
	else
	begin	
		-- now fixed allocation can exceed capacity 
		-- (customer fixed vs customer capacity)
		-- (customer group fixed vs group capacity)
		-- (alocation quantity vs overall capacity)	
		
		

		-- get animation product details to iterate over them
		declare @animationProductDetails table
		(animationproductDetail uniqueidentifier, iterator int identity(1,1))

		insert into @animationProductDetails
			select ID from dbo.AnimationProductDetail 
				where IDAnimationProduct IN
					(SELECT ID FROM dbo.AnimationProduct where IDAnimation = @animationID)
		

		-- loop over animation products details
		declare @animationProductDetailID uniqueidentifier
		declare @iterator int
		declare @productCode nvarchar(max)
		declare @iteratorCustAllocation int
		declare @iteratorGroupAllocation int
		declare @overallCapacity int
		declare @alocationQuantity int
		declare @groupName nvarchar(255)
		declare @customerName nvarchar(255)
		select @iterator = max(iterator) from @animationProductDetails
		
		-- to avoid joining null string
		set @errorMsg = ''

		while @iterator > 0
		begin				
			
			-- create tables
			declare @customerAllocations table (ID uniqueidentifier, customerID uniqueidentifier, fixed int, iterator int identity(1,1))
			declare @customerGroupAllocations table (ID uniqueidentifier, groupID uniqueidentifier, manualFixed int, systemFixed int, iterator int identity(1,1))			

			-- get animation product id
			select @animationProductDetailID = animationproductDetail from @animationProductDetails
				where iterator = @iterator
				
			-- get itemTypeID
			SELECT @itemTypeID = IDItemType FROM dbo.AnimationProduct WHERE ID =
			 (SELECT IDAnimationProduct from dbo.AnimationProductDetail where ID = @animationProductDetailID)
			 
			-- get customer groups 
			DECLARE @groups TABLE (groupID uniqueidentifier, capacity int, groupName nvarchar(255))

			INSERT INTO @groups (groupID, groupName)
				SELECT a.IDCustomerGroup, b.Name FROM dbo.AnimationCustomerGroup as a 
				inner join dbo.CustomerGroup as b on (a.IDCustomerGroup = b.ID)
				WHERE IDAnimation = @animationID

			-- get customers and their capacities
			DECLARE @customers TABLE (customerID uniqueidentifier, capacity int, 
								cusGroupID uniqueidentifier, customerName nvarchar(255))

			INSERT INTO @customers (customerID, cusGroupID, customerName)
				SELECT ID, IDCustomerGroup, Name FROM Customer
					WHERE IDCustomerGroup IN (SELECT groupID FROM @groups)

			-- update their capacities
			UPDATE @customers SET capacity = b.Capacity
				FROM @customers as a inner join dbo.CustomerCapacity as b 
					ON (a.customerID = b.IDCustomer 
						AND b.IDAnimationType = @animaitonTypeNew  -- new one
						AND b.IDPriority = @priorityNew -- new one
						AND b.IDItemType = @itemTypeId)

			-- update group capacities
			UPDATE @groups set capacity = 
			(select sum(isnull(capacity,0)) from @customers where cusGroupID = groupID) 			 
			 
				
			-- get product Code
			select @productCode = c.MaterialCode
				from dbo.AnimationProductDetail	as a 
				inner join dbo.AnimationProduct as b on (a.IDAnimationProduct = b.ID)
				inner join dbo.Product as c on (b.IDProduct = c.ID)
				where a.ID = @animationProductDetailID

			-- fill tables
			insert into @customerAllocations (ID, customerID, fixed)
				select ID, IDCustomer, FixedAllocation 
					from dbo.CustomerAllocation 
						where IDAnimationProductDetail = @animationProductDetailID

			insert into @customerGroupAllocations (ID, groupID, manualFixed, systemFixed)
				select ID, IDCustomerGroup, ManualFixedAllocation, SystemFixedAllocation 
					from dbo.CustomerGroupAllocation
						where IDAnimationProductDetail = @animationProductDetailID

			-- check customer allocations
			select @iteratorCustAllocation = max(iterator) from @customerAllocations
			declare @capacity int
			declare @fixed int

			while @iteratorCustAllocation > 0
			begin
				
				-- find fixed allocation
				select @fixed = fixed from @customerAllocations	
					where iterator = @iteratorCustAllocation

				-- find customer capacity
				select @capacity = capacity, @customerName = customerName from @customers
					where customerID = (select customerID from @customerAllocations where iterator = @iteratorCustAllocation)

				set @capacity = isnull(@capacity,0)

				-- check condition
				if @fixed > @capacity
				begin
			
					SET @errorMsg = @errorMsg + @newLineChar +
						dbo.uf_getSystemMessage('tr_AnimationCapacityChangedCustomer', @customerName, cast(@fixed as nvarchar(50)), cast(@capacity as nvarchar(50)), null, null, null, null, null)
					
				end

				-- decrease iterator
				set @iteratorCustAllocation	= @iteratorCustAllocation - 1
				
			end

			-- check customer groups allocations
			select @iteratorGroupAllocation = max(iterator) from @customerGroupAllocations
			declare @groupCapacity int
			declare @groupManualFixed int
			declare @groupSystemFixed int

			while @iteratorGroupAllocation > 0
			begin
		
				-- find group manual fixed
				select @groupManualFixed = manualFixed from @customerGroupAllocations
					where iterator = @iteratorGroupAllocation

				-- find system fixed 
				select @groupSystemFixed = systemFixed from @customerGroupAllocations
					where iterator = @iteratorGroupAllocation

				-- find capacity
				select @groupCapacity = capacity, @groupName = groupName from @groups
					where groupID = (select groupID from @customerGroupAllocations where iterator = @iteratorGroupAllocation)

				set @groupCapacity = isnull(@groupCapacity, 0)

				-- check conditions (2)
				if @groupManualFixed > @groupCapacity
				begin
					
					SET @errorMsg = @errorMsg + @newLineChar +
						dbo.uf_getSystemMessage('tr_AnimationCapacityChangedGroupSystem', @groupName, cast(@groupManualFixed as nvarchar(50)), cast(@groupCapacity as nvarchar(50)), null, null, null, null, null)
					
				end
				
				if @groupSystemFixed > @groupCapacity
				begin
					
					SET @errorMsg = @errorMsg + @newLineChar +
						dbo.uf_getSystemMessage('tr_AnimationCapacityChangedGroupManual', @groupName, cast(@groupSystemFixed as nvarchar(50)), cast(@groupCapacity as nvarchar(50)), null, null, null, null, null)
					
				end


				-- decrease iterator
				set @iteratorGroupAllocation = @iteratorGroupAllocation - 1		

			end


			-- check allocation quantity against overall capacity
			SELECT @overallCapacity = sum(isnull(capacity,0)) from @customers
			set @overallCapacity = isnull(@overallCapacity, 0)
			SELECT @alocationQuantity = AllocationQuantity from dbo.AnimationProductDetail
				where ID = @animationProductDetailID
				
			set @alocationQuantity = isnull(@alocationQuantity, 0)	
				
			if 	@alocationQuantity > @overallCapacity
			begin
				
				SET @errorMsg = @errorMsg + @newLineChar +
						dbo.uf_getSystemMessage('tr_AnimationCapacityChangedAllocation', @productCode, cast(@alocationQuantity as nvarchar(50)), cast(@overallCapacity as nvarchar(50)), null, null, null, null, null)
					
			end
			

			-- clear tables
			delete @customerAllocations
			delete @customerGroupAllocations
			delete @groups
			delete @customers

			set @iterator = @iterator - 1

		end	


	end
	
	-- raise error
	if @errorMsg <> ''
	begin
		RAISERROR (@errorMsg, 16, 27)
		ROLLBACK TRAN
		RETURN		
	end

end