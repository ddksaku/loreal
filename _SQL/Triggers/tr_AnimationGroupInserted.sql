if object_id('tr_AnimationGroupInserted') > 0
	drop trigger tr_AnimationGroupInserted
go

create trigger tr_AnimationGroupInserted
on dbo.AnimationCustomerGroup
after insert
as
begin

	declare @animationID uniqueidentifier
	select @animationID = IDAnimation from inserted

	declare @groupID uniqueidentifier
	select @groupID = IDCustomerGroup from inserted

	declare @animationProductDetails table
	(ID uniqueidentifier)
	
	-- first, insert Animation Product Details with the inserted Animation Customer Group
	declare @animationProducts table
	(ID uniqueidentifier)
	
	declare @salesAreaToInsert table
	(salesAreaID uniqueidentifier
     ,iterator int identity(1,1))

	insert into @salesAreaToInsert (salesAreaID)
		-- from Group
		select cg.IDSalesArea from dbo.CustomerGroup as cg where cg.ID = (select IDCustomerGroup from inserted)
		union
		-- from Customer
		select distinct IDSalesArea_AllocationSalesArea from dbo.Customer
			where IDCustomerGroup = @groupID and IDSalesArea_AllocationSalesArea is not null
				and Deleted = 0 and IncludeInSystem = 1
	
	declare @maxIterator int
	declare @salesAreaIteratorID uniqueidentifier
	select @maxIterator = max(iterator) from @salesAreaToInsert

	while @maxIterator > 0
	begin

		select @salesAreaIteratorID = salesAreaID from @salesAreaToInsert where iterator = @maxIterator

		insert into @animationProducts 
		select ap.ID from dbo.AnimationProduct as ap
		where ap.IDAnimation = @animationID
		AND (not exists (select apd.ID from dbo.AnimationProductDetail as apd 
						 where apd.IDAnimationProduct = ap.ID AND apd.IDSalesArea = @salesAreaIteratorID))

		insert into dbo.AnimationProductDetail (ID, IDAnimationProduct, IDSalesArea, ModifiedBy, ModifiedDate, BDCQuantity)
			select NEWID(), ap.ID, @salesAreaIteratorID, 	'New Animation Product - automatically created', getdate(), 0
			from @animationProducts as ap

		-- finally, insert allocations
		insert into @animationProductDetails
			select a.ID
				from dbo.AnimationProductDetail as a 
					inner join dbo.AnimationProduct as b on (a.IDAnimationProduct = b.ID)
					where b.IDAnimation = @animationID

		declare @string nvarchar(max)
		
		select @string = COALESCE(@string + ', ', '') + 
		   CAST(ID AS nvarchar(55))
				FROM @animationProductDetails
				
		exec up_recreateAllocations	@string

		delete from @animationProductDetails
		delete from @animationProducts


		set @maxIterator = @maxIterator - 1

	end
	


end	