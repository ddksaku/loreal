IF OBJECT_ID ('tr_Customer_NewCustomer', 'TR') IS NOT NULL
   DROP TRIGGER tr_Customer_NewCustomer;
GO

CREATE TRIGGER tr_Customer_NewCustomer
ON Customer
AFTER INSERT 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	-- 235.	When a new store is added (either manual or SAP), blank capacity data is automatically 
	-- produced for all item types.
	declare @customerID uniqueidentifier
	select @customerID = ID FROM inserted

	declare @groupID uniqueidentifier
	select @groupID = IDCustomerGroup FROM inserted

	declare @salesAreaID uniqueidentifier
	select @salesAreaID = IDSalesArea from dbo.CustomerGroup where ID = @groupID

	declare @divisionID uniqueidentifier
	select @divisionID = IDDivision from dbo.SalesArea where ID = @salesAreaID

	
	-- insert new capacities from carthesian product
	-- filter by division ID
	insert into dbo.CustomerCapacity (IDCustomer, IDAnimationType, IDPriority, IDItemType, ModifiedBy, ModifiedDate)
		select @customerID, b.ID, c.ID, a.ID, 'New store - automatically created', getdate() from dbo.ItemType as a, dbo.AnimationType as b, dbo.Priority as c
			where 
				a.IDDivision = @divisionID AND a.Deleted = 0 
				AND b.IDDivision = @divisionID AND b.Deleted = 0
				AND c.IDDivision = @divisionID AND c.Deleted = 0
	
		
	-- link new customer with "All" category
	declare @categoryID uniqueidentifier
	
	select @categoryID = ID from dbo.Category where IDDivision = @divisionID and [Name] = 'All'

	-- if this doesn't exist, create it
	if @categoryID is null
	begin		
		set @categoryID = newid()

		insert into dbo.Category (ID, Name, IDDivision, Deleted, ModifiedBy, ModifiedDate)
			values (@categoryID, 'All', @divisionID, 0, 'New customer - automaticelly created', getdate())
	end

	-- link it with customer
	insert into dbo.CustomerCategory (ID, IDCustomer, IDCategory, ModifiedBy, ModifiedDate)
		values (newid(), @customerID, @categoryID, 'New customer - automatically created', getdate())


	
	RETURN
END