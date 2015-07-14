IF OBJECT_ID ('tr_AnimationProduct_AllocationLogicChangedWarning', 'TR') IS NOT NULL
   DROP TRIGGER tr_AnimationProduct_AllocationLogicChangedWarning;
GO

CREATE TRIGGER tr_AnimationProduct_AllocationLogicChangedWarning
ON AnimationProduct
AFTER UPDATE 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	-- is animation Active
	declare @animationID uniqueidentifier
	select @animationID = IDAnimation from inserted
	declare @divisionID uniqueidentifier
	declare @productID uniqueidentifier

	select @productID = IDProduct from inserted
	declare @animationStatus int
	select @animationStatus = [Status], @divisionID = IDDivision	
		from dbo.Animation as a		
			where a.ID = @animationID
	
	if @animationStatus <> 5 AND @animationStatus <> 6
	begin
		declare @newLineChar char(2)
		set @newLineChar = CHAR(13) + CHAR(10)
		declare @productName nvarchar(200)
		declare @productCode nvarchar(20)
		declare @animationName nvarchar(100)
		declare @animationCode nvarchar(20)
		declare @allertMessage nvarchar(max)
		DECLARE @modifiedBy NVARCHAR(255)
		SELECT @modifiedBy = ModifiedBy FROM inserted

		select @animationName = [Name], @animationCode = [Code] from dbo.Animation where ID = @animationID
		select @productName = [Description], @productCode = [MaterialCode] from dbo.Product where ID = @productID


		-- Item Type
		declare @itemTypeOld uniqueidentifier
		declare @itemTypeNew uniqueidentifier

		select @itemTypeOld = IDItemType from deleted
		select @itemTypeNew = IDItemType from inserted

		if @itemTypeOld <> @itemTypeNew
		begin
			declare @itemOldName nvarchar(50)
			declare @itemNewName nvarchar(50)

			select @itemOldName = [Name] from dbo.ItemType where ID = @itemTypeOld
			select @itemNewName = [Name] from dbo.ItemType where ID = @itemTypeNew

			/*set @allertMessage = 'Animation allocation logic changed in allocate by item type' + @newLineChar +
								 'Animation: ' + @animationName + ' (' + @animationCode + ')' + @newLineChar +
								 'Product: ' + @productName + ' (' + @productCode + ')'*/
			set @allertMessage = dbo.uf_getSystemMessage('tr_AnimationProduct_AllocationLogicChanged01', @animationName, @animationCode, @productName, @productCode, null, null, null, null)			 

			insert into dbo.AuditAlert (AlertType, AlertDescription, DateCreated, IDDivision, OriginalValue, NewValue, Processed, ModifiedBy)
				values('Animation Product', @allertMessage, getdate(), @divisionID, @itemOldName, @itemNewName, 0, @modifiedBy)
			
		end

		-- Signature
		declare @signatureOld uniqueidentifier
		declare @signatureNew uniqueidentifier

		select @signatureOld = IDSignature from deleted
		select @signatureNew = IDSignature from inserted

		if @signatureOld <> @signatureNew
		begin
			declare @oldSigName nvarchar(50)
			declare @newSigName nvarchar(50)

			select @oldSigName = [Name] from dbo.Signature where ID = @signatureOld
			select @newSigName = [Name] from dbo.Signature where ID = @signatureNew

			/*set @allertMessage = 'Animation allocation logic changed in allocate by signature' + @newLineChar +
								 'Animation: ' + @animationName + ' (' + @animationCode + ')' + @newLineChar +
								 'Product: ' + @productName + ' (' + @productCode + ')'*/
			set @allertMessage = dbo.uf_getSystemMessage('tr_AnimationProduct_AllocationLogicChanged02', @animationName, @animationCode, @productName, @productCode, null, null, null, null)			 					 
			insert into dbo.AuditAlert (AlertType, AlertDescription, DateCreated, IDDivision, OriginalValue, NewValue, Processed, ModifiedBy)
				values('Animation Product', @allertMessage, getdate(), @divisionID, @oldSigName, @newSigName, 0, @modifiedBy)

		end

		-- BrandAxe
		declare @brandAxeOld uniqueidentifier 
		declare @brandAxeNew uniqueidentifier

		select @brandAxeOld = IDBrandAxe from deleted
		select @brandAxeNew = IDBrandAxe from inserted

		-- two randomly generated guids will always differ
		if  (ISNULL(@brandAxeOld, NEWID()) <> ISNULL(@brandAxeNew, NEWID())) 
		begin
			IF  NOT (@brandAxeOld IS NULL AND @brandAxeNew IS NULL)
			begin
				declare @oldBrandName nvarchar(50)
				declare @newBrandName nvarchar(50)

				select @oldBrandName = ISNULL([Name], '--') from dbo.BrandAxe where ID = @brandAxeOld
				select @newBrandName = ISNULL([Name], '--') from dbo.BrandAxe where ID = @brandAxeNew

				/*set @allertMessage = 'Animation allocation logic changed in allocate by brand/axe' + @newLineChar +
									 'Animation: ' + @animationName + ' (' + @animationCode + ')' + @newLineChar +
									 'Product: ' + @productName + ' (' + @productCode + ')'*/
									 
				set @allertMessage = dbo.uf_getSystemMessage('tr_AnimationProduct_AllocationLogicChanged03', @animationName, @animationCode, @productName, @productCode, null, null, null, null)			 					 
									 
									 
				insert into dbo.AuditAlert (AlertType, AlertDescription, DateCreated, IDDivision, OriginalValue, NewValue, Processed, ModifiedBy)
					values('Animation Product', @allertMessage, getdate(), @divisionID, ISNULL(@oldBrandName,'--'), ISNULL(@newBrandName,'--'), 0, @modifiedBy)
			end

		end
			

		-- Category
		declare @categoryOld UNIQUEIDENTIFIER
		DECLARE @categoryNew UNIQUEIDENTIFIER
		
		SELECT @categoryOld = IDCategory FROM DELETED
		SELECT @categoryNew = IDCategory FROM INSERTED
		
		IF  (ISNULL(@categoryNew, NEWID()) <> ISNULL(@categoryOld, NEWID())) 
		BEGIN
			IF NOT (@categoryNew IS NULL AND @categoryOld IS NULL)
			BEGIN
				DECLARE @oldCategoryName NVARCHAR(50)
				DECLARE @newCategoryName NVARCHAR(50)
				
				SELECT @oldCategoryName = ISNULL([Name],'--') FROM dbo.Category WHERE ID = @categoryOld
				SELECT @newCategoryName = ISNULL([Name],'--') FROM dbo.Category WHERE ID = @categoryNew
				
				/*SET @allertMessage = 'Animation allocation logic changed in allocate by category' + @newLineChar +
									 'Animation: ' + @animationName + ' (' + @animationCode + ')' + @newLineChar +
									 'Product: ' + @productName + ' (' + @productCode + ')'*/
				set @allertMessage = dbo.uf_getSystemMessage('tr_AnimationProduct_AllocationLogicChanged04', @animationName, @animationCode, @productName, @productCode, null, null, null, null)			 					 
									 
				insert into dbo.AuditAlert (AlertType, AlertDescription, DateCreated, IDDivision, OriginalValue, NewValue, Processed, ModifiedBy)
					values('Animation Product', @allertMessage, getdate(), @divisionID, ISNULL(@oldCategoryName,'--'), ISNULL(@newCategoryName,'--'), 0, @modifiedBy)
			END				 
				
		END


	END
	
	RETURN
END