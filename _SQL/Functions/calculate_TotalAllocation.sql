IF OBJECT_ID (N'dbo.calculate_TotalAllocation', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_TotalAllocation;
GO

CREATE FUNCTION dbo.calculate_TotalAllocation (@AnimationProductID uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	declare @TotalAllocation int	

	SELECT @TotalAllocation=ISNULL(SUM(apd.AllocationQuantity),0)
		FROM dbo.AnimationProductDetail apd
		LEFT JOIN dbo.AnimationProduct ap ON ap.ID = apd.IDAnimationProduct
		WHERE ap.ID = @AnimationProductID

	RETURN isnull(@TotalAllocation,0)

END