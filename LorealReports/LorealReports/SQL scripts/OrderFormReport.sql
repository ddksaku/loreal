CREATE PROCEDURE [dbo].[rep_OrderFormReport] 
							@salesDriveId VARCHAR(MAX),
							@animations NVARCHAR(max),
							@salesArea NVARCHAR(MAX),
							@itemGroupId NVARCHAR(MAX)

AS

	SET NOCOUNT ON

	SELECT	IG.Name [ITEM GROUP NAME],
			P.MaterialCode [MATERIAL CODE], 
			P.[Description] [PRODUCT DESCRIPTION], 
			P.EAN [EAN BARCODE], 
			M.Value [MULTIPLE], 
			SA.CurrencySymbol + ' ' + CAST(APD.RRP as VARCHAR) [RRP], 
			SA.CurrencySymbol + ' ' + CAST(APD.RRP * SA.RRPToListPriceRate as VARCHAR) as [LIST PRICE], 
			A.OnCounterDate,
			SA.CurrencySymbol,
			AP.IDItemGroup,
			AP.IDAnimation,
			A.Name AnimationName,
			A.SAPDespatchCode PromotionCode,
			A.PLVDeliveryDate DeliveryDate,
			'Comment' FooterComment,
			GETDATE() RunDateTime
	FROM AnimationProductDetail APD
	JOIN AnimationProduct AP
	ON APD.IDAnimationProduct = AP.ID
	JOIN Product P
	ON AP.IDProduct = P.ID
	LEFT JOIN Multiple M
	ON AP.IDMultipleNormal = M.ID
	JOIN SalesArea SA
	ON APD.IDSalesArea = SA.ID
	JOIN ItemGroup IG
	ON AP.IDItemGroup = IG.ID
	JOIN Animation A
	ON AP.IDAnimation = A.ID
	WHERE A.IDSalesDrive = COALESCE(@salesDriveId, A.IDSalesDrive)
	AND APD.IDSalesArea = COALESCE(@salesArea, APD.IDSalesArea)
	AND AP.IDItemGroup in (SELECT value FROM [dbo].[uf_split](@itemGroupId,','))
	AND ((@animations IS NULL) OR (AP.IDAnimation in (SELECT value FROM [dbo].[uf_split](@animations,',')))) 

 
-- exec dbo.rep_OrderFormReport @salesDriveId=NULL,@animations=NULL,@salesArea=N'aa2cd9f7-4d19-4413-9af8-e2b05cfe7ca8',@itemGroupId=N'6317f458-524f-469c-827f-02b3bbf4ef10,83ae4939-4b09-4091-a5f2-09c65b7f0cd8,2ab93cfc-8c70-4bdd-9e94-15827f86ea57,49e8c5e1-5710-42c8-817f-164a67f401ca,107766f2-0008-4dc3-b486-18a235372929,96de8e29-b1fb-4174-88ae-1cf2ac15d96d,9674f6a2-7677-47f3-a5ea-2744cafa967e,6db983bd-7618-4d8a-a5ae-39c08f398850,736d4f49-0108-4d69-a624-482c12d4c69e,5fc33f23-5e58-4193-99b3-49383c3c6b94,703f716d-fb22-492c-bffc-49748ee80baa,481f5c39-5232-4c94-9560-51e03f8a864e,dd1240df-0716-49f4-ae79-664d1d2510bc,5830a7f5-b6d4-4c94-b120-7bcb0194a38b,39dc7465-1cc3-46c2-841b-7d676a2c7d50,d634a94f-891d-4755-bad9-8bf7014757e4,d49a8210-61e2-42ad-b0d6-90d3039f18aa,0002e338-eac6-4efc-9803-9211daf6c931,edf73dcb-23dc-4bdf-9cfb-9ebca4a9626a,52ef3956-a57a-487d-a0e8-a7af16ef2db1,65e03634-f92f-4a64-80f8-b46b5ee0258e,e619c1d0-d4cc-40ec-8514-b5be26f1c875,62ec7084-3a55-4ff4-9b2e-c7dca973699f,fe38a244-79c1-4416-bf47-c9c412c9f840,6a9f2ac1-82e5-49da-8a9a-da43c53f2970,614a784a-7eda-4805-b1f9-deebfb9db73a,5f4c2662-aced-46eb-95cd-edb7fb506533'


