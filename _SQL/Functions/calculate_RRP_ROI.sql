IF OBJECT_ID (N'dbo.calculate_RRP_ROI', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_RRP_ROI;
GO

CREATE FUNCTION dbo.calculate_RRP_ROI (@AnimationProductID uniqueidentifier, @IDDivision uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	declare @RrpROI int

	SELECT @RrpROI = SUM(ISNULL(apd.RRP,0))
	FROM dbo.AnimationProductDetail apd
	LEFT JOIN dbo.AnimationProduct ap ON ap.ID = apd.IDAnimationProduct
	LEFT JOIN dbo.SalesArea sa ON sa.ID = apd.IDSalesArea
	LEFT JOIN dbo.DistributionChannel dc ON dc.ID = sa.IDDistributionChannel
	LEFT JOIN dbo.SalesOrganization so ON so.ID = sa.IDSalesOrganization
	WHERE ap.ID = @AnimationProductID AND dc.Code = '02' AND so.Code = '0350'	AND sa.IDDivision = @IDDivision		
	
	return isnull(@RrpROI,0)
END