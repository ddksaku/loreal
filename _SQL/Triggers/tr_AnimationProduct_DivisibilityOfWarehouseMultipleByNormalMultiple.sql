if object_id('tr_AnimationProduct_DivisibilityOfWarehouseMultipleByNormalMultiple') > 0
	drop trigger tr_AnimationProduct_DivisibilityOfWarehouseMultipleByNormalMultiple
go

create trigger tr_AnimationProduct_DivisibilityOfWarehouseMultipleByNormalMultiple
on dbo.AnimationProduct
for update
as
begin

	declare @animationProductID uniqueidentifier
	declare @multipleOldID uniqueidentifier
	declare @multipleNewID uniqueidentifier
	declare @multipleWarehouseOldID uniqueidentifier
	declare @multipleWarehouseNewID uniqueidentifier
	declare @errorMsg nvarchar(max)
	declare @newLineChar char(2)
	set @newLineChar = CHAR(13) + CHAR(10)
	
	declare @normalMultiple int
	declare @warehouseMultiple int

	select @animationProductID = ID, @multipleNewID = IDMultipleNormal, @multipleWarehouseNewID = IDMutlipleWarehouse
		from inserted
		
	select 	@multipleOldID = IDMultipleNormal, @multipleWarehouseOldID = IDMutlipleWarehouse
		from deleted
		
		
	-- ** warehouse multiple changed to not blank value
	IF 	@multipleWarehouseNewID IS NOT NULL
		 AND @multipleWarehouseNewID <> ISNULL(@multipleWarehouseOldID, newid())
		 AND @multipleNewID IS NOT NULL
	begin		
		
		select @normalMultiple = Value from dbo.Multiple
			where ID = @multipleNewID
			
		select 	@warehouseMultiple = Value from dbo.Multiple
			where ID = @multipleWarehouseNewID
			
		if 	@warehouseMultiple % @normalMultiple > 0
		begin
			
			/*
				Warehouse multiple must be divisible by normal multiple.
			*/		
			SET @errorMsg =  dbo.uf_getSystemMessage('tr_AnimationProduct_DivisibilityOfWarehouseMultipleByNormalMultiple', null, null, null, null, null, null, null, null)
			RAISERROR (@errorMsg, 16, 35)
			ROLLBACK TRAN
			RETURN	
		
		end	
	
	end
	
	-- ** normal multiple changed to not blank value
	IF @multipleNewID IS NOT NULL
		AND @multipleNewID <> ISNULL(@multipleOldID, newid())
		AND @multipleWarehouseNewID IS NOT NULL
	begin	
	
		select @normalMultiple = Value from dbo.Multiple
			where ID = @multipleNewID
			
		select 	@warehouseMultiple = Value from dbo.Multiple
			where ID = @multipleWarehouseNewID	
	
		if 	@warehouseMultiple % @normalMultiple > 0
		begin
			
			/*
				Warehouse multiple must be divisible by normal multiple.
			*/		
			SET @errorMsg =  dbo.uf_getSystemMessage('tr_AnimationProduct_DivisibilityOfWarehouseMultipleByNormalMultiple', null, null, null, null, null, null, null, null)
			RAISERROR (@errorMsg, 16, 35)
			ROLLBACK TRAN
			RETURN	
		
		end	
	
	
	
	end	


end	