IF OBJECT_ID (N'dbo.calculate_TotalForecastQuantity', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_TotalForecastQuantity;
GO

CREATE FUNCTION dbo.calculate_TotalForecastQuantity (@AnimationProductID uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	declare @TotalForecastQuantity int
	

	SELECT @TotalForecastQuantity=ISNULL(SUM(apd.ForecastProcQuantity),0)
		FROM dbo.AnimationProductDetail apd
		LEFT JOIN dbo.AnimationProduct ap ON ap.ID = apd.IDAnimationProduct
		WHERE ap.ID = @AnimationProductID


	RETURN ISNULL(@TotalForecastQuantity, 0)

END