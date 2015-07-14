IF OBJECT_ID (N'dbo.calculate_TotalCIV', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_TotalCIV;
GO

CREATE FUNCTION dbo.calculate_TotalCIV (@AnimationProductID uniqueidentifier)
RETURNS money
WITH SCHEMABINDING
AS
BEGIN
	declare @TotalQuantity int
	select @TotalQuantity = dbo.calculate_TotalAllocation(@AnimationProductID)

	declare @productID uniqueidentifier
	SELECT @productID = IDProduct FROM dbo.AnimationProduct WHERE ID = @AnimationProductID

	declare @civ money
	select @civ = CIV FROM dbo.Product WHERE ID = @productID 	

	RETURN @civ * ISNULL(@TotalQuantity, 0)

END