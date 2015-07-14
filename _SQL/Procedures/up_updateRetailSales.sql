if object_id('up_updateRetailSales') > 0
	drop procedure up_updateRetailSales
go

create procedure up_updateRetailSales
	@customerID uniqueidentifier = null
as

if @customerID is not null
begin
	
	update dbo.Customer set TotalSales = ISNULL(dbo.countSale(@customerID, NULL, NULL, NULL, NULL, null) , 0)
		where ID = @customerID

end
else
begin
	-- update * customers
	update dbo.Customer set TotalSales = ISNULL(dbo.countSale(ID, NULL, NULL, NULL, NULL, null) , 0)	
		where Deleted = 0 and IncludeInSystem = 1

end




