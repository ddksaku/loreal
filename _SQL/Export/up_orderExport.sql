if object_id('up_orderExport') > 0
	drop procedure up_orderExport
go

create procedure up_orderExport
	@animationID uniqueidentifier,
	@salesOrganizationCode nvarchar(255)
as

--declare @salesOrganizationCode nvarchar(255)
--set @salesOrganizationCode = '0350'

--declare @animationID uniqueidentifier
--set @animationID = '2BFCC079-7B92-4DDB-8630-35E1034EB1DA'


declare @orderExportMessageType nvarchar(max)
declare @orderExportPartnerType nvarchar(max)
select @orderExportMessageType = SettingValue from dbo.ApplicationSettings
	where SettingKey = 'OrderExportMessageType'
select @orderExportPartnerType = SettingValue from dbo.ApplicationSettings
	where SettingKey = 'OrderExportPartnerType'

if @orderExportMessageType is null
	set @orderExportMessageType = 'ORDER'
if @orderExportPartnerType is null
	set @orderExportPartnerType = 'OPT'	

select so.Code as SalesOrganization, dch.Code as DistributionChannel, d.Code as Division,
	ot.Code as OrderType, c.AccountNumber as CustomerAccount, a.DefaultCustomerReference as DefaultCustomerReference,
	convert(nvarchar(55), a.RequestedDeliveryDate, 104) as RequestedDeliveryDate, a.SAPDespatchCode as SAPDespatchCode,
	p.MaterialCode as MaterialCode, ISNULL(ca.CalculatedAllocation, 0) as AllocationQuantity, sa.Code as SalesArea,
	cgit.ID as Warehouse, cg.ID as CustomerGroup, it.ID as ItemType, 0 as IsWarehouse, apd.ID as AnimationProductDetail
	into #t
	from dbo.CustomerAllocation as ca
	inner join dbo.AnimationProductDetail as apd on (apd.ID = ca.IDAnimationProductDetail)
	inner join dbo.AnimationProduct as ap on (apd.IDAnimationProduct = ap.ID)
	inner join dbo.ItemType as it on (ap.IDItemType = it.ID)
	inner join dbo.Animation as a on (ap.IDAnimation = a.ID)
	inner join dbo.OrderType as ot on (ot.ID = a.IDOrderType_Order)
	inner join dbo.Product as p on (p.ID = ap.IDProduct)
	inner join dbo.AnimationType as at on (at.ID = a.IDAnimationType)
	inner join dbo.Customer as c on (c.ID = ca.IDCustomer)
	inner join dbo.CustomerGroup as cg on (c.IDCustomerGroup = cg.ID)
	left join dbo.CustomerGroupItemType as cgit on (cgit.IDCustomer = c.ID AND cgit.IDItemType = it.ID)
	inner join dbo.SalesArea as sa on (sa.ID = c.IDSalesArea_AllocationSalesArea)
	inner join dbo.Division as d on (d.ID = sa.IDDivision)
	inner join dbo.SalesOrganization as so on (so.ID = sa.IDSalesOrganization)
	inner join dbo.DistributionChannel as dch on (dch.ID = a.IDDistributionChannel_Order)
	where so.Code = @salesOrganizationCode AND 
			a.ID = @animationID  
			AND it.IncludeInSAPOrders = 1
			--AND cg.IncludeInSAPOrders = 1 TODO uncomment
			AND cgit.IncludeInSAPOrders IS NULL OR cgit.IncludeInSAPOrders<> 0


/*
	0 ... within group there is no warehouse store  /default value/
	1 ... customer is warehouse store
	2 ... customer is not warehouse store, but there is warehouse account within the group
*/

-- 1 ... customer is warehouse store
update #t set IsWarehouse = 1
where Warehouse is not null

--
select CustomerGroup as cg, ItemType as it, IsWarehouse as iw 
into #temp3
from #t

-- 2 ... customer is not warehouse store, but there is warehouse account within the group
update #t set IsWarehouse = 2
where exists (select * from #temp3 where iw = 1 and cg = CustomerGroup and it = ItemType)


-- update calculated allocation for warehouse stores
update #t set AllocationQuantity = 
	(SELECT CalculatedAllocation from dbo.CustomerGroupAllocation 
		where IDCustomerGroup = CustomerGroup
			and IDAnimationProductDetail = AnimationProductDetail)
	where IsWarehouse = 1	
	
-- zero quantity for stores from group with warehouse account	
update #t set AllocationQuantity = 0 where IsWarehouse = 2

select CustomerAccount, SAPDespatchCode, SalesArea 
into #temp2
from #t 
group by CustomerAccount, SAPDespatchCode, SalesArea

alter table #temp2
add iterator INT IDENTITY(1,1)

declare @loop int
select @loop = max(iterator) from #temp2

declare @loopAccount nvarchar(255)
declare @loopSAP nvarchar(255)
declare @loopSalesArea nvarchar(255)

declare @t table
(
	column01 nvarchar(255),
	column02 nvarchar(255),
	column03 nvarchar(255),
	column04 nvarchar(255),
	column05 nvarchar(255),
	column06 nvarchar(255),
	column07 nvarchar(255),
	column08 nvarchar(255),
	column09 nvarchar(255),
	column10 nvarchar(255),
	column11 nvarchar(255),
	column12 nvarchar(255),
	column13 nvarchar(255),
	column14 nvarchar(255),
	column15 nvarchar(255),
	column16 nvarchar(255),
	iterator int identity(1,1)
)


while @loop > 0
begin
	select	@loopAccount = CustomerAccount, @loopSAP = SAPDespatchCode, @loopSalesArea = SalesArea
		from #temp2
			where iterator = @loop

	-- insert empty line at the start
	--  insert into @t (column01)
	--	 select null

	-- ** NO WAREHOUSE ACCOUNT

	-- insert Header row
	insert into @t (column01, column02, column03, column04, column05, column06, column07,
					column08, column09, column10, column11, column12, column13, column14,
					column15, column16)
		select distinct 'H', SalesOrganization, DistributionChannel, Division, null, OrderType, CustomerAccount,
				DefaultCustomerReference, cast(RequestedDeliveryDate as nvarchar(255)), 'AUTO', SAPDespatchCode, null,
				null, null, @orderExportPartnerType, @orderExportMessageType
			from #t
				where CustomerAccount = @loopAccount AND  SAPDespatchCode = @loopSAP AND SalesArea = @loopSalesArea

	-- insert Detail rows
	insert into @t (column01, column02, column03, column04, column05, column06, column07,
					column08, column09, column10, column11, column12, column13, column14)
		select  'D', null, null, null, null, null, null, null, null, null, null, null,
				MaterialCode, sum(AllocationQuantity)
			from #t				
				where CustomerAccount = @loopAccount AND  SAPDespatchCode = @loopSAP AND SalesArea = @loopSalesArea
				group by MaterialCode



	set @loop = @loop - 1
end

drop table #t
drop table #temp2
drop table #temp3

select * from @t



	