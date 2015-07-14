if object_id('tr_ItemType_New') > 0
	drop trigger tr_ItemType_New
go

CREATE TRIGGER dbo.tr_ItemType_New
ON dbo.ItemType
AFTER INSERT 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	-- create new blank capacities for customers
	declare @itemTypeID uniqueidentifier
	select @itemTypeID = ID from inserted

	declare @divisionID uniqueidentifier
	select @divisionID = IDDivision from inserted
	
	-- customers from division
	declare @t table
	(customerID uniqueidentifier)
	
	insert into @t
		select a.ID from dbo.Customer a 
			inner join dbo.CustomerGroup b on (a.IDCustomerGroup = b.ID)
			inner join dbo.SalesArea c on (b.IDSalesArea = c.ID)
			inner join dbo.Division d on (c.IDDivision = d.ID)
			where d.ID = @divisionID and a.Deleted = 0 and a.IncludeInSystem = 1

	
	-- insert new capacities from carthesian product
	-- filter by division ID
	insert into dbo.CustomerCapacity (IDCustomer, IDAnimationType, IDPriority, IDItemType, ModifiedBy, ModifiedDate)
		select b.ID, c.ID, a.ID, @itemTypeID, 'New item type - automatically created', getdate() 
			from dbo.Priority as a, dbo.Customer as b, dbo.AnimationType as c
			where 
				a.IDDivision = @divisionID AND a.Deleted = 0 
			AND b.Deleted = 0 AND b.IncludeInSystem = 1 
			AND b.ID IN (SELECT customerID FROM @t)
			AND c.IDDivision = @divisionID AND c.Deleted = 0
	
	RETURN
END