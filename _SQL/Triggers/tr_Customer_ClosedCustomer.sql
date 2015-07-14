-- need find out, what Closed Store actually means in DB

IF OBJECT_ID ('tr_Customer_ClosedCustomer', 'TR') IS NOT NULL
   DROP TRIGGER tr_Customer_ClosedCustomer;
GO

CREATE TRIGGER tr_Customer_ClosedCustomer
ON Customer
AFTER UPDATE 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	-- 358.	A weekly audit email is required to notify the division administrators of key data 
	-- changes / additions in SAP. The following data should be monitored and changes reported:
	-- c.	Closed stores.
	declare @newValue bit
	declare @oldValue bit

	select @newValue = IncludeInSystem from inserted
	select @oldValue = IncludeInSystem from deleted

	if @newValue <> @oldValue AND @newValue = 0
	begin
		--> Alert
		declare @divisionID uniqueidentifier
		declare @customerID uniqueidentifier
		declare @alertMessage nvarchar(max)
		DECLARE @modifiedBy NVARCHAR(255)
		SELECT @modifiedBy = ModifiedBy FROM inserted
		DECLARE @newLineChar char(2)
		SET @newLineChar = CHAR(13) + CHAR(10)

		select @customerID = ID from inserted

		select @divisionID = c.IDDivision, @alertMessage = dbo.uf_getSystemMessage('tr_Customer_ClosedCustomer', a.Name, a.AccountNumber, null, null, null, null, null, null) 
				from dbo.Customer as a
				inner join dbo.CustomerGroup as b on (a.IDCustomerGroup = b.ID)
				inner join dbo.SalesArea as c on (b.IDSalesArea = c.ID)

		insert into dbo.AuditAlert (AlertType, AlertDescription, DateCreated, IDDivision, Processed, ModifiedBy)
			values('Customer', @alertMessage, getdate(), @divisionID, 0, @modifiedBy)					


	end	

	
	RETURN
END