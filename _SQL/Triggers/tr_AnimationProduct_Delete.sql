IF OBJECT_ID ('tr_AnimationProduct_Delete', 'TR') IS NOT NULL
   DROP TRIGGER tr_AnimationProduct_Delete;
GO

CREATE TRIGGER tr_AnimationProduct_Delete
ON AnimationProduct
AFTER DELETE 
AS 
BEGIN

	IF @@ROWCOUNT = 0 RETURN 
	
	declare @animationProductID uniqueidentifier
	select @animationProductID = ID FROM inserted

	declare @animatiioProductDetails table
	(ID uniqueidentifier)

	-- delete from Customer Allocation
	delete from dbo.CustomerAllocation
		where IDAnimationProductDetail in (select ID from @animatiioProductDetails)

	-- delete from Customer Group Allocation
	delete from dbo.CustomerGroupAllocation
		where IDAnimationProductDetail in (select ID from @animatiioProductDetails)

	

	-- delete from Animation Product Detail
	delete from dbo.AnimationProductDetail
		where IDAnimationProduct = @animationProductID
	
	
	RETURN
END