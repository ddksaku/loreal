
ALTER PROCEDURE [dbo].[up_createSnapshot] (@animationID uniqueidentifier, @name nvarchar(50) = NULL, @comments nvarchar(1000) = NULL, @createdBy nvarchar(50) = NULL)
AS

declare @versionID uniqueidentifier
declare @animationStatus int
declare @animationName nvarchar(100)
set @versionID = NEWID()
select @animationStatus = [Status], @animationName = [Name] FROM dbo.Animation WHERE ID = @animationID
SET @animationName = @animationName + '_snapshot_' + convert(varchar,getdate(),20)

INSERT INTO dbo.[Version] (ID, IDAnimation, AnimationStatus, [Name], DateCreated, CreatedBy, Comments, ModifiedBy, ModifiedDate)
VALUES(@versionId, @animationID, @animationStatus, ISNULL(@name, @animationName), getdate(), ISNULL(@createdBy,'create snaphsot procedure'), @comments, @comments, getdate())

set nocount on


DECLARE @snapshot TABLE(
	[ID] [uniqueidentifier] NOT NULL,
	[IDVersion] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[ModifiedDate] [datetime] NULL,
	[Division_Name] [nvarchar](50) NULL,
	[Division_ID] [uniqueidentifier] NULL,
	[SalesDrive_Name] [nvarchar](100) NULL,
	[SalesDrive_Year] [int] NULL,
	[Animation_ID] [uniqueidentifier] NULL,
	[Animation_Name] [nvarchar](100) NULL,
	[Animation_Code] [nvarchar](20) NULL,
	[Animation_SAPDespatchCode] [nvarchar](20) NULL,
	[Animation_DefaultCustomerReference] [nvarchar](20) NULL,
	[Animation_Status] [tinyint] NULL,
	[Animation_RequestedDeliveryDate] [datetime] NULL,
	[Priority_Name] [nvarchar](50) NULL,
	[Priority_ID] [uniqueidentifier] NULL,
	[AnimationType_Name] [nvarchar](50) NULL,
	[AnimationType_ID] [uniqueidentifier] NULL,
	[Animation_OnCounterDate] [datetime] NULL,
	[Animation_PLVDeliveryDate] [datetime] NULL,
	[Animation_PLVComponentDate] [datetime] NULL,
	[Animation_StockDate] [datetime] NULL,
	[ItemGroup_Name] [nvarchar](50) NULL,
	[ItemGroup_ID] [uniqueidentifier] NULL,
	[ItemType_Name] [nvarchar](100) NULL,
	[ItemType_ID] [uniqueidentifier] NULL,
	[Signature_Name] [nvarchar](50) NULL,
	[Signature_ID] [uniqueidentifier] NULL,
	[BrandAxe_Name] [nvarchar](50) NULL,
	[BrandAxe_ID] [uniqueidentifier] NULL,
	[Category_Name] [nvarchar](50) NULL,
	[Category_ID] [uniqueidentifier] NULL,
	[AnimationProduct_OnCAS] [bit] NULL,
	[MultipleNormal] [int] NULL,
	[MultipleWarehouse] [int] NULL,
	[AnimationProduct_ConfirmedMADMonth] [datetime] NULL,
	[AnimationProduct_StockRisk] [bit] NULL,
	[AnimationProduct_DeliveryRisk] [bit] NULL,
	[AnimationProduct_LogisticsComments] [nvarchar](2000) NULL,
	[Product_ID] [uniqueidentifier] NULL,
	[Product_MaterialCode] [nvarchar](20) NULL,
	[Product_Description] [nvarchar](200) NULL,
	[AnimationProduct_BDCBookNumber] [nvarchar](20) NULL,
	[AnimationProduct_SortOrder] [int] NULL,
	[Product_InternationalCode] [nvarchar](20) NULL,
	[Product_EAN] [nvarchar](50) NULL,
	[Product_Status] [nvarchar](50) NULL,
	[Product_Manual] [bit] NULL,
	[ItemType_IncludeInOrders] [int] NULL,
	[Product_ProcurementType] [nvarchar](50) NULL,
	[Product_Stock] [int] NULL,
	[Product_StockLessPipe] [int] NULL,
	[AnimationProduct_ID] [uniqueidentifier] NULL,
	[TotalCapacity] [int] NULL,
	[TotalBDCQuantity] [int] NULL,
	[TotalForecastQuantity] [int] NULL,
	[TotalAllocation] [int] NULL,
	[TotalCIV] [money] NULL,
	[Product_InTransit] [int] NULL,
	[Product_Confirmed] [int] NULL,
	[Product_Reliquat] [int] NULL,
	[Product_MonthDeleveryCurrent] [int] NULL,
	[Product_MonthDelevery1] [int] NULL,
	[Product_MonthDelevery2] [int] NULL,
	[Product_MonthDelevery3] [int] NULL,
	[Product_MonthDelevery4] [int] NULL,
	[Product_MonthDelevery5] [int] NULL,
	[Product_MonthDelevery6] [int] NULL,
	[Product_MonthDelevery7] [int] NULL,
	[Product_MonthDelevery8] [int] NULL,
	[Product_MonthDelevery9] [int] NULL,
	[Product_MonthDelevery10] [int] NULL,
	[Product_MonthDelevery11] [int] NULL,
	[Product_ReceivedToDate] [int] NULL,
	[SalesArea_SalesOrganizationName] [nvarchar](100) NULL,
	[SalesArea_DistributionChannelName] [nvarchar](50) NULL,
	[SalesArea_DivisionName] [nvarchar](50) NULL,
	[SalesArea_Name] [nvarchar](100) NULL,
	[SalesArea_Code] [nvarchar](20) NULL,
	[AnimationProductDetail_ID] [uniqueidentifier] NULL,
	[AnimationProductDetail_RRP] [money] NULL,
	[ListPriceUK] [money] NULL,
	[ListPriceROI] [money] NULL,
	[Product_CostPrice] [money] NULL,
	[AnimationProductDetail_CostValue] [money] NULL,
	[AnimationProductDetail_BDCQuantity] [int] NULL,
	[AnimationProductDetail_ForecastProcQuantity] [int] NULL,
	[AnimationProductDetail_AllocationQuantity] [int] NULL,
	[AnimationProductDetail_MarketingComments] [nvarchar](2000) NULL,
	[AnimationProductDetail_TotalCapacity] [int] NULL,
	[AnimationProductDetail_AllocationRemainder] [int] NULL,
	[CustomerGroup_ID] [uniqueidentifier] NULL,
	[CustomerGroup_Name] [nvarchar](100) NULL,
	[CustomerGroup_Code] [nvarchar](10) NULL,
	[CustomerGroup_SalesArea] [nvarchar](100) NULL,
	[CustomerGroup_SalesAreaID] [uniqueidentifier] NULL,
	[CustomerGroup_RetailUplift] [float] NULL,
	[CustomerGroup_ManualFixed] [int] NULL,
	[CustomerGroup_SystemFixed] [int] NULL,
	[CustomerGroup_CalculatedAllocation] [int] NULL,
	[CustomerGroup_TotalCapacity] [int] NULL,
	[CustomerGroup_SaleData] [float] NULL,
	[CustomerGroup_SaleDataMinimumDate] [datetime] NULL,
	[CustomerGroup_SaleDataMaximumDate] [datetime] NULL,
	[Customer_SalesEmployeeName] [nvarchar](50) NULL,
	[Customer_SalesEmployeeID] [uniqueidentifier] NULL,
	[Customer_SalesAreaName] [nvarchar](100) NULL,
	[Customer_SalesAreaID] [uniqueidentifier] NULL,
	[Customer_AccountNumber] [nvarchar](20) NULL,
	[Customer_Name] [nvarchar](200) NULL,
	[Customer_FixedAllocation] [int] NULL,
	[Customer_RetailUplift] [float] NULL,
	[Customer_CalculatedAllocation] [int] NULL,
	[Customer_Capacity] [int] NULL,
	[Customer_ID] [uniqueidentifier] NULL,
	[Customer_SaleData] [float] NULL,
	[Customer_SaleDataMinimumDate] [datetime] NULL,
	[Customer_SaleDataMaximumDate] [datetime] NULL,
	[RrpUK] [money] NULL,
	[RrpROI] [money] NULL,
	[onCounterDate] [datetime] NULL,
	[SalesArea_Detail_ID] [uniqueidentifier] NULL
)


