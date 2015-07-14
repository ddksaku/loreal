if object_id('up_orderExport_fileName') > 0
	drop procedure up_orderExport_fileName
go

-- compose file name

create procedure up_orderExport_fileName
	@animationID uniqueidentifier,
	@salesOrgaznizationCode nvarchar(255)
AS

declare @output nvarchar(500)
declare @disChannelCode nvarchar(55)
declare @divisionCode nvarchar(55)
declare @salesDriveName nvarchar(255)
declare @animationCode nvarchar(55)

select @salesDriveName = [Name] FROM dbo.SalesDrive WHERE ID IN (SELECT IDSalesDrive FROM dbo.Animation WHERE ID = @animationID)
select @animationCode = Code FROM dbo.Animation WHERE ID = @animationID 


set @output = @salesOrgaznizationCode + '-' + @animationCode + ' ' + @salesDriveName

select @output