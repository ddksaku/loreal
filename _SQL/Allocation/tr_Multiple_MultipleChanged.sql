IF OBJECT_ID ('tr_Multiple_MultipleChanged', 'TR') IS NOT NULL
   DROP TRIGGER tr_Multiple_MultipleChanged;
GO

CREATE TRIGGER tr_Multiple_MultipleChanged
ON Multiple
AFTER UPDATE 
AS 
BEGIN

	-- 95.	If a product multiple changes when the product exists on an active (not closed or cleared) animation 
	-- (and the original multiple is no longer valid) 
	declare @productID uniqueidentifier
	select @productID = IDProduct from inserted	
	
	declare @animationProducts table
	(ID uniqueidentifier)
	
	insert into @animationProducts
		select a.ID from dbo.AnimationProduct as a 
			inner join dbo.Animation as b on (a.IDAnimation = b.ID) 
			where IDProduct = @productID AND b.[Status] IN (1,2,3,4)

	declare @count int
	select @count = count(*) from @animationProducts
		 
	IF (@count > 0)
	begin
		-- then the original multiple will be cleared out of the animation and set to {blank}
		declare @multipleID uniqueidentifier
		select @multipleID = ID from inserted
		
		UPDATE Multiple SET [Value] = NULL WHERE ID = @multipleID
	
	end	

	RETURN
END