INSERT INTO @snapshot
           ([ID],
			[IDVersion],
			[ModifiedBy],
			[ModifiedDate],
           [Division_Name]
           ,[Division_ID]
           ,[SalesDrive_Name]
           ,[SalesDrive_Year]
           ,[Animation_ID]
           ,[Animation_Name]
           ,[Animation_Code]
           ,[Animation_SAPDespatchCode]
           ,[Animation_DefaultCustomerReference]
           ,[Animation_Status]
           ,[Animation_RequestedDeliveryDate]
           ,[Priority_Name]
           ,[Priority_ID]
           ,[AnimationType_Name]
           ,[AnimationType_ID]
           ,[Animation_OnCounterDate]
           ,[Animation_PLVDeliveryDate]
           ,[Animation_PLVComponentDate]
           ,[Animation_StockDate]
           ,[ItemGroup_Name]
           ,[ItemGroup_ID]
           ,[ItemType_Name]
           ,[ItemType_ID]
           ,[Signature_Name]
           ,[Signature_ID]
           ,[BrandAxe_Name]
           ,[BrandAxe_ID]
           ,[Category_Name]
           ,[Category_ID]
           ,[AnimationProduct_OnCAS]
           ,[MultipleNormal]
           ,[MultipleWarehouse]
           ,[AnimationProduct_ConfirmedMADMonth]
           ,[AnimationProduct_StockRisk]
           ,[AnimationProduct_DeliveryRisk]
           ,[AnimationProduct_LogisticsComments]
           ,[Product_ID]
           ,[Product_MaterialCode]
           ,[Product_Description]
           ,[AnimationProduct_BDCBookNumber]
           ,[AnimationProduct_SortOrder]
           ,[Product_InternationalCode]
           ,[Product_EAN]
           ,[Product_Status]
           ,[Product_Manual]
           ,[ItemType_IncludeInOrders]
           ,[Product_ProcurementType]
           ,[Product_Stock]
           ,[Product_StockLessPipe]
           ,[AnimationProduct_ID]
           ,[TotalCapacity]
           ,[TotalBDCQuantity]
           ,[TotalForecastQuantity]
           ,[TotalAllocation]
           ,[TotalCIV]
           ,[Product_InTransit]
           ,[Product_Confirmed]
           ,[Product_Reliquat]
           ,[Product_MonthDeleveryCurrent]
           ,[Product_MonthDelevery1]
           ,[Product_MonthDelevery2]
           ,[Product_MonthDelevery3]
           ,[Product_MonthDelevery4]
           ,[Product_MonthDelevery5]
           ,[Product_MonthDelevery6]
           ,[Product_MonthDelevery7]
           ,[Product_MonthDelevery8]
           ,[Product_MonthDelevery9]
           ,[Product_MonthDelevery10]
           ,[Product_MonthDelevery11]
           ,[Product_ReceivedToDate]
           ,[SalesArea_SalesOrganizationName]
           ,[SalesArea_DistributionChannelName]
           ,[SalesArea_DivisionName]
           ,[SalesArea_Name]
           ,[SalesArea_Code]
           ,[AnimationProductDetail_ID]
           ,[AnimationProductDetail_RRP]
           ,[ListPriceUK]
           ,[ListPriceROI]
           ,[RrpUK]
           ,[RrpROI]
           ,[Product_CostPrice]
           ,[AnimationProductDetail_CostValue]
           ,[AnimationProductDetail_BDCQuantity]
           ,[AnimationProductDetail_ForecastProcQuantity]
           ,[AnimationProductDetail_AllocationQuantity]
           ,[AnimationProductDetail_MarketingComments]
           ,[AnimationProductDetail_TotalCapacity]
           ,[AnimationProductDetail_AllocationRemainder]
           ,[CustomerGroup_ID]
           ,[CustomerGroup_Name]
           ,[CustomerGroup_Code]
           ,[CustomerGroup_SalesArea]
           ,[CustomerGroup_SalesAreaID]
           ,[CustomerGroup_RetailUplift]
           ,[CustomerGroup_ManualFixed]
           ,[CustomerGroup_SystemFixed]
           ,[CustomerGroup_CalculatedAllocation]
           ,[CustomerGroup_TotalCapacity]
           ,[CustomerGroup_SaleData]
           ,[CustomerGroup_SaleDataMinimumDate]
           ,[CustomerGroup_SaleDataMaximumDate]
           ,[Customer_SalesEmployeeName]
           ,[Customer_SalesEmployeeID]
           ,[Customer_SalesAreaName]
           ,[Customer_SalesAreaID]
           ,[Customer_AccountNumber]
           ,[Customer_Name]
           ,[Customer_FixedAllocation]
           ,[Customer_RetailUplift]
           ,[Customer_CalculatedAllocation]
           ,[Customer_Capacity]
           ,[Customer_ID]
           ,[Customer_SaleData]
           ,[Customer_SaleDataMinimumDate]
           ,[Customer_SaleDataMaximumDate]
           ,[onCounterDate]
           ,[SalesArea_Detail_ID])
