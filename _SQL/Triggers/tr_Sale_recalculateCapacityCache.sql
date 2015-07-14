if object_id('tr_Sale_recalculateCapacityCache') > 0
	drop trigger tr_Sale_recalculateCapacityCache
go

create trigger tr_Sale_recalculateCapacityCache
on dbo.Sale
after insert, update
as
begin

	declare @customerIDs table
	(ID uniqueidentifier)	
	
	insert into @customerIDs
		select IDCustomer from inserted
		
	-- find customer allocation IDs			
	declare @allocationIDs table
	(ID uniqueidentifier)
	
	insert into @allocationIDs		
		select ID from dbo.CustomerAllocation 
			where IDCustomer in (select ID from @customerIDs)
				
	 update dbo.CapacityCache set capacity = dbo.calculate_TotalCapacityCustomer(IDCustomerAllocation)	
		where IDCustomerAllocation in (select ID from @allocationIDs)	


end