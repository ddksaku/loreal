IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS
      WHERE TABLE_NAME = 'v_snapshot')
   DROP VIEW v_snapshot
GO
CREATE VIEW v_snapshot
AS 


select 
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
	dbo.calculate_TotalCapacity(b.ID) as TotalCapacity,
	dbo.calculate_TotalBDCQuantity(b.ID) as TotalBDCQuantity,
	dbo.calculate_TotalForecastQuantity(b.ID) as TotalForecastQuantity,
	dbo.calculate_TotalAllocation(b.ID) as TotalAllocation,
	dbo.calculate_TotalCIV(b.ID) as TotalCIV,
	k.InTransit as Product_InTransit,
	dbo.calculate_ProductConfirmed(k.ID) as Product_Confirmed,
	dbo.calculate_Reliquat(k.ID) as Product_Reliquat,
	dbo.calculate_MonthDelivery(k.ID, 0) as Product_MonthDeleveryCurrent,
	dbo.calculate_MonthDelivery(k.ID, 1) as Product_MonthDelevery1,
	dbo.calculate_MonthDelivery(k.ID, 2) as Product_MonthDelevery2,
	dbo.calculate_MonthDelivery(k.ID, 3) as Product_MonthDelevery3,
	dbo.calculate_MonthDelivery(k.ID, 4) as Product_MonthDelevery4,
	dbo.calculate_MonthDelivery(k.ID, 5) as Product_MonthDelevery5,
	dbo.calculate_MonthDelivery(k.ID, 6) as Product_MonthDelevery6,
	dbo.calculate_MonthDelivery(k.ID, 7) as Product_MonthDelevery7,
	dbo.calculate_MonthDelivery(k.ID, 8) as Product_MonthDelevery8,
	dbo.calculate_MonthDelivery(k.ID, 9) as Product_MonthDelevery9,
	dbo.calculate_MonthDelivery(k.ID, 10) as Product_MonthDelevery10,
	dbo.calculate_MonthDelivery(k.ID, 11) as Product_MonthDelevery11,
	dbo.calculate_ProductReceived(b.ID) as Product_ReceivedToDate,
	v.Name as SalesArea_SalesOrganizationName, 
	w.Name as SalesArea_DistributionChannelName,
	x.Name as SalesArea_DivisionName,
	s.Name as SalesArea_Name,
	s.Code as SalesArea_Code,
	c.ID as AnimationProductDetail_ID,
	c.RRP as AnimationProductDetail_RRP,
	dbo.calculate_ListPriceUK(c.ID, h.ID) as ListPriceUK,
	dbo.calculate_ListPriceROI(c.ID, h.ID) as  ListPriceROI,
	dbo.calculate_RRP_UK(c.ID, h.ID) AS RrpUK,
	dbo.calculate_RRP_ROI(c.ID, h.ID) AS RrpROI,
	k.CIV as Product_CostPrice,
	k.CIV * c.ForecastProcQuantity as AnimationProductDetail_CostValue,
	c.BDCQuantity as AnimationProductDetail_BDCQuantity,
	c.ForecastProcQuantity as AnimationProductDetail_ForecastProcQuantity,
	c.AllocationQuantity as AnimationProductDetail_AllocationQuantity,
	c.MarketingComments as AnimationProductDetail_MarketingComments,
	dbo.calculate_ProductDetailTotals(c.ID) as AnimationProductDetail_TotalCapacity,
	dbo.calculate_AllocationRemainder(c.ID) as AnimationProductDetail_AllocationRemainder,
	f.ID as CustomerGroup_ID,
	f.Name as CustomerGroup_Name,
	f.Code as CustomerGroup_Code,
	t.Name as CustomerGroup_SalesArea,
	t.ID as CustomerGroup_SalesAreaID,
	u.RetailUplift as CustomerGroup_RetailUplift,
	u.ManualFixedAllocation as CustomerGroup_ManualFixed,
	u.SystemFixedAllocation as CustomerGroup_SystemFixed,
	u.CalculatedAllocation as CustomerGroup_CalculatedAllocation,	
	dbo.calculate_TotalCapacityCustomerGroup(b.ID, f.ID) as CustomerGroup_TotalCapacity,
	dbo.countSale(NULL, f.ID, q.ID, s.ID, c.ID, p.ID) as CustomerGroup_SaleData,
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
	dbo.countSale(e.ID, NULL, q.ID, s.ID, c.ID, p.ID) as Customer_SaleData,
	DATEADD(year, -1, a.DateCalculated) as Customer_SaleDataMinimumDate,
	a.DateCalculated as Customer_SaleDataMaximumDate,
	ISNULL(ab.OnCounterDate,a.OnCounterDate	) AS OnCounterDate

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