IF OBJECT_ID (N'dbo.calculate_ActiveAnimations', N'FN') IS NOT NULL
    DROP FUNCTION dbo.calculate_ActiveAnimations;
GO

CREATE FUNCTION dbo.calculate_ActiveAnimations (@AnimationProductID uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN

--declare @AnimationProductID uniqueidentifier
--set @AnimationProductID = '25768935-59A8-4EA6-9506-B0AD5E74D9BF'

	declare @ActiveAnimations int
	DECLARE @ProductID UNIQUEIDENTIFIER
	DECLARE @SalesDriveID UNIQUEIDENTIFIER

	SELECT @ProductID = ap.IDProduct, @SalesDriveID = a.IDSalesDrive
	FROM dbo.AnimationProduct ap
	LEFT JOIN dbo.Animation a ON a.ID = ap.IDAnimation
	WHERE ap.ID = @AnimationProductID
	
	declare @t table
	(a uniqueidentifier, b uniqueidentifier)
	
	insert into @t
	SELECT distinct IDAnimation, IDProduct		
	FROM dbo.AnimationProduct ap
				LEFT JOIN dbo.Animation a ON a.ID = ap.IDAnimation
				WHERE ap.IDProduct = @ProductID AND a.IDSalesDrive = @SalesDriveID
				AND a.[Status] <> 5 AND a.[Status] <> 6
	
	select @ActiveAnimations = count(*) from @t
	

	return @ActiveAnimations
END