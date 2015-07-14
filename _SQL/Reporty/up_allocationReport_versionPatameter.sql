if object_id('up_allocationReport_versionPatameter') > 0
	drop procedure up_allocationReport_versionPatameter
go

create procedure up_allocationReport_versionPatameter
	@salesDrive nvarchar(255)
as

SELECT a.Name AS VersionName, a.ID AS VersionID
FROM dbo.Version as a 
inner join dbo.Animation as b on (a.IDAnimation = b.ID)
inner join dbo.SalesDrive as c on (b.IDSalesDrive = c.ID)
where c.Name = @salesDrive
order by DateCreated desc




