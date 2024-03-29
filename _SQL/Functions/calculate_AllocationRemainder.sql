if object_id('calculate_AllocationRemainder') > 0
	drop function [calculate_AllocationRemainder]
go

CREATE FUNCTION [dbo].[calculate_AllocationRemainder] (@AnimationProductDetailID uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	declare @allocationQuantity int	
	SELECT @allocationQuantity = AllocationQuantity FROM dbo.AnimationProductDetail WHERE ID = @AnimationProductDetailID

	declare @allocated int
	select @allocated = SUM(ISNULL(CalculatedAllocation,0)) FROM dbo.CustomerAllocation
		WHERE IDAnimationProductDetail = @AnimationProductDetailID		

	RETURN ISNULL(@allocationQuantity - @allocated,0)

END
