if object_id('up_storeAllocationReport_salesDriveParameter') > 0
	drop procedure up_storeAllocationReport_salesDriveParameter
go

create procedure up_storeAllocationReport_salesDriveParameter
	@divisionID nvarchar(255) = null
as

if @divisionID is null
begin

	SELECT [NAME] AS SalesDriveName FROM dbo.SalesDrive
		where Deleted = 0
			order by Name

end
else
begin
	
		SELECT [NAME] AS SalesDriveName FROM dbo.SalesDrive
			where Deleted = 0 AND IDDivision = @DivisionID
				order by Name


end
