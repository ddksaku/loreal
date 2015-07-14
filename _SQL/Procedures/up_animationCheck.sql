if object_id('up_animationCheck') > 0
	drop procedure up_animationCheck
go

create procedure up_animationCheck
	 @animationID uniqueidentifier
    ,@countOfErrors int OUTPUT
    ,@errorMsg nvarchar(max) OUTPUT
as

SET CONCAT_NULL_YIELDS_NULL OFF


/*declare @animationID uniqueidentifier
    ,@countOfErrors int 
    ,@errorMsg nvarchar(max) 

set @animationID = '0039C11E-0502-4C85-9F9D-519E0E45B88C'*/

begin

	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	SET NOCOUNT ON

	declare @newLineChar char(2)
	set @newLineChar = CHAR(13) + CHAR(10)

	SET @errorMsg = ''
	SET @countOfErrors = 0

	declare @animationProducts table
	( animationProductID uniqueidentifier
     ,normalMultiple int
	 ,normalMultipleID uniqueidentifier
     ,warehouseMultipleID uniqueidentifier
     ,warehouseMultiple int
	 ,productID uniqueidentifier
	 ,productCode nvarchar(55)
	)

	insert into @animationProducts (animationProductID, normalMultiple, normalMultipleID, warehouseMultipleID, 
									warehouseMultiple, productID, productCode)
		select distinct a.ID, mNor.Value, a.IDMultipleNormal, a.IDMutlipleWarehouse, mWar.Value, a.IDProduct, p.MaterialCode
			from dbo.AnimationProduct as a 
				left join dbo.Multiple as mNor on (a.IDMultipleNormal = mNor.ID)
				left join dbo.Multiple as mWar on (a.IDMutlipleWarehouse = mWar.ID)
				inner join dbo.Product as p on (a.IDProduct = p.ID)
				where IDAnimation = @animationID

	
	-- ** CHECK 01
	-- ** -- warehouse multiple must be divisible by normal multiple
	declare @countOfBadRows int
	select @countOfBadRows = count(*) from @animationProducts
		where normalMultiple > 0 and warehouseMultiple > 0 and warehouseMultiple % normalMultiple > 0

	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'There are some warehouse multiple values which are not divisible by normal multiple' 

		declare @error01details nvarchar(max)
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
			'  Product: ' + productCode + ' ; Multiple Warehouse: ' + cast(warehouseMultiple as nvarchar(55))
			+' ; Multiple Normal: ' + cast(normalMultiple as nvarchar(55))
		from @animationProducts	where normalMultiple > 0 and warehouseMultiple > 0 and warehouseMultiple % normalMultiple > 0

		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end
	-- ***********************************************************************************************************************




	-- ** CHECK 02
	-- ** -- multiple exists for product

	-- normal multiple
	select @countOfBadRows = count(*) from @animationProducts
		where (normalMultipleID is not null and not exists (select * from dbo.Multiple where ID = normalMultipleID and IDProduct = productID))
				
	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'There are some normal multiples set, which don''t exist within such product' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
					'  Product: ' + productCode	+' ; Multiple Normal: ' + cast(normalMultiple as nvarchar(55))
				from @animationProducts	where (normalMultipleID is not null and 
					not exists (select * from dbo.Multiple where ID = normalMultipleID and IDProduct = productID))

		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end


	-- warehouse multiples
	select @countOfBadRows = count(*) from @animationProducts
		where (warehouseMultipleID is not null and not exists (select * from dbo.Multiple where ID = warehouseMultipleID and IDProduct = productID))

	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'There are some warehouse multiples set, which don''t exist within such product' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				'  Product: ' + productCode	+' ; Multiple Warehouse: ' + cast(warehouseMultiple as nvarchar(55))
			from @animationProducts	
				where (warehouseMultipleID is not null and
				 not exists (select * from dbo.Multiple where ID = warehouseMultipleID and IDProduct = productID))

		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end

	-- ***********************************************************************************************************************

	declare @animationProductDetails table
	( apdID uniqueidentifier
     ,animationProductID uniqueidentifier
     ,productName nvarchar(255)
	 ,forecast int
	 ,quantity int
	 ,capacity int
	 ,multiple int
	 ,sumOfFixed int
	 ,sumOfGroupFixed int
	)

	insert into @animationProductDetails (apdID, animationProductID, productName, 
						forecast, quantity, capacity, multiple, sumOfFixed, sumOfGroupFixed)
		select ID, IDAnimationProduct, 
				  (select MaterialCode from dbo.Product	where ID = 
					(select IDProduct from dbo.AnimationProduct where ID = IDAnimationProduct))
				, ForecastProcQuantity, AllocationQuantity
				, dbo.calculate_ProductDetailTotals(ID)
				, (select normalMultiple from @animationProducts where animationProductID = IDAnimationProduct)
				, (select sum(isnull(FixedAllocation, 0)) from dbo.CustomerAllocation where IDAnimationProductDetail = ID)
				, (select sum(isnull(ManualFixedAllocation, 0)) from dbo.CustomerGroupAllocation where IDAnimationProductDetail = ID)
			from dbo.AnimationProductDetail as a				
				where IDAnimationProduct in (select animationProductID from @animationProducts )


	-- ** CHECK 03
	-- **  -- check divisibility of allocation quantity by multiple

	select @countOfBadRows = count(*) from @animationProductDetails
		where quantity > 0 and multiple > 0 AND quantity % multiple > 0
	
	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'There are some allocation quantities not divisible by multiple' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				'  Quantity: ' + quantity+' ; Multiple: ' + cast(multiple as nvarchar(55))
			from @animationProductDetails
		where quantity > 0 and multiple > 0 AND quantity % multiple > 0

		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end
	-- ***********************************************************************************************************************

	-- ** CHECK 04
	-- **  -- check if quantity doesn't exceed capacity

	select @countOfBadRows = count(*) from @animationProductDetails
		where quantity > 0 and capacity < quantity

	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'There are some allocation quantities which exceed overall capacity' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				'  Quantity: ' + cast(quantity as nvarchar(55)) + ' ; Capacity: ' + cast(capacity as nvarchar(55))
			from @animationProductDetails
			where quantity > 0 and capacity < quantity

		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end
	-- ***********************************************************************************************************************
	
	-- ** CHECK 05
	-- **  -- check allocation qunatities against sum of customer fixed
	update @animationProductDetails set 
		sumOfFixed = 
		(select sum(isnull(FixedAllocation, 0)) from dbo.CustomerAllocation 
			where IDAnimationProductDetail in (select ID from dbo.AnimationProductDetail where IDAnimationProduct = animationProductID))
		,sumOfGroupFixed = 
		(select sum(isnull(ManualFixedAllocation, 0)) from dbo.CustomerGroupAllocation 
			where IDAnimationProductDetail in (select ID from dbo.AnimationProductDetail where IDAnimationProduct = animationProductID))
	
	select @countOfBadRows = count(*) from @animationProductDetails
		where quantity > 0 and sumOfFixed > quantity
		
	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'Sum of customer fixed allocations exceeds allocation qunatity' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				' Product: ' + productName + @newLineChar +  
				'  Quantity: ' + cast(quantity as nvarchar(55)) + ' ; Sum Of Fixed: ' + cast(sumOfFixed as nvarchar(55))
			from @animationProductDetails
			where quantity > 0 and sumOfFixed > quantity

		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end	
	
	-- ***********************************************************************************************************************
		
		
	-- ** CHECK 06
	-- ** -- check allocation quantityies againts sum of group fixed	
	
	select @countOfBadRows = count(*) from @animationProductDetails
		where quantity > 0 and sumOfGroupFixed > quantity	
		
	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'Sum of customer group fixed allocations exceeds allocation qunatity' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				' Product: ' + productName + @newLineChar +  
				'  Quantity: ' + cast(quantity as nvarchar(55)) + ' ; Sum Of Fixed: ' + cast(sumOfGroupFixed as nvarchar(55))
			from @animationProductDetails
			where quantity > 0 and sumOfGroupFixed > quantity

		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end		
	-- ***********************************************************************************************************************
	
	-- ** CHECK 07
	-- ** -- check individual fixed allocations
	declare @fixedAllocations table
	( animationProductDetailID uniqueidentifier
	 ,quantity int
	 ,customerAllocationID uniqueidentifier
	 ,fixedAllocation int
	 ,productName nvarchar(255)
	 ,multiple int
	 ,retailUplift float	 
	 ,capacity int
	 )
	 
	 insert into @fixedAllocations
		select b.ID, AllocationQuantity, a.ID, FixedAllocation, d.MaterialCode,
				e.Value, a.RetailUplift				
				 , dbo.calculate_ProductDetailTotals(b.ID) 		
			from dbo.CustomerAllocation	as a
					inner join dbo.AnimationProductDetail as b on (a.IDAnimationProductDetail = b.ID)
					inner join dbo.AnimationProduct as c on (b.IDANimationProduct = c.ID)
					inner join dbo.Product as d on (c.IDProduct = d.ID)
					left join dbo.Multiple as e on (c.IDMultipleNormal = e.ID)
				where b.ID in (select apdID from @animationProductDetails)
				
	select @countOfBadRows = count(*) from @fixedAllocations
		where fixedAllocation > 0 and fixedAllocation > quantity	
		
	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'Some of customer fixed allocation exceeds allocation quantity' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				' Product: ' + productName + @newLineChar +  
				'  Quantity: ' + cast(quantity as nvarchar(55)) + ' ; Fixed: ' + cast(fixedAllocation as nvarchar(55))
			from @fixedAllocations
			where fixedAllocation > 0 and fixedAllocation > quantity

		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end					
	-- ***********************************************************************************************************************
	
	
	-- ** CHECK 08
	-- ** -- check individual group fixed allocations	
	declare @fixedGroupAllocations table
	( animationProductDetailID uniqueidentifier
	 ,quantity int
	 ,customerAllocationID uniqueidentifier
	 ,groupID uniqueidentifier
	 ,fixedAllocation int
	 ,productName nvarchar(255)
	 ,multiple int
	 ,sumOfCustomerFixed int
	 ,sumOfCustomerCapacity int
	 ,retailUplift float
	 )
	 
	 insert into @fixedGroupAllocations
		select b.ID, AllocationQuantity, a.ID, a.IDCustomerGroup, ManualFixedAllocation, d.MaterialCode,
				e.Value
				, (SELECT SUM(isnull(FixedAllocation,0)) from dbo.CustomerAllocation where IDAnimationProductDetail = b.ID)
				, dbo.calculate_TotalCapacityCustomerGroup(a.ID)
				, a.RetailUplift
			from dbo.CustomerGroupAllocation as a
					inner join dbo.AnimationProductDetail as b on (a.IDAnimationProductDetail = b.ID)
					inner join dbo.AnimationProduct as c on (b.IDANimationProduct = c.ID)
					left join dbo.Multiple as e on (c.IDMultipleNormal = e.ID)
					inner join dbo.Product as d on (c.IDProduct = d.ID)
				where b.ID in (select apdID from @animationProductDetails)
				
	select @countOfBadRows = count(*) from @fixedGroupAllocations
		where fixedAllocation > 0 and fixedAllocation > quantity	
	
	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'Some of customer group fixed allocation exceeds allocation quantity' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				' Product: ' + productName + @newLineChar +  
				'  Quantity: ' + cast(quantity as nvarchar(55)) + ' ; Group fixed: ' + cast(fixedAllocation as nvarchar(55))
			from @fixedGroupAllocations
			where fixedAllocation > 0 and fixedAllocation > quantity

		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end					
	-- ***********************************************************************************************************************
	
	
	-- ** CHECK 09
	-- **  -- check divisibility of of fixed allocation
	
	select @countOfBadRows = count(*) from @fixedAllocations
		where fixedAllocation > 0 and multiple > 0 and fixedAllocation%multiple > 0
		
	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'Some fixed allocation is not divisible by multiple' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				' Product: ' + productName + @newLineChar +  
				'  Multiple: ' + cast(multiple as nvarchar(55)) + ' ; Fixed: ' + cast(fixedAllocation as nvarchar(55))
			from @fixedAllocations
			where fixedAllocation > 0 and multiple > 0 and fixedAllocation%multiple > 0

		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end				
	-- ***********************************************************************************************************************
	
	-- ** CHECK 10
	-- **  -- only one of retail and fixed could be filled in
	select @countOfBadRows = count(*) from @fixedAllocations
		where fixedAllocation > 0 and retailUplift > 0 
		
	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'Just one from Retail Uplift and Fixed Allocation could be filled in' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				' Product: ' + productName + @newLineChar +  
				'  Retail: ' + cast(retailUplift as nvarchar(55)) + ' ; Fixed: ' + cast(fixedAllocation as nvarchar(55))
			from @fixedAllocations
			where fixedAllocation > 0 and retailUplift > 0 

		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end				
	-- ***********************************************************************************************************************
	
	-- ** CHECK 11
	-- ** -- check group fixed against capacity
	select @countOfBadRows = count(*) from @fixedAllocations
		where fixedAllocation > capacity
		
	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'Some fixed allocation exceeds capacity' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				' Product: ' + productName + @newLineChar +  
				'  Capccity: ' + cast(capacity as nvarchar(55)) + ' ; Fixed: ' + cast(fixedAllocation as nvarchar(55))
			from @fixedAllocations
			where fixedAllocation > 0 and multiple > 0 and fixedAllocation%multiple > 0
		
		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end				
	-- ******
	
	-- ** CHECK 12
	-- **  -- check sum of fixed against customer group fixed
	select @countOfBadRows = count(*) from @fixedGroupAllocations
		where fixedAllocation < sumOfCustomerFixed
	
	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'Sum of customer''s fixed allocations exceeds group allocation' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				' Product: ' + productName + @newLineChar +  
				'  Sum of customer fixed: ' + cast(sumOfCustomerFixed as nvarchar(55)) + ' ; Fixed: ' + cast(fixedAllocation as nvarchar(55))
			from @fixedGroupAllocations
			where fixedAllocation < sumOfCustomerFixed

		--set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end				
	-- ***********************************************
	
	-- ** CHECK 13
	-- **  -- check divisibility of group fixed
	select @countOfBadRows = count(*) from @fixedGroupAllocations
		where multiple > 1 and fixedAllocation > 0 and fixedAllocation % multiple > 0
	
	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'There is customer group fixed allocation which is not divisible by multiple' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				' Product: ' + productName + @newLineChar +  
				'  Multiple: ' + cast(multiple as nvarchar(55)) + ' ; Fixed: ' + cast(fixedAllocation as nvarchar(55))
			from @fixedGroupAllocations
			where multiple > 1 and fixedAllocation > 0 and fixedAllocation % multiple > 0


		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end				
	-- *********************************************
	
	-- ** CHECK 14
	-- **  -- check fixed vs quantity
	select @countOfBadRows = count(*) from @fixedGroupAllocations
		where fixedAllocation > quantity
	
	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'Group fixed allocation exceeds allocation quantity' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				' Product: ' + productName + @newLineChar +  
				'  Quantity: ' + cast(quantity as nvarchar(55)) + ' ; Fixed: ' + cast(fixedAllocation as nvarchar(55))
			from @fixedGroupAllocations
			where fixedAllocation > quantity


		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end				
	-- *******************************************
	
	-- ** CHECK 15
	-- **  -- check fixed vs group capacity
	select @countOfBadRows = count(*) from @fixedGroupAllocations
		where fixedAllocation > 0 and fixedAllocation > sumOfCustomerCapacity
		
	
	if @countOfBadRows > 0
	begin
		set @errorMsg =  @errorMsg + ' Fixed allocation exceeds group capacity' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				' Product: ' + productName + @newLineChar +  
				'  Capacity: ' + cast(sumOfCustomerCapacity as nvarchar(55)) + ' ; Fixed: ' + cast(fixedAllocation as nvarchar(55))
			from @fixedGroupAllocations
			where quantity > 0 and quantity > sumOfCustomerCapacity


		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end				
	-- *********************************************
	
	
	
	-- ** CHECK 16
	-- **  -- fixed or retail only
	select @countOfBadRows = count(*) from @fixedGroupAllocations
		where fixedAllocation > 0 and retailUplift > 0
	
	if @countOfBadRows > 0
	begin
		set @errorMsg = @errorMsg + 'Just one from Retail Uplift and Manual Fixed Allocation could be filled in' 

		SET @error01details = ''
		select @error01details = coalesce(@error01details + @newLineChar , '') + 
				' Product: ' + productName + @newLineChar 
			from @fixedGroupAllocations
			where fixedAllocation > 0 and retailUplift > 0


		set @errorMsg = @errorMsg + @error01details
		set @countOfErrors = @countOfErrors + 1
		set @errorMsg = @errorMsg + @newLineChar + @newLineChar
		
	end				
	-- *********************************************
	
	

end
/*
IF @countOfErrors = 0
BEGIN
	print('==============================================')
	print('There were no errors found.')
	print('==============================================')
END
ELSE
BEGIN
	print('==============================================')
	print('Errors found: ' + cast(@countOfErrors as nvarchar(50)))
	print('==============================================')
	print @errorMsg
END

*/