select newid(), @versionID, @createdBy, getdate(),
	h.Name as Division_Name,
	h.ID as Division_ID,
	g.Name as SalesDrive_Name,
	g.Year as SalesDrive_Year,
	a.ID as Animation_ID,
	a.Name as Animation_Name,
	a.Code as Animation_Code,
	a.SAPDespatchCode as Animation_SAPDespatchCode,
	a.DefaultCustomerReference as Animation_DefaultCustomerReference,
	a.Status as Animation_Status,
	a.RequestedDeliveryDate as Animation_RequestedDeliveryDate,
	l.Name as Priority_Name,
	l.ID as Priority_ID,
	m.Name as AnimationType_Name,
	m.ID as AnimationType_ID,
	a.OnCounterDate as Animation_OnCounterDate,
	a.PLVDeliveryDate as Animation_PLVDeliveryDate,
	a.PLVComponentDate as Animation_PLVComponentDate,
	a.StockDate as Animation_StockDate,
	n.Name as ItemGroup_Name,
	n.ID as ItemGroup_ID,
	o.Name as ItemType_Name,
	o.ID as ItemType_ID,
	p.Name as Signature_Name,
	p.ID as Signature_ID,
	q.Name as BrandAxe_Name,
	q.ID as BrandAxe_ID,
	r.Name as Category_Name,
	r.ID as Category_ID,
	b.OnCAS as AnimationProduct_OnCAS,
	i.Value as MultipleNormal,
	j.Value as MultipleWarehouse,
	b.ConfirmedMADMonth as AnimationProduct_ConfirmedMADMonth,
	b.StockRisk as AnimationProduct_StockRisk,
	b.DeliveryRisk as AnimationProduct_DeliveryRisk,
	b.LogisticsComments as AnimationProduct_LogisticsComments,
	k.ID as Product_ID,
	k.MaterialCode as Product_MaterialCode,
	k.Description as Product_Description,
	b.BDCBookNumber as AnimationProduct_BDCBookNumber,
	b.SortOrder as AnimationProduct_SortOrder,
	k.InternationalCode as Product_InternationalCode,
	k.EAN as Product_EAN,
	k.Status as Product_Status,
	k.Manual as Product_Manual,
	case when o.IncludeInSAPOrders = 1 then 1 else 0 end as ItemType_IncludeInOrders,
	k.ProcurementType as Product_ProcurementType,
	k.Stock as Product_Stock,
	k.StockLessPipe as Product_StockLessPipe,
	b.ID as AnimationProduct_ID,
	NULL,--dbo.calculate_TotalCapacity(b.ID) as TotalCapacity,
	NULL,--dbo.calculate_TotalBDCQuantity(b.ID) as TotalBDCQuantity,
	NULL,--dbo.calculate_TotalForecastQuantity(b.ID) as TotalForecastQuantity,
	NULL,--dbo.calculate_TotalAllocation(b.ID) as TotalAllocation,
	NULL,--dbo.calculate_TotalCIV(b.ID) as TotalCIV,
	k.InTransit as Product_InTransit,
	NULL,--dbo.calculate_ProductConfirmed(k.ID) as Product_Confirmed,
	NULL,--dbo.calculate_Reliquat(k.ID) as Product_Reliquat,
	NULL,--dbo.calculate_MonthDelivery(k.ID, 0) as Product_MonthDeleveryCurrent,
	NULL,--dbo.calculate_MonthDelivery(k.ID, 1) as Product_MonthDelevery1,
	NULL,--dbo.calculate_MonthDelivery(k.ID, 2) as Product_MonthDelevery2,
	NULL,--dbo.calculate_MonthDelivery(k.ID, 3) as Product_MonthDelevery3,
	NULL,--dbo.calculate_MonthDelivery(k.ID, 4) as Product_MonthDelevery4,
	NULL,--dbo.calculate_MonthDelivery(k.ID, 5) as Product_MonthDelevery5,
	NULL,--dbo.calculate_MonthDelivery(k.ID, 6) as Product_MonthDelevery6,
	NULL,--dbo.calculate_MonthDelivery(k.ID, 7) as Product_MonthDelevery7,
	NULL,--dbo.calculate_MonthDelivery(k.ID, 8) as Product_MonthDelevery8,
	NULL,--dbo.calculate_MonthDelivery(k.ID, 9) as Product_MonthDelevery9,
	NULL,--dbo.calculate_MonthDelivery(k.ID, 10) as Product_MonthDelevery10,
	NULL,--dbo.calculate_MonthDelivery(k.ID, 11) as Product_MonthDelevery11,
	NULL,--dbo.calculate_ProductReceived(b.ID) as Product_ReceivedToDate,
	v.Name as SalesArea_SalesOrganizationName, 
	w.Name as SalesArea_DistributionChannelName,
	x.Name as SalesArea_DivisionName,
	s.Name as SalesArea_Name,
	s.Code as SalesArea_Code,
	c.ID as AnimationProductDetail_ID,
	c.RRP as AnimationProductDetail_RRP,
	NULL,--dbo.calculate_ListPriceUK(c.ID, h.ID) as ListPriceUK,
	NULL,--dbo.calculate_ListPriceROI(c.ID, h.ID) as  ListPriceROI,
	NULL,--dbo.calculate_RRP_UK(c.ID, h.ID) AS RrpUK,
	NULL,--dbo.calculate_RRP_ROI(c.ID, h.ID) AS RrpROI,
	k.CIV as Product_CostPrice,
	k.CIV * c.ForecastProcQuantity as AnimationProductDetail_CostValue,
	c.BDCQuantity as AnimationProductDetail_BDCQuantity,
	c.ForecastProcQuantity as AnimationProductDetail_ForecastProcQuantity,
	c.AllocationQuantity as AnimationProductDetail_AllocationQuantity,
	c.MarketingComments as AnimationProductDetail_MarketingComments,
	NULL,--dbo.calculate_ProductDetailTotals(c.ID) as AnimationProductDetail_TotalCapacity,
	NULL,--dbo.calculate_AllocationRemainder(c.ID) as AnimationProductDetail_AllocationRemainder,
	f.ID as CustomerGroup_ID,
	f.Name as CustomerGroup_Name,
	f.Code as CustomerGroup_Code,
	t.Name as CustomerGroup_SalesArea,
	t.ID as CustomerGroup_SalesAreaID,
	u.RetailUplift as CustomerGroup_RetailUplift,
	u.ManualFixedAllocation as CustomerGroup_ManualFixed,
	u.SystemFixedAllocation as CustomerGroup_SystemFixed,
	u.CalculatedAllocation as CustomerGroup_CalculatedAllocation,	
	NULL,--dbo.calculate_TotalCapacityCustomerGroup(u.ID) as CustomerGroup_TotalCapacity,
	NULL,--dbo.countSale(NULL, f.ID, q.ID, s.ID, c.ID, p.ID) as CustomerGroup_SaleData,
	DATEADD(year, -1, a.DateCalculated) as CustomerGroup_SaleDataMinimumDate,
	a.DateCalculated as CustomerGroup_SaleDataMaximumDate,
	y.Name as Customer_SalesEmployeeName,
	y.ID as Customer_SalesEmployeeID,
	z.Name as Customer_SalesAreaName,
	z.ID as Customer_SalesAreaID,
	e.AccountNumber as Customer_AccountNumber,
	e.Name as Customer_Name,
	d.FixedAllocation as Customer_FixedAllocation,
	d.RetailUplift as Customer_RetailUplift,
	d.CalculatedAllocation as Customer_CalculatedAllocation,
	aa.Capacity as Customer_Capacity,
	e.ID as Customer_ID,
	NULL,--dbo.countSale(e.ID, NULL, q.ID, s.ID, c.ID, p.ID) as Customer_SaleData,
	DATEADD(year, -1, a.DateCalculated) as Customer_SaleDataMinimumDate,
	a.DateCalculated as Customer_SaleDataMaximumDate,
	ISNULL(ab.OnCounterDate,a.OnCounterDate	) AS OnCounterDate,
	s.ID as SalesArea_Detail_ID

