IF OBJECT_ID (N'dbo.calculate_ProductReceived', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_ProductReceived;
GO

CREATE FUNCTION dbo.calculate_ProductReceived (@AnimationProduct uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	

	declare @ConfirmedMADMonth datetime
	declare @ProductID uniqueidentifier
	declare @return int

	select @ProductID = IDProduct FROM dbo.AnimationProduct WHERE ID = @AnimationProduct
	select @ConfirmedMADMonth  = ConfirmedMADMonth FROM dbo.AnimationProduct WHERE ID = @AnimationProduct		
	
		
	SELECT @return = ISNULL(SUM(Quantity),0)	
		FROM dbo.ProductReceived 	
		WHERE IDProduct = @ProductID
			AND @ConfirmedMADMonth <= DATEADD(month,0,CAST(CAST([Year] AS varchar) + '-' + CAST([Month] AS varchar) + '-1' AS DATETIME))
			and [Year] > 0 and [Month] > 0
	
	return isnull(@return,0)

END