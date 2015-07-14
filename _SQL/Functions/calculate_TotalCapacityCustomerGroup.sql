IF OBJECT_ID (N'dbo.calculate_TotalCapacityCustomerGroup', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_TotalCapacityCustomerGroup;
GO

CREATE FUNCTION dbo.calculate_TotalCapacityCustomerGroup (@customerGroupAllocationID uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN

	declare @output int
	declare @animationProductDetailID uniqueidentifier
	
	select @animationProductDetailID = IDAnimationProductDetail 
		from dbo.CustomerGroupAllocation
			where ID = @customerGroupAllocationID
	
	declare @t table
	(customerAllocationID uniqueidentifier, capacity int)
	
	insert into @t (customerAllocationID)
		select ID
			from dbo.CustomerAllocation where IDAnimationProductDetail = @animationProductDetailID
			
	update 	@t set capacity = dbo.calculate_TotalCapacityCustomer(customerAllocationID)	
	
	select @output = sum(isnull(capacity, 0)) from @t


	RETURN ISNULL(@output, 0)

END