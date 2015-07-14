IF OBJECT_ID (N'dbo.calculate_RRP_UK', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_RRP_UK;
GO

CREATE FUNCTION dbo.calculate_RRP_UK (@AnimationProductID uniqueidentifier, @IDDivision uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	declare @RrpUK int

	SELECT @RrpUK = SUM(ISNULL(apd.RRP,0))
	FROM dbo.AnimationProductDetail apd
	LEFT JOIN dbo.AnimationProduct ap ON ap.ID = apd.IDAnimationProduct
	LEFT JOIN dbo.SalesArea sa ON sa.ID = apd.IDSalesArea
	LEFT JOIN dbo.DistributionChannel dc ON dc.ID = sa.IDDistributionChannel
	LEFT JOIN dbo.SalesOrganization so ON so.ID = sa.IDSalesOrganization
	WHERE ap.ID = @AnimationProductID AND dc.Code = '02' AND so.Code = '0300'	AND sa.IDDivision = @IDDivision	
	
	return isnull(@RrpUK,0)
END