IF OBJECT_ID (N'dbo.calculate_MonthDelivery', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_MonthDelivery;
GO

CREATE FUNCTION dbo.calculate_MonthDelivery (@productID uniqueidentifier, @monthNumber int)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	declare @maxYear int
	declare @maxMonth int
	DECLARE @return int
	declare @sqlQuery nvarchar(max)

	SELECT @maxYear = MAX(Year) FROM dbo.ProductConfirmed WHERE IDProduct = @productID
	SELECT @maxMonth = MAX(Month) FROM dbo.ProductConfirmed WHERE IDProduct = @productID AND Year = @maxYear

	IF @monthNumber = '0'
	BEGIN
		SELECT @return = [Current] FROM dbo.ProductConfirmed
				WHERE  IDProduct = @productID AND Year = @maxYear AND Month = @maxMonth
	END
	ELSE
	BEGIN		
		SELECT @return = 
			CASE @monthNumber
				WHEN 1 THEN Month1
				WHEN 2 THEN Month2
				WHEN 3 THEN Month3
				WHEN 4 THEN Month4
				WHEN 5 THEN Month5
				WHEN 6 THEN Month6
				WHEN 7 THEN Month7
				WHEN 8 THEN Month8
				WHEN 9 THEN Month9
				WHEN 10 THEN Month10
				WHEN 11 THEN Month11
			END
			 FROM dbo.ProductConfirmed
				WHERE IDProduct = @productID AND Year = @maxYear AND Month = @maxMonth		
	END

	
	RETURN ISNULL(@return, 0)

END