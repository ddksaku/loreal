/****** Object:  Table [dbo].[CapacityCache]    Script Date: 07/29/2010 09:45:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CapacityCache](
	[customerAllocationID] [uniqueidentifier] NOT NULL,
	[capacity] [int] NOT NULL,
 CONSTRAINT [PK_CapacityCache] PRIMARY KEY CLUSTERED 
(
	[customerAllocationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



ALTER FUNCTION [dbo].[calculate_TotalCapacityCustomer] (@customerAllocationID uniqueidentifier)
RETURNS INT
AS
BEGIN
	declare @output int
	select @output = capacity from dbo.CapacityCache where customerAllocationID = @customerAllocationID
	
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
		FROM AnimationProductDetail apd
		LEFT JOIN  AnimationProduct ap ON ap.ID = apd.IDAnimationProduct
		LEFT JOIN Animation a ON a.ID = ap.IDAnimation
		LEFT JOIN [Signature] s ON s.ID = ap.IDSignature 
		WHERE apd.ID = @animationProductDetailID
		
		
		SELECT @output = ISNULL(SUM(cc.Capacity),0)
		FROM CustomerCapacity cc
		INNER JOIN Customer c ON c.ID = cc.IDCustomer
		INNER JOIN CustomerGroup cg ON cg.ID = c.IDCustomerGroup
		--c.	Store category matches that entered against the Animation Product.	
		WHERE 
		c.ID = @customerID AND
		c.IncludeInSystem = 1 AND cg.IncludeInSystem = 1 AND c.Deleted = 0
		AND	EXISTS(SELECT ID FROM CustomerCategory WHERE IDCustomer = c.ID AND IDCategory = @CategoryID)
		--e.	Capacity is for the Promotion Type and Priority entered against the Animation.
		AND IDPriority = @PriorityID AND IDAnimationType = @AnimationTypeID
		--f.	Capacity is for the Item Type entered against the Animation Product.
		AND IDItemType = @ItemTypeID 
		--b.	Store is valid for the allocation country
		AND ((c.IDSalesArea_AllocationSalesArea = @SalesAreaID) OR (c.IDSalesArea_AllocationSalesArea IS NULL))
		--g.	Customer groups are included in the animation
		--AND (EXISTS (SELECT ID FROM AnimationCustomerGroup WHERE IDCustomerGroup = cg.ID))
		--d.	Store sells the ‘allocated by’ Signature (& Brand/Axe if entered) i.e. sales exist for the signature/brand/axe at any point over the past 12 months.
		AND (EXISTS (SELECT s.ID FROM Sale s 
					 LEFT JOIN BrandAxe ba ON ba.ID = s.IDBrandAxe 
						WHERE  s.IDCustomer = c.ID
						and not exists (select * from dbo.CustomerBrandExclusion where IDCustomer = c.ID and IDBrandAxe = s.IDBrandAxe and Excluded = 1)
						AND ((@BrandAxeID IS NOT NULL AND IDBrandAxe = @BrandAxeID) OR (@BrandAxeID IS NULL AND ba.IDSignature = @SignatureID))
						AND s.[Date] > DATEADD(year,-1,GETDATE())
			))
	end	
		
	
	return isnull(@output, 0)

END
GO

insert into dbo.CapacityCache
	select ID, dbo.calculate_TotalCapacityCustomer(ID)
		from dbo.CustomerAllocation