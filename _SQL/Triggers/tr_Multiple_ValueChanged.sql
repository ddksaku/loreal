if object_id('tr_Multiple_ValueChanged') > 0
	drop trigger tr_Multiple_ValueChanged
go

create trigger tr_Multiple_ValueChanged
ON dbo.Multiple
AFTER UPDATE
AS
BEGIN

	declare @animationProductID uniqueidentifier
	declare @multipleOldID uniqueidentifier
	declare @multipleNewID uniqueidentifier
	declare @errorMsg nvarchar(max)
	declare @newLineChar char(2)
	set @newLineChar = CHAR(13) + CHAR(10)
	
	select @animationProductID = ID, @multipleNewID = IDMultipleNormal from inserted	
	select @multipleOldID = IDMultipleNormal from deleted
	
	-- if multiple value changed and new value is not blank
	if @multipleNewID <> @multipleOldID AND @multipleNewID IS NOT NULL
	begin
	
		-- get multiple value
		declare @multipleNewValue int
		select @multipleNewValue = Value from dbo.Multiple where ID = @multipleNewID


		-- ** Check Allocation Quantity on AnimationProductDetails
		declare @animationProductDetails table
		(animationProductDetailID uniqueidentifier, allocationQunatity int)
		
		insert into @animationProductDetails
			select ID, AllocationQuantity
				from dbo.AnimationProductDetail
					where IDAnimationProduct = @animationProductID
					
		declare @countDetailNotDivisible int
		
		select @countDetailNotDivisible = count(*) from @animationProductDetails
			where allocationQunatity % @multipleNewValue > 0 
			
		if @countDetailNotDivisible > 0
		begin
			/*
				There is at least one allocation quantity which is not divisible by new multiple value.
			*/		
			SET @errorMsg =  dbo.uf_getSystemMessage('tr_Multiple_ValueChanged_DivisibilityOfAllocationQuantity', null, null, null, null, null, null, null, null)
			RAISERROR (@errorMsg, 16, 32)
			ROLLBACK TRAN
			RETURN					
			
		end	
	
	
		-- ** Check customer group fixed allocation
		declare @customerGroupAllocations table
		(allocationID uniqueidentifier, fixed int)
		
		insert into @customerGroupAllocations
			select ID, ManualFixedAllocation
				from dbo.CustomerGroupAllocation
					where IDAnimationProductDetail in (select animationProductDetailID from @animationProductDetails)
						and ManualFixedAllocation
	
		declare @countGroupFixedNotDivisible int
		
		select @countGroupFixedNotDivisible = count(*) 
			from @customerGroupAllocations
				where fixed % @multipleNewValue > 0 
	
	
		if @countGroupFixedNotDivisible > 0
		begin			
			/*
				There is at least one customer group fixed allocation which is not divisible by new multiple value.
			*/		
			SET @errorMsg =  dbo.uf_getSystemMessage('tr_Multiple_ValueChanged_DivisibilityOfGroupFixed', null, null, null, null, null, null, null, null)
			RAISERROR (@errorMsg, 16, 33)
			ROLLBACK TRAN
			RETURN			
		
		end
		
		
		
		-- ** Check customer fixed allocation
		declare @customerAllocations table
		(
			customerAllocationID uniqueidentifier,
			fixed int
		)
		
		insert into @customerAllocations
			select ID, FixedAllocation
				from dbo.CustomerAllocation
					where IDAnimationProductDetail in (select animationProductDetailID from @animationProductDetails)
						and FixedAllocation > 0
						
		declare @countCustomerFixedNotDivisible int
		
		select @countCustomerFixedNotDivisible = count(*)
			from @customerAllocations
				where fixed % @multipleNewValue > 0
				
		if 	@countCustomerFixedNotDivisible > 0
		begin
			/*
				There is at least one customer fixed allocation which is not divisible by new multiple value.
			*/		
			SET @errorMsg =  dbo.uf_getSystemMessage('tr_Multiple_ValueChanged_DivisibilityOfCustomerFixed', null, null, null, null, null, null, null, null)
			RAISERROR (@errorMsg, 16, 34)
			ROLLBACK TRAN
			RETURN	
				
		end
		
	
	
	
	
	end


END