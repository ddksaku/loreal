if object_id('up_orderExport_salesAreas') > 0
	drop procedure up_orderExport_salesAreas
go

create procedure up_orderExport_salesAreas
	@animationId uniqueidentifier
as


declare @salesAreas table
(ID uniqueidentifier)

insert into @salesAreas
	select IDSalesArea from dbo.AnimationProductDetail where IDAnimationProduct  
		IN (SELECT ID FROM dbo.AnimationProduct WHERE IDAnimation = @animationId)


select Code from dbo.SalesOrganization where Deleted = 0 and ID 
	IN (SELECT IDSalesOrganization FROM dbo.SalesArea where ID 
		IN (SELECT ID FROM @salesAreas))


