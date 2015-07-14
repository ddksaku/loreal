-- need find out, what Closed Store actually means in DB

IF OBJECT_ID ('tr_Product_Dead', 'TR') IS NOT NULL
   DROP TRIGGER tr_Product_Dead;
GO

CREATE TRIGGER tr_Product_Dead
ON Product
AFTER UPDATE 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	-- 358.	A weekly audit email is required to notify the division administrators of key data 
	-- changes / additions in SAP. The following data should be monitored and changes reported:
	-- e.	Sales Status changes to dead (sales status 9) (only for products included on non-Cleared animations).
	declare @newValue nvarchar(50)
	declare @oldValue nvarchar(50)

	select @newValue = [Status] from inserted
	select @oldValue = [Status]  from deleted

	if @newValue <> @oldValue AND @newValue = 9
	begin

		-- are there non cleared animations where the product is included
		declare @productId uniqueidentifier
		declare @divisionId uniqueidentifier
		
		select @productId = ID from inserted
		select @divisionId = IDDivision from inserted
		
		declare @animations table (ID uniqueidentifier)		

		insert into @animations
			select b.ID
				from  dbo.AnimationProduct as a 
				inner join dbo.Animation as b on (a.IDAnimation = b.ID)
					where a.IDProduct = @productId and b.Status <> 6 -- non cleared
			
		declare @count int
		select @count = count(*) from @animations

		if @count > 0
		begin
			--> alert
			declare @alertMessage nvarchar(max)
			declare @newLineChar char(2)
			declare @productName nvarchar(100)
			declare @productCode nvarchar(20)
			DECLARE @modifiedBy NVARCHAR(255)
			SELECT @modifiedBy = ModifiedBy FROM INSERTED 

			select @productName = [Description] from inserted
			select @productCode = MaterialCode from inserted
			set @newLineChar = CHAR(13) + CHAR(10)

			declare @alert table (animationId uniqueidentifier, alertMessage nvarchar(max))

			insert into @alert
				select a.ID, dbo.uf_getSystemMessage('tr_Product_Dead', b.Name, b.Code, @productName, @productCode, null ,null, null, null)
				
							/*'There is a product on this animation which status has been set to dead' + @newlineChar +
							'Animation: ' + b.Name + ' ('+ b.Code + ')' + @newLineChar +
							'Product: ' + @productName + ' (' + @productCode + ')'	*/			
					from @animations as a 
					inner join dbo.Animation as b on (a.ID = b.ID)		

			insert into dbo.AuditAlert (AlertType, AlertDescription, DateCreated, IDDivision, Processed, OriginalValue, NewValue, ModifiedBy)
				select 'Animation', alertMessage, getdate(), @divisionId, 0, @oldValue, @newValue, @modifiedBy
					from @alert		
	
			end	
		end
	
	RETURN
END