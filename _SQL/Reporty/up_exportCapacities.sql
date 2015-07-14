if object_id('up_exportCapacities') > 0
	drop procedure up_exportCapacities
go

create procedure up_exportCapacities
	@customers nvarchar(max) = null,
	@animationTypes nvarchar(max) = null,
    @priorities nvarchar(max) = null,
	@itemTypes nvarchar(max) = null,
	@divisionID nvarchar = null
as
begin


	select b.Name + ' (' + div.Name + ')' as Customer,
		c.Name as AnimationType, d.Name as Priority, e.Name as ItemType,
		a.Capacity as Capacity 
	from dbo.CustomerCapacity a			
		inner join dbo.Customer b on (a.IDCustomer = b.ID and b.IncludeInSystem = 1)
		inner join dbo.CustomerGroup cg on (b.IDCustomerGroup = cg.ID and cg.IncludeInSystem = 1)
		inner join dbo.SalesArea sa on (cg.IDSalesArea = sa.ID and sa.Deleted = 0)
		inner join dbo.Division as div on (sa.IDDivision = div.ID and div.Deleted = 0)
		inner join dbo.AnimationType c on (a.IDAnimationType = c.ID and c.Deleted = 0)
		inner join dbo.Priority d on (a.IDPriority = d.ID and d.Deleted = 0)
		inner join dbo.ItemType e on (a.IDItemType = e.ID and e.Deleted = 0)
		where 
			
			IDCustomer in (select ltrim(rtrim(Value)) from dbo.uf_split(@customers,',')) 
			and IDAnimationType in (select ltrim(rtrim(Value)) from dbo.uf_split(@animationTypes,','))
			and IDPriority in (select ltrim(rtrim(Value)) from dbo.uf_split(@priorities,','))
			and IDItemType in (select ltrim(rtrim(Value)) from dbo.uf_split(@itemTypes,','))
		

end