from dbo.Animation as a
inner join dbo.AnimationProduct as b on (a.ID = b.IDAnimation)
inner join dbo.AnimationProductDetail as c on (b.ID = c.IDAnimationProduct)
inner join dbo.CustomerAllocation as d on (c.ID = d.IDAnimationProductDetail)
inner join dbo.Customer as e on d.IDCustomer = e.ID
inner join dbo.CustomerGroup as f on (e.IDCustomerGroup = f.ID)
inner join dbo.SalesDrive as g on (a.IDSalesDrive = g.ID)
inner join dbo.Division as h on (g.IDDivision = h.ID)
left join dbo.Multiple as i on (b.IDMultipleNormal = i.ID)
left join dbo.Multiple as j on (b.IDMutlipleWarehouse = j.ID)
inner join dbo.Product as k on (b.IDProduct = k.ID)
inner join dbo.Priority as l on (a.IDPriority = l.ID)
inner join dbo.AnimationType as m on (a.IDAnimationType = m.ID)
inner join dbo.ItemGroup as n on (b.IDItemGroup = n.ID)
inner join dbo.ItemType as o on (b.IDItemType = o.ID)
inner join dbo.Signature as p on (b.IDSignature = p.ID)
left join dbo.BrandAxe as q on (b.IDBrandAxe = q.ID)
left join dbo.Category as r on (b.IDCategory = r.ID)
inner join dbo.SalesArea as s on (c.IDSalesArea = s.ID)
inner join dbo.SalesArea as t on (f.IDSalesArea = t.ID)
inner join dbo.CustomerGroupAllocation as u on (f.ID = u.IDCustomerGroup AND c.ID = u.IDAnimationProductDetail)
inner join dbo.SalesOrganization as v on (s.IDSalesOrganization = v.ID)
inner join dbo.DistributionChannel as w on (s.IDDistributionChannel = w.ID)
inner join dbo.Division as x on (s.IDDivision = x.ID)
inner join dbo.SalesEmployee as y on (e.IDSalesEmployee = y.ID)
left join dbo.SalesArea as z on (e.IDSalesArea_AllocationSalesArea = z.ID)
left join dbo.CustomerCapacity as aa on (e.ID = aa.IDCustomer AND m.ID = aa.IDAnimationType AND l.ID = aa.IDPriority AND o.ID = aa.IDItemType)
LEFT JOIN dbo.AnimationCustomerGroup AS ab ON (a.ID = ab.IDAnimation AND f.ID = ab.IDCustomerGroup)
WHERE e.IncludeInSystem = 1 AND f.IncludeInSystem = 1 AND e.Deleted = 0
and a.ID = @animationID--'0343923A-5E78-4674-BFEE-1E1A79FABD2F'
--and a.ID = @animationID





