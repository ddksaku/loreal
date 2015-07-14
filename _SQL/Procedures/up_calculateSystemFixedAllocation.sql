if object_id('up_calculateSystemFixedAllocation') > 0
begin
	drop procedure  up_calculateSystemFixedAllocation
end
go


create procedure up_calculateSystemFixedAllocation (@animationId uniqueidentifier)
as 
begin


	declare @status tinyint
	select @status = [Status] from dbo.Animation where ID = @animationId
	
	if @status >= 3
	begin

		declare @groups table (ID uniqueidentifier)	
		
		insert into @groups
			select IDCustomerGroup from dbo.AnimationCustomerGroup
				where IDAnimation = @animationId


		declare @animationProductDetails table (ID uniqueidentifier)
		
		insert into @animationProductDetails
			select	a.ID
				from dbo.AnimationProductDetail as a 
				inner join dbo.AnimationProduct as b on (a.IDAnimationProduct = b.ID)
				inner join dbo.Animation as c on (b.IDAnimation = c.ID)
				where c.ID = @animationId

		-- find customer group records for update
		UPDATE dbo.CustomerGroupAllocation SET 
				SystemFixedAllocation = 
					CASE 
						WHEN a.ManualFixedAllocation > 0 THEN a.ManualFixedAllocation 
						ELSE dbo.calculate_SumOfStoreAllocation(b.ID, c.ID)
					END
			FROM dbo.CustomerGroupAllocation as a 
			inner join @groups as b on (a.IDCustomerGroup = b.ID)
			inner join @animationProductDetails as c on (a.IDAnimationProductDetail = c.ID)
	end
		
end








/****** Script for SelectTopNRows command from SSMS  ******/
--exec up_calculateSystemFixedAllocation '824F5320-0843-4C16-B9AC-AD634EDAC1B2'