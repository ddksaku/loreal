IF OBJECT_ID (N'dbo.calculate_ProductConfirmed', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_ProductConfirmed;
GO

CREATE FUNCTION dbo.calculate_ProductConfirmed (@productID uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	declare @maxYear int
	declare @maxMonth int
	DECLARE @return int

	SELECT @maxYear = MAX(Year) FROM dbo.ProductConfirmed WHERE IDProduct = @productID
	SELECT @maxMonth = MAX(Month) FROM dbo.ProductConfirmed WHERE IDProduct = @productID AND Year = @maxYear

	SELECT 	@return = Reliquat + [Current] + Month1 + Month2 + Month3 + Month4 + Month5 + Month6 + Month7
		+ Month8 + Month9 + Month10 + Month11
		FROM  dbo.ProductConfirmed
			WHERE  IDProduct = @productID AND Year = @maxYear AND Month = @maxMonth

	
	RETURN ISNULL(@return, 0)

END