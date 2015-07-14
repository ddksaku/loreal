IF OBJECT_ID (N'dbo.calculate_TotalBDCQuantity', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_TotalBDCQuantity;
GO

CREATE FUNCTION dbo.calculate_TotalBDCQuantity (@AnimationProductID uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	declare @TotalBDCQuantity int	

	SELECT @TotalBDCQuantity=ISNULL(SUM(apd.BDCQuantity),0)
		FROM dbo.AnimationProductDetail apd
		LEFT JOIN dbo.AnimationProduct ap ON ap.ID = apd.IDAnimationProduct
		WHERE ap.ID = @AnimationProductID


	RETURN ISNULL(@TotalBDCQuantity,0)

END