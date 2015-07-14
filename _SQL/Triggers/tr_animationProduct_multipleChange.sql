if object_id('tr_animationProduct_multipleChange') > 0
	drop trigger tr_animationProduct_multipleChange
go

create trigger tr_animationProduct_multipleChange
on dbo.AnimationProduct
after UPDATE 
as
begin

	declare @IDMultipleNormalNew uniqueidentifier
	declare @IDMultipleNormalOld uniqueidentifier

	declare @IDMutlipleWarehouseNew uniqueidentifier
	declare @IDMutlipleWarehouseOld uniqueidentifier
	
	declare @productID uniqueidentifier
	declare @animationID uniqueidentifier
	declare @divisionID uniqueidentifier

	select @animationID = IDAnimation from inserted
	select @productID = IDProduct from inserted
	select @IDMultipleNormalNew = IDMultipleNormal from inserted
	select @IDMultipleNormalOld = IDMultipleNormal from deleted
	select @IDMutlipleWarehouseNew = IDMutlipleWarehouse from inserted
	select @IDMutlipleWarehouseOld = IDMutlipleWarehouse from deleted

	declare @errorMsg nvarchar(max)
	declare @auditDescription nvarchar(2000)
	declare @count int
	declare @animation nvarchar(255)	
		
	declare @modifiedBy nvarchar(255)
	select @modifiedBy = ModifiedBy from inserted
	
	declare @productCode nvarchar(255)
	select @productCode = MaterialCode, @divisionID = IDDivision from dbo.Product where ID = @productID
	
	declare @animationName nvarchar(255)
	select @animationName = [Name] from dbo.Animation where ID = @animationID
		
	set @count = 0

	declare @multiple int

	declare @animationProductID uniqueidentifier
	select @animationProductID = ID from inserted


	declare @animationProductDetails table
	(ID uniqueidentifier)

	insert into @animationProductDetails
		select ID 
			from dbo.AnimationProductDetail
				where IDAnimationProduct = @animationProductID
			
	-- multiple changed to not null value
	IF @IDMultipleNormalNew <> @IDMultipleNormalOld 
	begin
			
		if @IDMultipleNormalNew is null
		begin
		
			--> alert
			set @auditDescription = dbo.uf_getSystemMessage('tr_AnimationProduct_NormalMultipleChangedAlert', @productCode, @animationName, null, null, null, null, null, null)
			
			declare @oldNormalValueString nvarchar(55)
			declare @newNormalValueString nvarchar(55)
			
			select @oldNormalValueString = [Value] from dbo.Multiple where ID = @IDMultipleNormalOld
			select @newNormalValueString = [Value] from dbo.Multiple where ID = @IDMultipleNormalNew
			
			exec up_SaveAuditAlert
				 'AnimationProduct'
				,@auditDescription
				,@divisionID
				,@oldNormalValueString
				,@newNormalValueString
				,@modifiedBy
			
		end	
		else
		begin
				
			select @multiple = [Value] from dbo.Multiple WHERE ID = @IDMultipleNormalNew

			-- find possible impacted items
			select @count = count(*) from dbo.CustomerGroupAllocation
				where IDAnimationProductDetail IN (select ID from @animationProductDetails)
					  and (SystemFixedAllocation % @multiple > 0 OR ManualFixedAllocation % @multiple > 0)

			select @count = @count + count(*) from dbo.CustomerAllocation
				where IDAnimationProductDetail IN (select ID from @animationProductDetails)
					  and FixedAllocation % @multiple > 0
			 

			select @count = @count + count(*) from dbo.AnimationProductDetail
				where ID in (select ID from @animationProductDetails)
					and AllocationQuantity % @multiple > 0


			if @count > 0
			begin
				SET @errorMsg = dbo.uf_getSystemMessage('tr_animationProduct_multipleChange', null, null, null, null, null, null, null, null )
				RAISERROR (@errorMsg, 16, 30)
				ROLLBACK TRAN
				RETURN	
			end

		end	
	end

	IF @IDMutlipleWarehouseNew <> @IDMutlipleWarehouseOld 
	begin
			
		if @IDMutlipleWarehouseNew is null
		begin
			--> alert
			set @auditDescription = dbo.uf_getSystemMessage('tr_AnimationProduct_WarehouseMultipleChangedAlert', @productCode, @animationName, null, null, null, null, null, null)
			
			declare @oldWarehouseValueString nvarchar(55)
			declare @newWarehouseValueString nvarchar(55)
			
			select @oldWarehouseValueString = [Value] from dbo.Multiple where ID = @IDMutlipleWarehouseOld
			select @newWarehouseValueString = [Value] from dbo.Multiple where ID = @IDMutlipleWarehouseNew
			
			exec up_SaveAuditAlert
				 'AnimationProduct'
				,@auditDescription
				,@divisionID
				,@oldWarehouseValueString
				,@newWarehouseValueString
				,@modifiedBy
		end	
		else
		begin			

			SET @errorMsg = dbo.uf_getSystemMessage('tr_animationProduct_multipleChange', null, null, null, null, null, null, null, null )
			RAISERROR (@errorMsg, 16, 30)
			ROLLBACK TRAN
			RETURN	

		end		
	end
		
	
end


