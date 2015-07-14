if object_id('up_checkCapacitiesAfterUpdate') > 0
	drop procedure up_checkCapacitiesAfterUpdate
go

-- =========================================
-- == Procedure to check capacities against
-- ==     1. customer fixed allocations
-- ==     2. group fixed allocations
-- ==     3. allocation quantities
-- =========================================


create procedure up_checkCapacitiesAfterUpdate
	@customerID uniqueidentifier
   ,@conditions nvarchar(max)	
   ,@errorMsg nvarchar(max) OUTPUT
as
begin

	declare @newLineChar char(2)
	set @newLineChar = CHAR(13) + CHAR(10)	

	declare @sqlQuery nvarchar(max)
	
	-- to avoid joining null string
	set @errorMsg = ''

	declare @customerGroupID uniqueidentifier
	select @customerGroupID = IDCustomerGroup from dbo.Customer where ID = @customerID


	-- **
	-- ** CHECK CUSTOMER FIXED ALLOCATIONS
	-- **
	create table #customerAllocations  
		(   allocationID uniqueidentifier
		  , animatPorduDetailID uniqueidentifier
		  , productID uniqueidentifier
		  , capacity int
		  , fixed int
		  , animationID uniqueidentifier 
		  , iterator int identity(1,1)
		   )	
	

	set @sqlQuery = N'insert into #customerAllocations (allocationID, fixed, animatPorduDetailID, capacity)
			select c.ID, FixedAllocation, IDAnimationProductDetail, dbo.calculate_TotalCapacityCustomer(c.ID)
				from dbo.CustomerAllocation  as c
					inner join dbo.AnimationProductDetail as a on (c.IDAnimationProductDetail = a.ID)
					inner join dbo.AnimationProduct as b on (a.IDAnimationProduct = b.ID)  
					inner join dbo.Animation as d on (b.IDAnimation = d.ID)
					where IDCustomer = @customerID 
						and FixedAllocation is not null and FixedAllocation > dbo.calculate_TotalCapacityCustomer(c.ID)'

	IF len(@conditions) > 0
		set @sqlQuery = @sqlQuery + ' AND ' + @conditions

	EXECUTE sp_executesql 
            @sqlQuery,
            N'@customerID uniqueidentifier',
			@customerID = @customerID;


	
	declare @countOfFixedErrors int
	select @countOfFixedErrors = count(*) from #customerAllocations
		where fixed is not null and fixed > capacity
		
	if @countOfFixedErrors > 0
	begin
	
		set @errorMsg = @errorMsg + 'Some of customer fixed allocations exceed their capacities' + @newLineChar
	
			
		update #customerAllocations set animationID = c.IDAnimation, productID = c.IDProduct
			from #customerAllocations as a
			 inner join dbo.AnimationProductDetail as b on (a.animatPorduDetailID = b.ID)
			 inner join dbo.AnimationProduct as c on (b.IDAnimationProduct = c.ID)
			 
		 declare @iterator int
		 declare @animationID uniqueidentifier
		 declare @productID uniqueidentifier
		 declare @animationName nvarchar(255)
		 declare @productName nvarchar(255)
		 declare @fixedIterator int
		 declare @capacityIterator int
		 select @iterator = max(iterator) from 	#customerAllocations 
	
		while @iterator > 0
		begin
			
			select @animationID = animationID, @fixedIterator = fixed, @capacityIterator = capacity,
					@productID = productID from #customerAllocations
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
	
	create table #customerGroupAllocations  
		(   allocationID uniqueidentifier
		  , animatPorduDetailID uniqueidentifier
		  , productID uniqueidentifier
		  , capacity int
		  , fixed int
		  , animationID uniqueidentifier 
		  , iterator int identity(1,1)
		   )

	set @sqlQuery = N'insert into #customerGroupAllocations (allocationID, fixed, animatPorduDetailID, capacity)
		select c.ID, ManualFixedAllocation, IDAnimationProductDetail, dbo.calculate_TotalCapacityCustomerGroup(c.ID)
			from dbo.CustomerGroupAllocation  as c
				inner join dbo.AnimationProductDetail as a on (c.IDAnimationProductDetail = a.ID)
				inner join dbo.AnimationProduct as b on (a.IDAnimationProduct = b.ID)  
				inner join dbo.Animation as d on (b.IDAnimation = d.ID)
				where IDCustomerGroup = @customerGroupID 
					and ManualFixedAllocation is not null and ManualFixedAllocation > dbo.calculate_TotalCapacityCustomerGroup(c.ID)'

	IF len(@conditions) > 0
		set @sqlQuery = @sqlQuery + ' AND ' + @conditions

	
	EXECUTE sp_executesql 
            @sqlQuery,
            N'@customerGroupID uniqueidentifier',
			@customerGroupID = @customerGroupID;

	
	declare @countOfGroupFixedErrors int
	select @countOfGroupFixedErrors = count(*) from #customerGroupAllocations
		where fixed is not null and fixed > capacity
		
	if @countOfGroupFixedErrors > 0
	begin
	
		set @errorMsg = @errorMsg + 'Some of customer group fixed allocations exceed their capacities' + @newLineChar	
	
		
		update #customerGroupAllocations set animationID = c.IDAnimation, productID = c.IDProduct
			from #customerGroupAllocations as a
			 inner join dbo.AnimationProductDetail as b on (a.animatPorduDetailID = b.ID)
			 inner join dbo.AnimationProduct as c on (b.IDAnimationProduct = c.ID)
			 
		
		select @iterator = max(iterator) from 	#customerGroupAllocations 
	
		while @iterator > 0
		begin
			
			select @animationID = animationID, @fixedIterator = fixed, @capacityIterator = capacity,
					@productID = productID from #customerGroupAllocations
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
	
	
	create table #animationProductDetails 
	( ID uniqueidentifier
	 ,allocationQuantity int
	 ,capacity int
	 ,productID uniqueidentifier
	 ,animationID uniqueidentifier
	 ,iterator int identity(1,1)
	 ) 
	 

	
	set @sqlQuery = N'insert into #animationProductDetails (ID, allocationQuantity, capacity, productID, animationID)
		select distinct IDAnimationProductDetail, a.AllocationQuantity, dbo.calculate_TotalCapacity(b.ID),
			b.IDProduct,  b.IDAnimation
				from dbo.CustomerAllocation as c 
					inner join dbo.AnimationProductDetail as a on (c.ID = a.ID)
					inner join dbo.AnimationProduct as b on (a.IDAnimationProduct = b.ID)	
					inner join dbo.Animation as d on (b.IDAnimation = d.ID)				
				where a.AllocationQuantity is not null and  dbo.calculate_TotalCapacity(b.ID) < a.AllocationQuantity'

	IF len(@conditions) > 0
		set @sqlQuery = @sqlQuery + ' AND ' + @conditions
	
	EXECUTE sp_executesql 
            @sqlQuery
	
	

	
	declare @countOfQunatityErrors int
	select @countOfQunatityErrors = count(*) from #animationProductDetails

		
	if @countOfQunatityErrors > 0
	begin
	
		set @errorMsg = @errorMsg + 'Some of allocation qunatities exceed capacities' + @newLineChar
		
		 select @iterator = max(iterator) from 	#animationProductDetails 
	
		while @iterator > 0
		begin
			
			select @animationID = animationID, @fixedIterator = allocationQuantity, @capacityIterator = capacity,
					@productID = productID from #animationProductDetails
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


	drop table #customerAllocations 
	drop table #animationProductDetails
	drop table #customerGroupAllocations
end