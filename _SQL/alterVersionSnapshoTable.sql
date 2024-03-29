alter table dbo.VersionSnapshot 
alter column Animation_DefaultCustomerReference nvarchar(20)

alter table dbo.VersionSnapshot
alter column AnimationProduct_LogisticsComments nvarchar(2000)

alter table dbo.VersionSnapshot
alter column AnimationProductDetail_MarketingComments nvarchar(2000)

alter table dbo.VersionSnapshot
alter column Division_Name nvarchar(50)

alter table dbo.VersionSnapshot
alter column CustomerGroup_Code nvarchar(10)
go 

ALTER FUNCTION [dbo].[calculate_RRP_UK] (@AnimationProductID uniqueidentifier, @IDDivision uniqueidentifier)
RETURNS FLOAT
AS
BEGIN
	declare @RrpUK float

	SELECT @RrpUK = SUM(ISNULL(apd.RRP,0))
	FROM AnimationProductDetail apd
	LEFT JOIN AnimationProduct ap ON ap.ID = apd.IDAnimationProduct
	LEFT JOIN SalesArea sa ON sa.ID = apd.IDSalesArea
	LEFT JOIN DistributionChannel dc ON dc.ID = sa.IDDistributionChannel
	LEFT JOIN SalesOrganization so ON so.ID = sa.IDSalesOrganization
	WHERE ap.ID = @AnimationProductID AND dc.Code = '02' AND so.Code = '0300'	AND sa.IDDivision = @IDDivision	
	
	return ISNULL(@RrpUK, 0)
END
GO


ALTER FUNCTION [dbo].[calculate_RRP_ROI] (@AnimationProductID uniqueidentifier, @IDDivision uniqueidentifier)
RETURNS float
AS
BEGIN
	declare @RrpROI float

	SELECT @RrpROI = SUM(ISNULL(apd.RRP,0))
	FROM AnimationProductDetail apd
	LEFT JOIN AnimationProduct ap ON ap.ID = apd.IDAnimationProduct
	LEFT JOIN SalesArea sa ON sa.ID = apd.IDSalesArea
	LEFT JOIN DistributionChannel dc ON dc.ID = sa.IDDistributionChannel
	LEFT JOIN SalesOrganization so ON so.ID = sa.IDSalesOrganization
	WHERE ap.ID = @AnimationProductID AND dc.Code = '02' AND so.Code = '0350'	AND sa.IDDivision = @IDDivision		
	
	return ISNULL(@RrpROI, 0)
END
GO