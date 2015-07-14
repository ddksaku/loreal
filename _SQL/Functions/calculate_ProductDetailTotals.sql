IF OBJECT_ID (N'dbo.calculate_ProductDetailTotals', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_ProductDetailTotals;
GO

CREATE FUNCTION dbo.calculate_ProductDetailTotals (@AnimationProductDetail uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	DECLARE @output int
	
	
	declare @t table
	(customerAllocation uniqueidentifier,
	 capacity int)
	 
	 insert into @t (customerAllocation)
		select ID from dbo.CustomerAllocation
			where IDAnimationProductDetail = @AnimationProductDetail
			
	 update @t set capacity = dbo.calculate_TotalCapacityCustomer(customerAllocation)			
	
	select @output = sum(isnull(capacity, 0)) from @t

	RETURN ISNULL(@output, 0)
	
END
