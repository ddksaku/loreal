IF OBJECT_ID (N'dbo.calculate_ListPriceUK', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_ListPriceUK;
GO

CREATE FUNCTION dbo.calculate_ListPriceUK (@AnimationProductID uniqueidentifier, @DivisionID uniqueidentifier)
RETURNS float
WITH SCHEMABINDING
AS
BEGIN
	DECLARE @RRPToListPriceUK FLOAT
	SELECT @RRPToListPriceUK = ISNULL(RRPToListPriceRate,0) 
	FROM dbo.SalesArea sa 
	LEFT JOIN dbo.DistributionChannel dc ON dc.ID = sa.IDDistributionChannel
	LEFT JOIN dbo.SalesOrganization so ON so.ID = sa.IDSalesOrganization
	WHERE dc.Code = '02' AND so.Code = '0300'	AND sa.IDDivision = @DivisionID
	
	DECLARE @ListPriceUK float
	SELECT @ListPriceUK = ISNULL(SUM(RRP * @RRPToListPriceUK),0)
	FROM dbo.AnimationProductDetail apd
	LEFT JOIN dbo.AnimationProduct ap ON ap.ID = apd.IDAnimationProduct
	WHERE ap.ID = @AnimationProductID

	RETURN isnull(@ListPriceUK ,0)

END