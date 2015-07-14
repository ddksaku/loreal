if object_id('tr_animationProduct_itemType') > 0
	drop trigger tr_animationProduct_itemType
go

create trigger tr_animationProduct_itemType
on dbo.AnimationProduct
after update
as
begin

	IF @@ROWCOUNT = 0 RETURN 


	declare @itemTypeNew uniqueidentifier
	declare @itemTypeOld uniqueidentifier
	declare @errorMsg nvarchar(max)
	declare @animationProductID uniqueidentifier
	declare @animationTypeID uniqueidentifier
	declare @animationID uniqueidentifier
	declare @priorityID uniqueidentifier
	declare @newLineChar char(2)
	set @newLineChar = CHAR(13) + CHAR(10)
	
	select @itemTypeNew = IDItemType from inserted
	select @itemTypeOld = IDItemType from deleted
	select @animationProductID = ID from inserted
	select @animationID = IDAnimation from inserted
	select @priorityID = IDPriority from dbo.Animation where ID = @animationID
	select @animationTypeID = IDAnimationType from dbo.Animation where ID = @animationID
	
	if @itemTypeNew = @itemTypeOld
	begin
			-- nothing happened
			return		
	end
	else
	begin
	
		-- get animation product details to iterate over them
		declare @animationProductDetails table
		(animationproductDetail uniqueidentifier, iterator int identity(1,1))

		insert into @animationProductDetails
			select ID from dbo.AnimationProductDetail 
				where IDAnimationProduct = @animationProductID
		
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
						AND b.IDAnimationType = @animationTypeID  
						AND b.IDPriority = @priorityID 
						AND b.IDItemType = @itemTypeNew) -- new one

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
		RAISERROR (@errorMsg, 16, 28)
		ROLLBACK TRAN
		RETURN		
	end

end	