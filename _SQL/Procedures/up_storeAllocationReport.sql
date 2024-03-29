
/****** Object:  StoredProcedure [dbo].[up_storeAllocationReport]    Script Date: 04/26/2010 15:03:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[up_storeAllocationReport] 
 
 @allocationVersion NVARCHAR(max)
,@employee NVARCHAR(max)
,@customerGroup NVARCHAR(max)


AS



IF @customerGroup = 'All groups'
BEGIN
	SELECT b.Name AS VersionName, Customer_AccountNumber, Customer_SalesEmployeeName, Animation_Name AS AnimationName, ItemGroup_Name AS ItemGroup, Product_Description AS ProductDescription, Product_MaterialCode AS MaterialCode, Customer_CalculatedAllocation AS AllocationQuantity, Customer_Name
		FROM dbo.VersionSnapshot AS a INNER JOIN dbo.Version AS b ON (a.IDVersion = b.ID)
			WHERE  Name COLLATE DATABASE_DEFAULT IN (SELECT VALUE FROM dbo.uf_split(@allocationVersion,','))
			-- divide employee parametres and add into temp table
			AND Customer_SalesEmployeeName COLLATE DATABASE_DEFAULT
				in (SELECT VALUE FROM dbo.uf_split(@employee,','))	
END
ELSE 
BEGIN
	SELECT b.Name AS VersionName, Customer_AccountNumber, Customer_SalesEmployeeName,  Animation_Name AS AnimationName, ItemGroup_Name AS ItemGroup, Product_Description AS ProductDescription, Product_MaterialCode AS MaterialCode, Customer_CalculatedAllocation AS AllocationQuantity, Customer_Name
		FROM dbo.VersionSnapshot AS a INNER JOIN dbo.Version AS b ON (a.IDVersion = b.ID)
			WHERE  Name COLLATE DATABASE_DEFAULT IN (SELECT VALUE FROM dbo.uf_split(@allocationVersion,','))
			-- divide employee parametres and add into temp table
			AND CustomerGroup_Name = @customerGroup
			AND Customer_SalesEmployeeName  COLLATE DATABASE_DEFAULT
				in (SELECT VALUE FROM dbo.uf_split(@employee,','))	

END
		
			
			


