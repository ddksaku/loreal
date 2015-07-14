IF OBJECT_ID (N'dbo.calculate_TotalCapacity', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_TotalCapacity;
GO

CREATE FUNCTION dbo.calculate_TotalCapacity (@AnimationProductID uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	declare @output int
	
	declare @animationProductDetails table
	(ID uniqueidentifier)
	
	insert into @animationProductDetails
		select ID from dbo.AnimationProductDetail
			where IDAnimationProduct = @AnimationProductID
	
	declare @t table
	(customerAllocationID uniqueidentifier, capacity int)
	
	insert into @t (customerAllocationID)
		select ID
			from dbo.CustomerAllocation
				where IDAnimationProductDetail in (select ID from @animationProductDetails)
		
	update 	@t set capacity = dbo.calculate_TotalCapacityCustomer(customerAllocationID)			
		
	select @output = sum(isnull(capacity,0)) from @t

	RETURN ISNULL(@output, 0)	

END