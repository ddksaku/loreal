if object_id('tr_Animation_recalculateCapacityCache') > 0
	drop trigger tr_Animation_recalculateCapacityCache
go

create trigger tr_Animation_recalculateCapacityCache
on dbo.Animation
after update, insert
as
begin

	-- trigger used for recalculate possibly impacted capacities from capacity cache
	declare @animationID uniqueidentifier
	declare @statusOld int
	declare @statusNew int
	declare @priorityOld uniqueidentifier
	declare @priorityNew uniqueidentifier
	declare @animationTypeOld uniqueidentifier
	declare @animationTypeNew uniqueidentifier	
	
	select @statusOld = [Status], @priorityOld = IDPriority, @animationTypeOld = IDAnimationType from deleted
	select @animationID = ID, @statusNew = [Status], @priorityNew = IDPriority, @animationTypeNew = IDAnimationType
		from inserted
	
	-- determine cases when we need to recalculate
	-- animation is active after saving
	if @statusNew in (1,2,3,4) 
	begin
	
		-- some value imppacting capacity is changed
		if @priorityOld <> @priorityNew OR @animationTypeOld <> @animationTypeNew
		begin
	
			-- find animation product details
			declare @animationProductDetails table
			(ID uniqueidentifier)
			
			insert into @animationProductDetails
				select a.ID
					from dbo.AnimationProductDetail as a
						inner join dbo.AnimationProduct as b on (a.IDAnimationProduct = b.ID)
						where b.IDAnimation = @animationID
						
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

end	