if object_id('tr_Customer_CategoryChanged') > 0
	drop trigger tr_Customer_CategoryChanged
go

create trigger tr_Customer_CategoryChanged
on dbo.CustomerCategory
after update, delete
as
begin

	IF @@ROWCOUNT = 0 RETURN 	

	declare @errorMsg nvarchar(max)	
	declare @newLineChar char(2)
	set @newLineChar = CHAR(13) + CHAR(10)

	-- check, if there will be some affected animations
	declare @categoryOld uniqueidentifier
	declare @customerID uniqueidentifier
	declare @customerGroupID uniqueidentifier
	
	select @categoryOld = IDCategory, @customerID = IDCustomer from deleted	
	select @customerGroupID = IDCustomerGroup from dbo.Customer where ID = @customerID
	
	-- to avoid joining null string
	set @errorMsg = ''
	
	declare @conditions varchar(max)
	set @conditions = N'd.Status not in (5, 6) and b.IDCategory = ''' + cast(@categoryOld as nvarchar(55)) + ''''

	exec up_checkCapacitiesAfterUpdate
		@customerID 
	   ,@conditions 	
	   ,@errorMsg  OUTPUT


	/*
	-- **
	-- ** CHECK CUSTOMER FIXED ALLOCATIONS
	-- **
	declare @customerAllocations table 
		(   allocationID uniqueidentifier
		  , animatPorduDetailID uniqueidentifier
		  , productID uniqueidentifier
		  , capacity int
		  , fixed int
		  , animationID uniqueidentifier 
		  , iterator int identity(1,1)
		   )
	
	insert into @customerAllocations (allocationID, fixed, animatPorduDetailID, capacity)
		select c.ID, FixedAllocation, IDAnimationProductDetail, dbo.calculate_TotalCapacityCustomer(c.ID)
			from dbo.CustomerAllocation  as c
				inner join dbo.AnimationProductDetail as a on (c.IDAnimationProductDetail = a.ID)
				inner join dbo.AnimationProduct as b on (a.IDAnimationProduct = b.ID)  
				inner join dbo.Animation as d on (b.IDAnimation = d.ID)
				where IDCustomer = @customerID 
					and FixedAllocation is not null and FixedAllocation > dbo.calculate_TotalCapacityCustomer(c.ID)
					and d.Status not in (5, 6)
					and b.IDCategory = @categoryOld
	
	declare @countOfFixedErrors int
	select @countOfFixedErrors = count(*) from @customerAllocations
		where fixed is not null and fixed > capacity
		
	if @countOfFixedErrors > 0
	begin
	
		set @errorMsg = @errorMsg + 'Some of customer fixed allocations exceed their capacities' + @newLineChar
	
			
		update @customerAllocations set animationID = c.IDAnimation, productID = c.IDProduct
			from @customerAllocations as a
			 inner join dbo.AnimationProductDetail as b on (a.animatPorduDetailID = b.ID)
			 inner join dbo.AnimationProduct as c on (b.IDAnimationProduct = c.ID)
			 
		 declare @iterator int
		 declare @animationID uniqueidentifier
		 declare @productID uniqueidentifier
		 declare @animationName nvarchar(255)
		 declare @productName nvarchar(255)
		 declare @fixedIterator int
		 declare @capacityIterator int
		 select @iterator = max(iterator) from 	@customerAllocations 
	
		while @iterator > 0
		begin
			
			select @animationID = animationID, @fixedIterator = fixed, @capacityIterator = capacity,
					@productID = productID from @customerAllocations
				where iterator = @iterator
		
			select @animationName = Name from dbo.Animation where ID = @animationID
			select @productName = MaterialCode from dbo.Product where ID = @productID
			
			set @errorMsg = @errorMsg + 'Animation: ' + @animationName + '; Product: ' + @productName + 
				@newLineChar + '  Fixed: ' + cast(@fixedIterator as nvarchar(55)) + @newLineChar + 
				'  Capacity: ' + cast(@capacityIterator as nvarchar(55)) + @newLineChar
		
			set @iterator = @iterator - 1
		
		end
		
		set @errorMsg = @errorMsg + @newLineChar
	
	end
	
	
	
	
	
	
	-- **
	-- ** CHECK CUSTOMER GROUP FIXED ALLOCATIONS
	-- **
	
	declare @customerGroupAllocations table 
		(   allocationID uniqueidentifier
		  , animatPorduDetailID uniqueidentifier
		  , productID uniqueidentifier
		  , capacity int
		  , fixed int
		  , animationID uniqueidentifier 
		  , iterator int identity(1,1)
		   )
	
	insert into @customerGroupAllocations (allocationID, fixed, animatPorduDetailID, capacity)
		select c.ID, ManualFixedAllocation, IDAnimationProductDetail, dbo.calculate_TotalCapacityCustomerGroup(c.ID)
			from dbo.CustomerGroupAllocation  as c
				inner join dbo.AnimationProductDetail as a on (c.IDAnimationProductDetail = a.ID)
				inner join dbo.AnimationProduct as b on (a.IDAnimationProduct = b.ID)  
				inner join dbo.Animation as d on (b.IDAnimation = d.ID)
				where IDCustomerGroup = @customerGroupID 
					and ManualFixedAllocation is not null and ManualFixedAllocation > dbo.calculate_TotalCapacityCustomerGroup(c.ID)
					and d.Status not in (5, 6) 
					and b.IDCategory = @categoryOld
	
	declare @countOfGroupFixedErrors int
	select @countOfGroupFixedErrors = count(*) from @customerGroupAllocations
		where fixed is not null and fixed > capacity
		
	if @countOfGroupFixedErrors > 0
	begin
	
		set @errorMsg = @errorMsg + 'Some of customer group fixed allocations exceed their capacities' + @newLineChar	
	
		
		update @customerGroupAllocations set animationID = c.IDAnimation, productID = c.IDProduct
			from @customerGroupAllocations as a
			 inner join dbo.AnimationProductDetail as b on (a.animatPorduDetailID = b.ID)
			 inner join dbo.AnimationProduct as c on (b.IDAnimationProduct = c.ID)
			 
		
		select @iterator = max(iterator) from 	@customerGroupAllocations 
	
		while @iterator > 0
		begin
			
			select @animationID = animationID, @fixedIterator = fixed, @capacityIterator = capacity,
					@productID = productID from @customerGroupAllocations
				where iterator = @iterator
		
			select @animationName = Name from dbo.Animation where ID = @animationID
			select @productName = MaterialCode from dbo.Product where ID = @productID
			
			set @errorMsg = @errorMsg + 'Animation: ' + @animationName + '; Product: ' + @productName + 
				@newLineChar + '  Fixed: ' + cast(@fixedIterator as nvarchar(55)) + @newLineChar + 
				'  Capacity: ' + cast(@capacityIterator as nvarchar(55)) + @newLineChar
		
			set @iterator = @iterator - 1
		
		end
		
		set @errorMsg = @errorMsg + @newLineChar
	
	end
					
				
	
	
	
	-- **
	-- ** CHECK ALLOCATION QUANTITIES
	-- **
	
	
	declare @animationProductDetails table
	( ID uniqueidentifier
	 ,allocationQuantity int
	 ,capacity int
	 ,productID uniqueidentifier
	 ,animationID uniqueidentifier
	 ,iterator int identity(1,1)
	 )
	 
	 insert into @animationProductDetails (ID, allocationQuantity, capacity, productID, animationID)
		select distinct IDAnimationProductDetail, b.AllocationQuantity, dbo.calculate_TotalCapacity(c.ID),
			c.IDProduct,  c.IDAnimation
				from dbo.CustomerAllocation as a 
					inner join dbo.AnimationProductDetail as b on (a.ID = b.ID)
					inner join dbo.AnimationProduct as c on (b.IDAnimationProduct = c.ID)	
					inner join dbo.Animation as d on (c.IDAnimation = d.ID)				
				where b.AllocationQuantity is not null and  dbo.calculate_TotalCapacity(c.ID) < b.AllocationQuantity
					and d.Status not in (5, 6) 
					and c.IDCategory = @categoryOld
	
	declare @countOfQunatityErrors int
	select @countOfQunatityErrors = count(*) from @animationProductDetails

		
	if @countOfQunatityErrors > 0
	begin
	
		set @errorMsg = @errorMsg + 'Some of allocation qunatities exceed capacities' + @newLineChar
		
		 select @iterator = max(iterator) from 	@animationProductDetails 
	
		while @iterator > 0
		begin
			
			select @animationID = animationID, @fixedIterator = allocationQuantity, @capacityIterator = capacity,
					@productID = productID from @animationProductDetails
				where iterator = @iterator
		
			select @animationName = Name from dbo.Animation where ID = @animationID
			select @productName = MaterialCode from dbo.Product where ID = @productID
			
			set @errorMsg = @errorMsg + 'Animation: ' + @animationName + '; Product: ' + @productName + 
				@newLineChar + '  Quantity: ' + cast(@fixedIterator as nvarchar(55)) + @newLineChar + 
				'  Capacity: ' + cast(@capacityIterator as nvarchar(55)) + @newLineChar
		
			set @iterator = @iterator - 1
		
		end
		
		set @errorMsg = @errorMsg + @newLineChar
	
	end
	*/

	-- raise error
	if len(@errorMsg) > 0
	begin
		RAISERROR (@errorMsg, 16, 36)
		ROLLBACK TRAN
		RETURN		
	end

end	