-- ** Calculate values ANIMATION PRODUCT
declare @animationProductCalculations table
(animationProductID uniqueidentifier
 ,cTotalCapacity int
 ,cTotalBDCQuantity int
 ,cTotalForecastQuantity int
 ,cTotalAllocation int
 ,cTotalCIV money
 ,cProduct_ReceivedToDate int
 )
 
 insert into @animationProductCalculations (animationProductID)
	select distinct [AnimationProduct_ID]
		from @snapshot

update @animationProductCalculations set cTotalCapacity = dbo.calculate_TotalCapacity(animationProductID)
update @animationProductCalculations set cTotalBDCQuantity = dbo.calculate_TotalBDCQuantity(animationProductID)
update @animationProductCalculations set cTotalForecastQuantity = dbo.calculate_TotalForecastQuantity(animationProductID)
update @animationProductCalculations set cTotalAllocation = dbo.calculate_TotalAllocation(animationProductID)
update @animationProductCalculations set cTotalCIV = dbo.calculate_TotalCIV(animationProductID)
update @animationProductCalculations set cProduct_ReceivedToDate = dbo.calculate_ProductReceived(animationProductID)



update @snapshot set TotalCapacity = b.cTotalCapacity, TotalBDCQuantity = b.cTotalBDCQuantity,
		TotalForecastQuantity = b.cTotalForecastQuantity, TotalAllocation = b.cTotalAllocation,
		TotalCIV = b.cTotalCIV, Product_ReceivedToDate = b.cProduct_ReceivedToDate
	from @snapshot a inner join @animationProductCalculations b on (a.[AnimationProduct_ID] = b.animationProductID)




-- ** Calculate values PRODUCT
declare @productCalculations table
(   ID uniqueidentifier,
    cProduct_Confirmed int,
	cProduct_Reliquat int,
	cProduct_MonthDeleveryCurrent int,
	cProduct_MonthDelevery1 int,
	cProduct_MonthDelevery2 int,
	cProduct_MonthDelevery3 int,
	cProduct_MonthDelevery4 int,
	cProduct_MonthDelevery5 int,
	cProduct_MonthDelevery6 int,
	cProduct_MonthDelevery7 int,
	cProduct_MonthDelevery8 int,
	cProduct_MonthDelevery9 int,
	cProduct_MonthDelevery10 int,
	cProduct_MonthDelevery11 int
)

insert into @productCalculations (ID)
	select distinct Product_ID
		from @snapshot


update @productCalculations set cProduct_Confirmed = dbo.calculate_ProductConfirmed(ID)
update @productCalculations set cProduct_Reliquat = dbo.calculate_Reliquat(ID)
update @productCalculations set cProduct_MonthDeleveryCurrent = dbo.calculate_MonthDelivery(ID, 0)
update @productCalculations set cProduct_MonthDelevery1 = dbo.calculate_MonthDelivery(ID, 1)
update @productCalculations set cProduct_MonthDelevery2  = dbo.calculate_MonthDelivery(ID, 2)
update @productCalculations set cProduct_MonthDelevery3 = dbo.calculate_MonthDelivery(ID, 3)
update @productCalculations set cProduct_MonthDelevery4  = dbo.calculate_MonthDelivery(ID, 4)
update @productCalculations set cProduct_MonthDelevery5  = dbo.calculate_MonthDelivery(ID, 5)
update @productCalculations set cProduct_MonthDelevery6  = dbo.calculate_MonthDelivery(ID, 6)
update @productCalculations set cProduct_MonthDelevery7  = dbo.calculate_MonthDelivery(ID, 7)
update @productCalculations set cProduct_MonthDelevery8  = dbo.calculate_MonthDelivery(ID, 8)
update @productCalculations set cProduct_MonthDelevery9  = dbo.calculate_MonthDelivery(ID, 9)
update @productCalculations set cProduct_MonthDelevery10  = dbo.calculate_MonthDelivery(ID, 10)
update @productCalculations set cProduct_MonthDelevery11  = dbo.calculate_MonthDelivery(ID, 11)




