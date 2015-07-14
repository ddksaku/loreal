
if  object_id('dbo.CalculateTotals') IS NOT NULL
BEGIN
	DROP PROCEDURE [dbo].[CalculateTotals] 
end


GO
/****** Object:  StoredProcedure [dbo].[CalculateTotals]    Script Date: 03/30/2010 15:49:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CalculateTotals] 
	-- Add the parameters for the stored procedure here
	@AnimationProduct UNIQUEIDENTIFIER,
	@IDDivision UNIQUEIDENTIFIER,
	@TotalCapacity INT OUTPUT,
	@TotalBDCQuantity INT OUTPUT,
	@TotalForecastQuantity INT OUTPUT,
	@TotalAllocation INT OUTPUT,
	@TotalAllocationUK INT OUTPUT,
	@TotalAllocationROI INT OUTPUT,
	@RrpUK float OUTPUT,
	@RrpROI float OUTPUT,
	@ListPriceUK float OUTPUT,
	@ListPriceROI float OUTPUT,
	@ProductRecieved INT OUTPUT,
	@ActiveAnimations INT OUTPUT
	
AS
BEGIN	
	--Set default values
	SELECT @TotalCapacity = 0
	SELECT @TotalBDCQuantity = 0
	SELECT @TotalForecastQuantity = 0
	SELECT @TotalAllocation = 0
	SELECT @TotalAllocationUK = 0
	SELECT @TotalAllocationROI = 0
	SELECT @RrpUK = 0
	SELECT @RrpROI = 0
	SELECT @ListPriceUK = 0
	SELECT @ListPriceROI = 0
	SELECT @ProductRecieved = 0
	SELECT @ActiveAnimations = 0
	
	-- call functions
	SELECT @TotalCapacity = dbo.calculate_TotalCapacity(@AnimationProduct)
	SELECT @TotalBDCQuantity = dbo.calculate_TotalBDCQuantity(@AnimationProduct)
	SELECT @TotalForecastQuantity = dbo.calculate_TotalForecastQuantity(@AnimationProduct)
	SELECT @TotalAllocation = dbo.calculate_TotalAllocation(@AnimationProduct)
	SELECT @TotalAllocationUK =  dbo.calculate_TotalAllocationUK(@AnimationProduct,@IDDivision) 
	SELECT @TotalAllocationROI = dbo.calculate_TotalAllocationROI(@AnimationProduct,@IDDivision)
	SELECT @RrpUK = dbo.calculate_RRP_UK(@AnimationProduct,@IDDivision)
	SELECT @RrpROI = dbo.calculate_RRP_ROI(@AnimationProduct,@IDDivision)
	SELECT @ListPriceUK = dbo.calculate_ListPriceUK(@AnimationProduct,@IDDivision)
	SELECT @ListPriceROI = dbo.calculate_ListPriceROI(@AnimationProduct,@IDDivision)
	SELECT @ProductRecieved = dbo.calculate_ProductReceived(@AnimationProduct)
	SELECT @ActiveAnimations = dbo.calculate_ActiveAnimations(@AnimationProduct)

END