IF OBJECT_ID ('tr_Multiple_MultipleChanged', 'TR') IS NOT NULL
   DROP TRIGGER tr_Multiple_MultipleChanged;
GO

CREATE TRIGGER tr_Multiple_MultipleChanged
ON Multiple
AFTER UPDATE 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	declare @oldValue int
	declare @newValue int
	declare @oldValueString nvarchar(25)
	declare @newValueString nvarchar(25)
	DECLARE @modifiedBy NVARCHAR(255)
	
	
	SELECT @modifiedBy = ModifiedBy FROM inserted
	select @oldValue = [Value] from deleted
	select @newValue = [Value] from inserted
	
	select @oldValueString = cast(@oldValue as nvarchar(25))
	select @newValueString = cast(@newValue as nvarchar(25))

	-- 95.	If a product multiple changes when the product exists on an active (not closed or cleared) animation 
	-- (and the original multiple is no longer valid)
	IF  @oldValue <> @newValue
	begin
		declare @productID uniqueidentifier
		select @productID = IDProduct from inserted	
		
		declare @divisionID uniqueidentifier
		declare @productCode nvarchar(20)
		select @productCode = MaterialCode, @divisionID = IDDivision from dbo.Product where ID = @productID
		
		
		DECLARE @newLineChar char(2)
		SET @newLineChar = CHAR(13) + CHAR(10)
		
		declare @animationProducts table
		(ID uniqueidentifier, animationID uniqueidentifier)
		
		insert into @animationProducts
			select a.ID, b.ID from dbo.AnimationProduct as a 
				inner join dbo.Animation as b on (a.IDAnimation = b.ID) 
				where IDProduct = @productID AND b.[Status] IN (1,2,3,4)

		declare @count int
		select @count = count(*) from @animationProducts
			 
		IF (@count > 0)
		begin
			-- then the original multiple will be cleared out of the animation and set to {blank}
			declare @multipleID uniqueidentifier
			select @multipleID = ID from inserted		
			
			UPDATE dbo.AnimationProduct SET IDMultipleNormal = NULL, ModifiedDate = getdate()
				WHERE IDMultipleNormal = @multipleID AND ID IN (SELECT ID FROM @animationProducts)
				
			UPDATE dbo.AnimationProduct SET IDMutlipleWarehouse = NULL, ModifiedDate = getdate()
				WHERE IDMutlipleWarehouse = @multipleID AND ID IN (SELECT ID FROM @animationProducts)	
				
			
		declare @animationsList nvarchar(max)		
		SELECT	@animationsList = COALESCE( @animationsList + ', ', '') + Name FROM Animation
				
		-- This will then be highlighted via the audit email so the division administrator can request it be updated.	
		declare @auditDescription nvarchar(2000)
		set @auditDescription = dbo.uf_getSystemMessage('tr_Multiple_MultipleChanged', @productCode, @animationsList, null, null, null, null, null, null)
		/*set @auditDescription = 'Product multiple changed on product existing on an active animation.' + @newLineChar + 
								'Product code: ' + @productCode + @newLineChar + 
								'Animation(s): ' + @animationsList*/
		
		
		
		exec up_SaveAuditAlert
			 'Multiples'
			,@auditDescription
			,@divisionID
			,@oldValueString
			,@newValueString
			,@modifiedBy
		
		end	
	end

	RETURN
END