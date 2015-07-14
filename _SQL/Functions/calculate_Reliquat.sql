IF OBJECT_ID (N'dbo.calculate_Reliquat', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_Reliquat;
GO

CREATE FUNCTION dbo.calculate_Reliquat (@productID uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	declare @maxYear int
	declare @maxMonth int
	DECLARE @return int

	SELECT @maxYear = MAX(Year) FROM dbo.ProductConfirmed WHERE IDProduct = @productID
	SELECT @maxMonth = MAX(Month) FROM dbo.ProductConfirmed WHERE IDProduct = @productID AND Year = @maxYear

	SELECT 	@return = Reliquat 
		FROM  dbo.ProductConfirmed
			WHERE  IDProduct = @productID AND Year = @maxYear AND Month = @maxMonth

	
	RETURN ISNULL(@return, 0)

END