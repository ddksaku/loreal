if object_id('up_insertMissingAllCategory') > 0
	drop procedure up_insertMissingAllCategory
go

create procedure up_insertMissingAllCategory
as
BEGIN	

	-- create All category if doesn't exist
	insert into dbo.Category (Name, IDDivision, Deleted, ModifiedBy, ModifiedDate)	
		select 'All', ID, 0, 'Automatically created', getdate()
			 from dbo.Division as d
				where not exists (select * from dbo.Category where Name = 'All' and IDDivision = d.ID )
	
	

	-- compose insert table
	declare @insertTable table
	(
	 customerID uniqueidentifier,
	 divisionID uniqueidentifier,
	 categoryID uniqueidentifier
	 )
	 
	 insert into @insertTable (customerID, divisionID)
		select c.ID, d.ID
			from dbo.Customer as c
				inner join dbo.CustomerGroup as cg on (c.IDCustomerGroup = cg.ID)
				inner join dbo.SalesArea as sa on (cg.IDSalesArea = sa.ID)
				inner join dbo.Division as d on (sa.IDDivision = d.ID)
				where c.Deleted = 0 AND c.IncludeInSystem = 1
				
				
		
	update @insertTable	set categoryID = c.ID
		from @insertTable as a inner join dbo.Category as c on (a.divisionID = c.IDDivision and c.Name = 'All')
	
	delete from @insertTable
		where exists (select * from dbo.CustomerCategory where IDCustomer = customerID and IDCategory = categoryID)


	-- link it with customer
	--insert into dbo.CustomerCategory (IDCustomer, IDCategory, ModifiedBy, ModifiedDate)
		select customerID, categoryID, 'New customer - automatically created', getdate()
			from @insertTable

END