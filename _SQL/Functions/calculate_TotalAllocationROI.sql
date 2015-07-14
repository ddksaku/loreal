IF OBJECT_ID (N'dbo.calculate_TotalAllocationROI', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_TotalAllocationROI;
GO

CREATE FUNCTION dbo.calculate_TotalAllocationROI (@AnimationProductID uniqueidentifier, @IDDivision uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	declare @TotalAllocationROI int

	SELECT @TotalAllocationROI = ISNULL(SUM(apd.AllocationQuantity),0)
	FROM dbo.AnimationProductDetail apd
	LEFT JOIN dbo.AnimationProduct ap ON ap.ID = apd.IDAnimationProduct
	LEFT JOIN dbo.SalesArea sa ON sa.ID = apd.IDSalesArea
	LEFT JOIN dbo.DistributionChannel dc ON dc.ID = sa.IDDistributionChannel
	LEFT JOIN dbo.SalesOrganization so ON so.ID = sa.IDSalesOrganization
	WHERE ap.ID = @AnimationProductID AND dc.Code = '02' AND so.Code = '0350'	AND sa.IDDivision = @IDDivision
	
	return isnull(@TotalAllocationROI,0)
END