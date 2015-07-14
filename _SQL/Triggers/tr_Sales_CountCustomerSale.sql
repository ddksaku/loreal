IF OBJECT_ID ('tr_Sales_CountCustomerSale', 'TR') IS NOT NULL
   DROP TRIGGER tr_Sales_CountCustomerSale;
GO

CREATE TRIGGER tr_Sales_CountCustomerSale
ON Sale
AFTER UPDATE, INSERT 
AS 
BEGIN
	
	IF @@ROWCOUNT = 0 RETURN 	
		
	declare @customerID uniqueidentifier
	select @customerID = IDCustomer from inserted

	update dbo.Customer set TotalSales = dbo.countSale(@customerID, NULL, NULL, NULL, NULL, null) 
	

	RETURN
END