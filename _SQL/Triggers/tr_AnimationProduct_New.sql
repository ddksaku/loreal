if object_id('[tr_AnimationProduct_New]') > 0
	drop trigger [tr_AnimationProduct_New]
go

CREATE TRIGGER [dbo].[tr_AnimationProduct_New]
ON [dbo].[AnimationProduct]
AFTER INSERT 
AS 
BEGIN

	IF @@ROWCOUNT = 0 RETURN 
	
	declare @animationProductID uniqueidentifier
	select @animationProductID = ID FROM inserted

	declare @animationID uniqueidentifier
	select @animationID = IDAnimation from inserted	

	declare @divisonID uniqueidentifier
	select @divisonID = IDDivision from Animation where ID = @animationID

	declare @salesAreasToInsert table
	(ID uniqueidentifier)

	insert into @salesAreasToInsert
		select sa.ID from dbo.SalesArea as sa
			where Deleted = 0
			AND exists (select acg.ID from dbo.AnimationCustomerGroup as acg
						INNER JOIN dbo.CustomerGroup as cg on cg.ID = acg.IDCustomerGroup
				        where acg.IDAnimation = @animationID AND cg.IDSalesArea = sa.ID)
		UNION 			        
		select distinct c.IDSalesArea_AllocationSalesArea 
						from dbo.AnimationCustomerGroup as acg 
							INNER JOIN dbo.CustomerGroup as cg on cg.ID = acg.IDCustomerGroup
							INNER JOIN dbo.Customer as c on (c.IDCustomerGroup = cg.ID)
						   where acg.IDAnimation = @animationID     
				       
	
	
	-- insert new Animation Product Details
	insert into dbo.AnimationProductDetail (ID, IDAnimationProduct, IDSalesArea, ModifiedBy, ModifiedDate, BDCQuantity)
		select NEWID(), @animationProductID, ID, 'New Animation Product - automatically created', getdate(), 0
			from @salesAreasToInsert
			

	
	
	RETURN
END
	
	
