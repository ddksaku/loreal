if object_id('tr_AnimationGroupDelete') > 0
	drop trigger tr_AnimationGroupDelete
go

create trigger tr_AnimationGroupDelete
on dbo.AnimationCustomerGroup
after delete
as
begin

	declare @ID uniqueidentifier
	declare @groupID uniqueidentifier
	declare @animationID uniqueidentifier
	select  @ID = ID, @groupID = IDCustomerGroup, @animationID = IDAnimation from deleted
	
	
	declare @animationProductDetails table
	(ID uniqueidentifier)
	
	declare @customers table
	(ID uniqueidentifier)
	
	insert into @customers
		select ID from dbo.Customer
			where IDCustomerGroup = @groupID
			
	insert into @animationProductDetails
		select a.ID
			from  dbo.AnimationProductDetail as a
				inner join dbo.AnimationProduct as b on (a.IDAnimationProduct = b.ID)
					where b.IDAnimation = @animationID
					
	-- delete customerallocatiomn
	delete from dbo.CustomerAllocation	
		where IDCustomer in (select ID from @customers)
			and IDAnimationProductDetail in (select ID from @animationProductDetails)
			
	-- delete customer group allcoation
	delete from dbo.CustomerGroupAllocation		
		WHERE  IDAnimationProductDetail in (select ID from @animationProductDetails)
			and IDCustomerGroup	= 	@groupID	


	declare @salesAreasToRemove table
	(salesAreaID uniqueidentifier,
     countOfRecords int )

	insert into @salesAreasToRemove (salesAreaID)
		select cg.IDSalesArea from dbo.CustomerGroup as cg where cg.ID = @groupID
		union
		select distinct IDSalesArea_AllocationSalesArea from dbo.Customer
			where  IDSalesArea_AllocationSalesArea is not null and IDCustomerGroup = @groupID

	update @salesAreasToRemove set countOfRecords = 
		(select count(acg.ID) from dbo.AnimationCustomerGroup as acg 
							  INNER JOIN dbo.CustomerGroup as cg on cg.ID = acg.IDCustomerGroup
							  where acg.IDAnimation = @animationID AND cg.IDSalesArea = salesAreaID
		)

	delete from dbo.AnimationProductDetail 
		where IDSalesArea in (select salesAreaID from @salesAreasToRemove where countOfRecords = 0)
			AND ID in (select ID from @animationProductDetails)

    /*
	-- delete animation product details
	declare @salesAreaToRemove uniqueidentifier
	select @salesAreaToRemove = (select cg.IDSalesArea from dbo.CustomerGroup as cg where cg.ID = @groupID)

	declare @salesAreaCount int
	select @salesAreaCount = (select count(acg.ID) from dbo.AnimationCustomerGroup as acg 
							  INNER JOIN dbo.CustomerGroup as cg on cg.ID = acg.IDCustomerGroup
							  where acg.IDAnimation = @animationID AND cg.IDSalesArea = @salesAreaToRemove)
							  
	if @salesAreaCount = 0
	begin
		delete from dbo.AnimationProductDetail
		where IDSalesArea = @salesAreaToRemove
		AND ID in (select ID from @animationProductDetails)
	end	
*/

end	
