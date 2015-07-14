if object_id('up_export') > 0
	drop procedure up_export
go

create procedure up_export
	@animations nvarchar(max)
as


--declare @animations nvarchar(max)
--set @animations = '6CA31B95-A045-43C6-BAA6-0161A894EFCC, 69DB9592-88BD-45BB-AC44-0F83D8287B91'

select distinct
	case 
		when e.[Status] = 1 then 'Open'
		when e.[Status] = 2 then 'Locked'
		when e.[Status] = 3 then 'Draft'
		when e.[Status] = 4 then 'Published'
		when e.[Status] = 5 then 'Closed'
		when e.[Status] = 6 then 'Cleared'
		else ''
	end as AnimationStatus,	
	e.RequestedDeliveryDate as RequestedDeliveryDate, e.OnCounterDate as OnCounterDate, e.SAPDespatchCode as SAPDespatchCode,
	e.DefaultCustomerReference as DefaultCustomerReference, e.PLVDeliveryDate as PLVDeliveryDate,
	e.PLVComponentDate as PLVComponentDate, e.StockDate as StockDate,
	b.Name AS Customer, c.Name as CustomerGroup, e.Name as Animation, isnull(a.CalculatedAllocation, 0) as Allocation,
	h.Description as Product, sa.Name as SalesArea, sd.Name as SalesDrive, p.Name as Priority, 
	isnull(at.Name,'') as AnimationType, 
	e.Code as AnimationCode, h.MaterialCode, h.InternationalCode, h.EAN, dd.Name as DivisionName, c.Code as GroupCode,
	h.ProcurementType as ProcurementType, h.CIV as CIV, h.Stock as Stock, h.StockLessPipe as StockLessPipe, 
	h.InTransit as InTransit,b.AccountNumber as CustomerAccount, se.Name as SalesEmployee, ig.Name as ItemGroup, 
	ISNULL(ba.Name,'') as BrandAxe, ISNULL(si.Name,'') as [Signature],
	ISNULL(cat.Name,'') as Category, ISNULL(mulNor.Value,'') as MultipleNormal, ISNULL(mulWar.Value,'') as MultipleWarehouse,
	a.FixedAllocation as FixedAllocation, a.RetailUplift as RetailUplift,
	so.Name as SalesOrganization, dc.Name as DistributionChannel, sa.Code as SalesAreaCode,
	cga.ManualFixedAllocation as GroupFixed, cga.RetailUplift as GroupRetail, ISNULL(cga.CalculatedAllocation,0) as GroupAllocation
 from  CustomerAllocation a 
	inner join Customer b on (a.IDCustomer = b.ID and b.Deleted = 0)
	inner join SalesEmployee se on (se.ID = b.IDSalesEmployee)
	inner join CustomerGroup c on (b.IDCustomerGroup = c.ID)
	inner join dbo.AnimationCustomerGroup d on (c.ID = d.IDCustomerGroup)
	inner join dbo.Animation e on (d.IDAnimation = e.ID)
	left join dbo.AnimationType at on (at.ID = e.IDAnimationType and at.Deleted = 0)
	left join dbo.Priority p on (p.ID = e.IDPriority and p.Deleted = 0)
	left join dbo.SalesDrive as sd on (sd.ID = e.IDSalesDrive and sd.Deleted = 0)	
	inner join dbo.Division as dd on (dd.ID = sd.IDDivision and dd.Deleted  = 0)
	inner join dbo.AnimationProduct f on (f.IDAnimation = e.ID)
	inner join dbo.ItemGroup ig on (f.IDItemGroup = ig.ID)
	left join dbo.BrandAxe ba on (f.IDBrandAxe = ba.ID)
	left join dbo.[Signature] si on (f.IDSignature = si.ID and si.Deleted = 0)
	left join  dbo.Category cat on (f.IDCategory = cat.ID and cat.Deleted = 0)
	left join dbo.Multiple mulNor on (f.IDMultipleNormal = mulNor.ID and mulNor.Deleted = 0)
	left join dbo.Multiple mulWar on (f.IDMutlipleWarehouse = mulWar.ID and mulWar.Deleted = 0)
	inner join dbo.Product h on (f.IDProduct = h.ID)	
	inner join dbo.AnimationProductDetail g on (g.IDAnimationProduct = f.ID)
	inner join dbo.SalesArea as sa on (g.IDSalesArea = sa.ID and sa.Deleted = 0)
	inner join dbo.SalesOrganization as so on (so.ID = sa.IDSalesOrganization and so.Deleted = 0)
	inner join dbo.DistributionChannel as dc on (dc.ID = sa.IDDistributionChannel and dc.Deleted = 0)
	inner join dbo.CustomerGroupAllocation cga on (cga.IDCustomerGroup = c.ID AND cga.IDAnimationProductDetail = g.ID)

	where e.ID IN 
		(select ltrim(rtrim(Value)) from dbo.uf_split(@animations,','))
		
	
	