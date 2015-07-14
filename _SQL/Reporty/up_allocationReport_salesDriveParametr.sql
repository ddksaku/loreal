if object_id('up_allocationReport_salesDriveParametr') > 0
	drop procedure up_allocationReport_salesDriveParametr
go

create procedure up_allocationReport_salesDriveParametr
	@DivisionID nvarchar(255) = null
as

if @DivisionID is null
begin

	SELECT ID AS SalesDriveId, NAME AS SalesDriveName FROM dbo.SalesDrive
		where Deleted = 0
			order by Name

end
else
begin
	
		SELECT ID AS SalesDriveId, NAME AS SalesDriveName FROM dbo.SalesDrive
			where Deleted = 0 AND IDDivision = @DivisionID
				order by Name


end