if object_id('tr_AnimationProduct_recalculateCapacityCache') > 0
	drop trigger tr_AnimationProduct_recalculateCapacityCache
go	

CREATE trigger [dbo].[tr_AnimationProduct_recalculateCapacityCache]
on [dbo].[AnimationProduct]
after update
as
begin


	declare @itemTypeNew uniqueidentifier
	declare @itemTypeOld uniqueidentifier
	declare @signatureOld uniqueidentifier
	declare @signatureNew uniqueidentifier
	declare @brandAxeOld uniqueidentifier
	declare @brandAxeNew uniqueidentifier
	declare @categoryOld uniqueidentifier
	declare @categoryNew uniqueidentifier

	declare @animationProduct uniqueidentifier
	select @itemTypeNew = IDItemType, @signatureNew = IDSignature, @brandAxeNew = IDBrandAxe, @categoryNew = IDCategory   from inserted
	select @itemTypeOld = IDItemType, @signatureOld = IDSignature, @brandAxeOld = IDBrandAxe, @categoryOld = IDCategory from deleted
	

	
	if @itemTypeNew = @itemTypeOld 
		AND @signatureNew = @signatureOld 
		AND @brandAxeOld = @brandAxeNew
		AND @categoryNew = @categoryOld
	begin
			-- nothing happened
			return		
	end
	else
	begin
	
		-- find animation product details
		declare @animationProductDetails table
		(ID uniqueidentifier)
		
		insert into @animationProductDetails
			select ID
				from dbo.AnimationProductDetail 
					where IDAnimationProduct  = @animationProduct
					
		-- find customer allocation IDs			
		declare @allocationIDs table
		(ID uniqueidentifier)
		
		insert into @allocationIDs		
			select ID from dbo.CustomerAllocation 
				where IDAnimationProductDetail in (select ID from @animationProductDetails)
					
		 update dbo.CapacityCache set capacity = dbo.calculate_TotalCapacityCustomer(IDCustomerAllocation)	
			where IDCustomerAllocation in (select ID from @allocationIDs)	
	
	end

end