update @snapshot set Product_Confirmed = b.cProduct_Confirmed, Product_Reliquat = b.cProduct_Reliquat,
	Product_MonthDeleveryCurrent = b.cProduct_MonthDeleveryCurrent, Product_MonthDelevery1 = b.cProduct_MonthDelevery1,
	Product_MonthDelevery2 = b.cProduct_MonthDelevery2, Product_MonthDelevery3 = b.cProduct_MonthDelevery3,
	Product_MonthDelevery4 = b.cProduct_MonthDelevery4, Product_MonthDelevery5 = b.cProduct_MonthDelevery5,
	Product_MonthDelevery6 = b.cProduct_MonthDelevery6, Product_MonthDelevery7 = b.cProduct_MonthDelevery7,
	Product_MonthDelevery8 = b.cProduct_MonthDelevery8, Product_MonthDelevery9 = b.cProduct_MonthDelevery9,
	Product_MonthDelevery10 = b.cProduct_MonthDelevery10, Product_MonthDelevery11 = b.cProduct_MonthDelevery11
	from @snapshot a inner join @productCalculations b on (a.Product_ID = b.ID)



-- ** Calculate values ANIMATION PRODUCT DETAIL + DIVISION
declare @detailAndDivision table
( apdID uniqueidentifier
 ,divisionID uniqueidentifier
 ,cListPriceUK money
 ,cListPriceROI money
 ,cRrpUK money
 ,cRrpROI money
 )


insert into @detailAndDivision (apdID, divisionID)
	select distinct AnimationProductDetail_ID, Division_ID
		from @snapshot
		
		
update @detailAndDivision set cListPriceUK = dbo.calculate_ListPriceUK(apdID, divisionID)
update @detailAndDivision set cListPriceROI = dbo.calculate_ListPriceROI(apdID, divisionID)
update @detailAndDivision set cRrpUK = dbo.calculate_RRP_UK(apdID, divisionID)
update @detailAndDivision set cRrpROI = dbo.calculate_RRP_ROI(apdID, divisionID)

update @snapshot set ListPriceUK = b.cListPriceUK , ListPriceROI = b.cListPriceROI , 
		RrpUK = b.cRrpUK, RrpROI = b.cRrpROI
from @snapshot  a inner join @detailAndDivision b on (a.Division_ID = b.divisionID AND a.AnimationProductDetail_ID = b.apdID)



-- ** Calculate values ANIMATION PRODUCT DETAIL

declare @detail table
( ID uniqueidentifier
 ,cAnimationProductDetail_TotalCapacity int
 ,cAnimationProductDetail_AllocationRemainder int
 )
 
 insert into @detail(ID)	
	select distinct AnimationProductDetail_ID
		from @snapshot
		
 update @detail set cAnimationProductDetail_TotalCapacity = dbo.calculate_ProductDetailTotals(ID)
 update @detail set cAnimationProductDetail_AllocationRemainder = dbo.calculate_AllocationRemainder(ID)
 
 update @snapshot set AnimationProductDetail_TotalCapacity = b.cAnimationProductDetail_TotalCapacity, 
	AnimationProductDetail_AllocationRemainder = b.cAnimationProductDetail_AllocationRemainder
	from @snapshot a inner join @detail b on (a.AnimationProductDetail_ID = b.ID)
	
	
	
-- ** Calculate values - CUSTOMER GROUP
declare @group table
( 
	  ID uniqueidentifier
	 ,cCustomerGroup_TotalCapacity int
 )	
 
 insert into @group (ID)
	select distinct CustomerGroup_ID
		from @snapshot
		
update @group set cCustomerGroup_TotalCapacity = dbo.calculate_TotalCapacityCustomerGroup(ID)

update @snapshot set CustomerGroup_TotalCapacity = b.cCustomerGroup_TotalCapacity
	from @snapshot a inner join @group b on (a.CustomerGroup_ID = b.ID)
	


-- ** Calculate values CUSTOMER SALES DATA
declare @groupSales table
(
 groupID uniqueidentifier,
 brandAxeID uniqueidentifier,
 salesAreaID uniqueidentifier,
 detailID uniqueidentifier,
 signatureID uniqueidentifier,
 value money
 )
 
 insert into @groupSales(groupID, brandAxeID, salesAreaID, detailID, signatureID)
	select distinct CustomerGroup_ID, BrandAxe_ID, SalesArea_Detail_ID, AnimationProductDetail_ID, Signature_ID
		from @snapshot
		


update @groupSales set value = (select value from dbo.countSalesTable(NULL, groupID, brandAxeID, salesAreaID, detailID, signatureID))
--update @groupSales set value = dbo.countSale(NULL, groupID, brandAxeID, salesAreaID, detailID, signatureID)





-- have to use subquery because of possible null values in parametres
update @snapshot set CustomerGroup_SaleData = 
	(select value from @groupSales where groupID = CustomerGroup_ID and brandAxeID = brandAxeID
		and salesAreaID = SalesArea_Detail_ID and detailID = AnimationProductDetail_ID and 
			signatureID = Signature_ID)



