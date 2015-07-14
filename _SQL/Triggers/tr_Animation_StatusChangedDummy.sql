IF OBJECT_ID ('tr_Animation_StatusChangedDummy', 'TR') IS NOT NULL
   DROP TRIGGER tr_Animation_StatusChangedDummy;
GO

CREATE TRIGGER tr_Animation_StatusChangedDummy
ON Animation
AFTER UPDATE 
AS 
BEGIN

	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @animationID uniqueidentifier
	SELECT @animationID = ID FROM inserted	
	
	DECLARE @newValue int
	DECLARE @oldValue int
	
	SELECT @oldValue = [Status] FROM deleted
	SELECT @newValue = [Status] FROM inserted
	
	if @oldValue = @newValue
		return
	
	-- 94.	It is not possible to mark an animation as ‘Closed’ until all dummy products have been replaced 
	-- or removed from the allocation
	IF (@newValue = 5)
	BEGIN
		
		-- are there any dummy products included?
		declare @products table (ID uniqueidentifier)
		
		insert into @products
			select IDProduct
				from dbo.AnimationProduct as a inner join dbo.Product as b on (a.IDProduct = b.ID)
				where a.IDAnimation = @animationID AND b.[Manual] = 1
				
		declare @count int
		select @count = count(*) from @products	
		
		if @count > 0
		begin
			DECLARE @errorMsg nvarchar(max)
			DECLARE @newLineChar char(2)
			SET @newLineChar = CHAR(13) + CHAR(10)
			
			SET @errorMsg = 'Animation can not be closed, because there is at least one dummy products included'
			 
			RAISERROR (@errorMsg, 16, 14)
			ROLLBACK TRAN
			RETURN	
			
		end
		
	END	
	
	RETURN
END