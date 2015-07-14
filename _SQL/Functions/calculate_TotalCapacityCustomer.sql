if object_id('calculate_TotalCapacityCustomer') > 0
	drop function [calculate_TotalCapacityCustomer]
go

CREATE FUNCTION [dbo].[calculate_TotalCapacityCustomer] (@customerAllocationID uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	declare @output int
	select @output = capacity from dbo.CapacityCache where IDCustomerAllocation = @customerAllocationID
	
	if (@output is  null)	
	begin

		DECLARE @SalesAreaID UNIQUEIDENTIFIER
		DECLARE @ItemTypeID UNIQUEIDENTIFIER
		DECLARE @AnimationTypeID UNIQUEIDENTIFIER
		DECLARE @PriorityID UNIQUEIDENTIFIER
		DECLARE @AnimationID UNIQUEIDENTIFIER
		DECLARE @CategoryID UNIQUEIDENTIFIER
		DECLARE @BrandAxeID UNIQUEIDENTIFIER
		DECLARE @SignatureID UNIQUEIDENTIFIER
		DECLARE @customerId uniqueidentifier
		DECLARE @animationProductDetailID uniqueidentifier
		
		SELECT @customerId = IDCustomer, @animationProductDetailID = IDAnimationProductDetail 
			from dbo.CustomerAllocation
				WHERE ID = @customerAllocationID
		
		
		SELECT @SalesAreaID = apd.IDSalesArea,
				@ItemTypeID = ap.IDItemType,
				@AnimationTypeID = a.IDAnimationType,
				@PriorityID = a.IDPriority,
				@AnimationID = a.ID,
				@CategoryID = ap.IDCategory,
				@ItemTypeID = ap.IDItemType,
				@BrandAxeID = ap.IDBrandAxe,
				@SignatureID = ap.IDSignature			
		FROM dbo.AnimationProductDetail apd
		LEFT JOIN  dbo.AnimationProduct ap ON ap.ID = apd.IDAnimationProduct
		LEFT JOIN dbo.Animation a ON a.ID = ap.IDAnimation
		LEFT JOIN dbo.[Signature] s ON s.ID = ap.IDSignature 
		WHERE apd.ID = @animationProductDetailID
		
		
		SELECT @output = ISNULL(SUM(cc.Capacity),0)
		FROM dbo.CustomerCapacity cc
		INNER JOIN dbo.Customer c ON c.ID = cc.IDCustomer
		INNER JOIN dbo.CustomerGroup cg ON cg.ID = c.IDCustomerGroup
		--c.	Store category matches that entered against the Animation Product.	
		WHERE 
		c.ID = @customerID AND
		c.IncludeInSystem = 1 AND cg.IncludeInSystem = 1 AND c.Deleted = 0
		AND	EXISTS(SELECT ID FROM dbo.CustomerCategory WHERE IDCustomer = c.ID AND IDCategory = @CategoryID)
		--e.	Capacity is for the Promotion Type and Priority entered against the Animation.
		AND IDPriority = @PriorityID AND IDAnimationType = @AnimationTypeID
		--f.	Capacity is for the Item Type entered against the Animation Product.
		AND IDItemType = @ItemTypeID 
		--b.	Store is valid for the allocation country
		AND ((c.IDSalesArea_AllocationSalesArea = @SalesAreaID) OR (c.IDSalesArea_AllocationSalesArea IS NULL))
		--g.	Customer groups are included in the animation
		--AND (EXISTS (SELECT ID FROM AnimationCustomerGroup WHERE IDCustomerGroup = cg.ID))
		--d.	Store sells the ‘allocated by’ Signature (& Brand/Axe if entered) i.e. sales exist for the signature/brand/axe at any point over the past 12 months.
		AND (EXISTS (SELECT s.ID FROM dbo.Sale s 
					 LEFT JOIN dbo.BrandAxe ba ON ba.ID = s.IDBrandAxe 
						WHERE  s.IDCustomer = c.ID
						and not exists (select ID from dbo.CustomerBrandExclusion where IDCustomer = c.ID and IDBrandAxe = s.IDBrandAxe and Excluded = 1)
						AND ((@BrandAxeID IS NOT NULL AND IDBrandAxe = @BrandAxeID) OR (@BrandAxeID IS NULL AND ba.IDSignature = @SignatureID))
						AND s.[Date] > DATEADD(year,-1,GETDATE())
			))
	end	
		
	
	return isnull(@output, 0)

END
