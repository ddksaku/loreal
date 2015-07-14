IF OBJECT_ID (N'dbo.calculate_ListPriceROI', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_ListPriceROI;
GO

CREATE FUNCTION dbo.calculate_ListPriceROI (@AnimationProductID uniqueidentifier, @DivisionID uniqueidentifier)
RETURNS float
WITH SCHEMABINDING
AS
BEGIN
	DECLARE @RRPToListPriceUK FLOAT
	SELECT @RRPToListPriceUK = ISNULL(RRPToListPriceRate,0) 
	FROM dbo.SalesArea sa 
	LEFT JOIN dbo.DistributionChannel dc ON dc.ID = sa.IDDistributionChannel
	LEFT JOIN dbo.SalesOrganization so ON so.ID = sa.IDSalesOrganization
	WHERE dc.Code = '02' AND so.Code = '0350'	AND sa.IDDivision = @DivisionID
	
	DECLARE @ListPriceUK float
	SELECT @ListPriceUK = ISNULL(SUM(RRP * @RRPToListPriceUK),0)
	FROM dbo.AnimationProductDetail apd
	LEFT JOIN dbo.AnimationProduct ap ON ap.ID = apd.IDAnimationProduct
	WHERE ap.ID = @AnimationProductID

	RETURN isnull(@ListPriceUK,0)

END