-- ** Calculate values CUSTOMER SALES
declare @cusSales table
(
 customerID uniqueidentifier,
 brandAxeID uniqueidentifier,
 salesAreaID uniqueidentifier,
 detailID uniqueidentifier,
 signatureID uniqueidentifier,
 value money
 )
 
  insert into @cusSales(customerID, brandAxeID, salesAreaID, detailID, signatureID)
	select distinct Customer_ID, BrandAxe_ID, SalesArea_Detail_ID, AnimationProductDetail_ID, Signature_ID
		from @snapshot
		
	--update @cusSales set value = 0--dbo.countSale(customerID, NULL, brandAxeID, salesAreaID, detailID, signatureID)
	update @cusSales set value = (select value from dbo.countSalesTable(customerID, NULL, brandAxeID, salesAreaID, detailID, signatureID))

	-- have to use subquery because of possible null values in parametres
	update @snapshot set Customer_SaleData = 
		(select value from @cusSales where customerID = Customer_ID and brandAxeID = brandAxeID
			and salesAreaID = SalesArea_Detail_ID and detailID = AnimationProductDetail_ID and 
				signatureID = Signature_ID)	
				
	
	INSERT INTO [VersionSnapshot]
           (ID,
            [IDVersion]
           ,[Division_Name]
           ,[Division_ID]
           ,[SalesDrive_Name]
           ,[SalesDrive_Year]
           ,[Animation_ID]
           ,[Animation_Name]
           ,[Animation_Code]
           ,[Animation_SAPDespatchCode]
           ,[Animation_DefaultCustomerReference]
           ,[Animation_Status]
           ,[Animation_RequestedDeliveryDate]
           ,[Priority_Name]
           ,[Priority_ID]
           ,[AnimationType_Name]
           ,[AnimationType_ID]
           ,[Animation_OnCounterDate]
           ,[Animation_PLVDeliveryDate]
           ,[Animation_PLVComponentDate]
           ,[Animation_StockDate]
           ,[ItemGroup_Name]
           ,[ItemGroup_ID]
           ,[ItemType_Name]
           ,[ItemType_ID]
           ,[Signature_Name]
           ,[Signature_ID]
           ,[BrandAxe_Name]
           ,[BrandAxe_ID]
           ,[Category_Name]
           ,[Category_ID]
           ,[AnimationProduct_OnCAS]
           ,[MultipleNormal]
           ,[MultipleWarehouse]
           ,[AnimationProduct_ConfirmedMADMonth]
           ,[AnimationProduct_StockRisk]
           ,[AnimationProduct_DeliveryRisk]
           ,[AnimationProduct_LogisticsComments]
           ,[Product_ID]
           ,[Product_MaterialCode]
           ,[Product_Description]
           ,[AnimationProduct_BDCBookNumber]
           ,[AnimationProduct_SortOrder]
           ,[Product_InternationalCode]
           ,[Product_EAN]
           ,[Product_Status]
           ,[Product_Manual]
           ,[ItemType_IncludeInOrders]
           ,[Product_ProcurementType]
           ,[Product_Stock]
           ,[Product_StockLessPipe]
           ,[AnimationProduct_ID]
           ,[TotalCapacity]
           ,[TotalBDCQuantity]
           ,[TotalForecastQuantity]
           ,[TotalAllocation]
           ,[TotalCIV]
           ,[Product_InTransit]
           ,[Product_Confirmed]
           ,[Product_Reliquat]
           ,[Product_MonthDeleveryCurrent]
           ,[Product_MonthDelevery1]
           ,[Product_MonthDelevery2]
           ,[Product_MonthDelevery3]
           ,[Product_MonthDelevery4]
           ,[Product_MonthDelevery5]
           ,[Product_MonthDelevery6]
           ,[Product_MonthDelevery7]
           ,[Product_MonthDelevery8]
           ,[Product_MonthDelevery9]
           ,[Product_MonthDelevery10]
           ,[Product_MonthDelevery11]
           ,[Product_ReceivedToDate]
           ,[SalesArea_SalesOrganizationName]
           ,[SalesArea_DistributionChannelName]
           ,[SalesArea_DivisionName]
           ,[SalesArea_Name]
           ,[SalesArea_Code]
           ,[AnimationProductDetail_ID]
           ,[AnimationProductDetail_RRP]
           ,RrpUK
           ,RrpROI
           ,[ListPriceUK]
           ,[ListPriceROI]
           ,[Product_CostPrice]
           ,[AnimationProductDetail_CostValue]
           ,[AnimationProductDetail_BDCQuantity]
           ,[AnimationProductDetail_ForecastProcQuantity]
           ,[AnimationProductDetail_AllocationQuantity]
           ,[AnimationProductDetail_MarketingComments]
           ,[AnimationProductDetail_TotalCapacity]
           ,[AnimationProductDetail_AllocationRemainder]
           ,[CustomerGroup_ID]
           ,[CustomerGroup_Name]
           ,[CustomerGroup_Code]
           ,[CustomerGroup_SalesArea]
           ,[CustomerGroup_SalesAreaID]
           ,[CustomerGroup_RetailUplift]
           ,[CustomerGroup_ManualFixed]
           ,[CustomerGroup_SystemFixed]
           ,[CustomerGroup_CalculatedAllocation]
           ,[CustomerGroup_TotalCapacity]
           ,[CustomerGroup_SaleData]
           ,[CustomerGroup_SaleDataMinimumDate]
           ,[CustomerGroup_SaleDataMaximumDate]
           ,[Customer_SalesEmployeeName]
           ,[Customer_SalesEmployeeID]
           ,[Customer_SalesAreaName]
           ,[Customer_SalesAreaID]
           ,[Customer_AccountNumber]
           ,[Customer_Name]
           ,[Customer_FixedAllocation]
           ,[Customer_RetailUplift]
           ,[Customer_CalculatedAllocation]
           ,[Customer_Capacity]
           ,[Customer_ID]
           ,[Customer_SaleData]
           ,[Customer_SaleDataMinimumDate]
           ,[Customer_SaleDataMaximumDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,OnCounterDate)
		select [ID] ,
				[IDVersion] ,				
				[Division_Name],
				[Division_ID] ,
				[SalesDrive_Name],
				[SalesDrive_Year],
				[Animation_ID] ,
				[Animation_Name] ,
				[Animation_Code] ,
				[Animation_SAPDespatchCode] ,
				[Animation_DefaultCustomerReference] ,
				[Animation_Status] ,
				[Animation_RequestedDeliveryDate] ,
				[Priority_Name] ,
				[Priority_ID] ,
				[AnimationType_Name] ,
				[AnimationType_ID] ,
				[Animation_OnCounterDate] ,
				[Animation_PLVDeliveryDate] ,
				[Animation_PLVComponentDate] ,
				[Animation_StockDate] ,
				[ItemGroup_Name] ,
				[ItemGroup_ID] ,
				[ItemType_Name] ,
				[ItemType_ID] ,
				[Signature_Name] ,
				[Signature_ID] ,
				[BrandAxe_Name] ,
				[BrandAxe_ID] ,
				[Category_Name] ,
				[Category_ID] ,
				[AnimationProduct_OnCAS] ,
				[MultipleNormal] ,
				[MultipleWarehouse] ,
				[AnimationProduct_ConfirmedMADMonth],
				[AnimationProduct_StockRisk] ,
				[AnimationProduct_DeliveryRisk] ,
				[AnimationProduct_LogisticsComments] ,
				[Product_ID] ,
				[Product_MaterialCode] ,
				[Product_Description] ,
				[AnimationProduct_BDCBookNumber] ,
				[AnimationProduct_SortOrder],
				[Product_InternationalCode] ,
				[Product_EAN] ,
				[Product_Status] ,
				[Product_Manual] ,
				[ItemType_IncludeInOrders],
				[Product_ProcurementType] ,
				[Product_Stock] ,
				[Product_StockLessPipe] ,
				[AnimationProduct_ID] ,
				[TotalCapacity] ,
				[TotalBDCQuantity] ,
				[TotalForecastQuantity] ,
				[TotalAllocation] ,
				[TotalCIV],
				[Product_InTransit] ,
				[Product_Confirmed] ,
				[Product_Reliquat] ,
				[Product_MonthDeleveryCurrent] ,
				[Product_MonthDelevery1] ,
				[Product_MonthDelevery2] ,
				[Product_MonthDelevery3] ,
				[Product_MonthDelevery4] ,
				[Product_MonthDelevery5] ,
				[Product_MonthDelevery6] ,
				[Product_MonthDelevery7] ,
				[Product_MonthDelevery8] ,
				[Product_MonthDelevery9] ,
				[Product_MonthDelevery10] ,
				[Product_MonthDelevery11],
				[Product_ReceivedToDate],
				[SalesArea_SalesOrganizationName] ,
				[SalesArea_DistributionChannelName] ,
				[SalesArea_DivisionName] ,
				[SalesArea_Name] ,
				[SalesArea_Code] ,
				[AnimationProductDetail_ID] ,
				[AnimationProductDetail_RRP] ,
				[RrpUK],
				[RrpROI] ,
				[ListPriceUK] ,
				[ListPriceROI] ,
				[Product_CostPrice] ,
				[AnimationProductDetail_CostValue] ,
				[AnimationProductDetail_BDCQuantity],
				[AnimationProductDetail_ForecastProcQuantity] ,
				[AnimationProductDetail_AllocationQuantity] ,
				[AnimationProductDetail_MarketingComments] ,
				[AnimationProductDetail_TotalCapacity],
				[AnimationProductDetail_AllocationRemainder] ,
				[CustomerGroup_ID] ,
				[CustomerGroup_Name] ,
				[CustomerGroup_Code] ,
				[CustomerGroup_SalesArea] ,
				[CustomerGroup_SalesAreaID] ,
				[CustomerGroup_RetailUplift] ,
				[CustomerGroup_ManualFixed] ,
				[CustomerGroup_SystemFixed] ,
				[CustomerGroup_CalculatedAllocation] ,
				[CustomerGroup_TotalCapacity] ,
				[CustomerGroup_SaleData] ,
				[CustomerGroup_SaleDataMinimumDate] ,
				[CustomerGroup_SaleDataMaximumDate] ,
				[Customer_SalesEmployeeName],
				[Customer_SalesEmployeeID] ,
				[Customer_SalesAreaName] ,
				[Customer_SalesAreaID],
				[Customer_AccountNumber] ,
				[Customer_Name] ,
				[Customer_FixedAllocation],
				[Customer_RetailUplift] ,
				[Customer_CalculatedAllocation] ,
				[Customer_Capacity] ,
				[Customer_ID] ,
				[Customer_SaleData] ,
				[Customer_SaleDataMinimumDate] ,
				[Customer_SaleDataMaximumDate] ,				
				[ModifiedBy],
				[ModifiedDate],
				[onCounterDate] 
			from @snapshot			
	
	