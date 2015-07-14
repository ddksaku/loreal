if object_id('up_insertMissingCapaties') > 0
	drop procedure up_insertMissingCapaties
go
USE [LorealOptimiseClean]
GO
/****** Object:  StoredProcedure [dbo].[up_insertMissingCapaties]    Script Date: 06/30/2010 16:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Stored Procedure

CREATE procedure [dbo].[up_insertMissingCapaties]
as
begin	

	INSERT INTO dbo.CustomerCapacity(ID, IDCustomer, IDAnimationType, IDPriority, IDItemType, Capacity, ModifiedDate, ModifiedBy)
	SELECT NEWID(), IDCustomer, IDAnimationType, IDPriority, IDItemType, 0, GETDATE(), 'automatically created'
	 FROM
		(SELECT  d.iD AS IDCustomer, b.ID AS IDAnimationType, c.ID AS IDPriority, a.ID AS IDItemType
		FROM dbo.ItemType as a, dbo.AnimationType as b, dbo.Priority as c, dbo.Customer AS d, dbo.CustomerGroup AS cg, dbo.SalesArea AS sa
		WHERE 
			d.IDCustomerGroup = cg.ID and cg.IDSalesArea = sa.ID and
			a.IDDivision = b.IDDivision and a.IDDivision = c.IDDivision and sa.IDDivision = a.IDDivision and
			a.Deleted = 0 and b.Deleted = 0 and c.Deleted = 0 and d.Deleted = 0
	EXCEPT 
		SELECT IDCustomer, IDAnimationType, IDPriority, IDItemType
		FROM dbo.CustomerCapacity) as t	


end
GO