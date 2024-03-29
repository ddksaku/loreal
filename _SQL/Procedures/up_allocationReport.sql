
/****** Object:  StoredProcedure [dbo].[up_allocationReport]    Script Date: 04/26/2010 15:24:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[up_allocationReport] 
	@showCustomerGroups BIT
   ,@allocationVersion NVARCHAR(max)
   ,@customerGroup NVARCHAR(255)   
   ,@salesEmployee NVARCHAR(max)
AS 	

DECLARE @groupID UNIQUEIDENTIFIER
SELECT @groupID = ID FROM dbo.CustomerGroup WHERE Name = @customerGroup

--DECLARE @versionID UNIQUEIDENTIFIER
--SELECT @versionID = ID FROM Version WHERE Name = @allocationVersion

IF @showCustomerGroups = 0
BEGIN


	DECLARE @t TABLE
		(
			 customer NVARCHAR(255)
			,customerAccount NVARCHAR(255) 
			,customerGroup NVARCHAR(255) 
			,materialCode NVARCHAR(255)
			,internationalCode NVARCHAR(255)
			,itemGroup NVARCHAR(255)
			,calculatedAllocation INT
			,productDescription NVARCHAR(255)
			,onCounterDate DATETIME
			,saleOrg NVARCHAR(255)
			,multiple INT
			,EAN NVARCHAR(50)
			,totalAllocationQuantity INT
			,versionName NVARCHAR(255)
			,rrpROI FLOAT
			,rrpUK FLOAT
			,listPriceUK FLOAT
			,listPriceROI FLOAT
			,animationName NVARCHAR(255)
			,animationCode NVARCHAR(50)
			,CustomerGroup_RetailUplift FLOAT
			,CustomerGroup_ManualFixed FLOAT
			,Customer_RetailUplift FLOAT
		)
		
	IF	@customerGroup = 'All groups'
	BEGIN
		INSERT INTO @t (versionName, saleOrg, customer, customerAccount, customerGroup, materialCode, internationalCode, 
				itemGroup, calculatedAllocation, productDescription, onCounterDate, multiple, EAN, totalAllocationQuantity
				,rrpROI, rrpUK, listPriceUK, listPriceROI, animationName, animationCode, CustomerGroup_RetailUplift, CustomerGroup_ManualFixed, Customer_RetailUplift)
			SELECT v.Name, SalesArea_SalesOrganizationName, Customer_Name, Customer_AccountNumber,  
				CASE WHEN b.ShowRBMInReporting = 1 THEN Customer_SalesEmployeeName ELSE  CustomerGroup_Name end, 
				Product_MaterialCode, Product_InternationalCode, ItemGroup_Name, Customer_CalculatedAllocation, Product_Description, 
				onCounterDate, MultipleNormal, Product_EAN, TotalAllocation, rrpROI, rrpUK, ListPriceUK, ListPriceROI
				,Animation_Name, Animation_Code, CustomerGroup_RetailUplift, CustomerGroup_ManualFixed, Customer_RetailUplift
				FROM dbo.VersionSnapshot AS a INNER JOIN dbo.CustomerGroup AS b ON (a.CustomerGroup_ID = b.ID)
					INNER JOIN Version AS v ON (a.IDVersion = v.ID)
					WHERE (SELECT Name FROM dbo.Version WHERE ID = a.IDVersion) COLLATE DATABASE_DEFAULT IN
						(SELECT VALUE FROM dbo.uf_split(@allocationVersion,',')) 
						AND Customer_SalesEmployeeName COLLATE DATABASE_DEFAULT
							in (SELECT VALUE FROM dbo.uf_split(@salesEmployee,','))	
					--WHERE  IDVersion = @versionID
	END
	ELSE
	BEGIN
		INSERT INTO @t (versionName, saleOrg, customer, customerAccount, customerGroup, materialCode, internationalCode, 
			itemGroup, calculatedAllocation, productDescription, onCounterDate, multiple, EAN, totalAllocationQuantity
			,rrpROI, rrpUK, listPriceUK, listPriceROI, animationName, animationCode, CustomerGroup_RetailUplift, CustomerGroup_ManualFixed, Customer_RetailUplift)
				SELECT v.Name, SalesArea_SalesOrganizationName, Customer_Name, Customer_AccountNumber,
				CASE WHEN b.ShowRBMInReporting = 1 THEN Customer_SalesEmployeeName ELSE  CustomerGroup_Name end, 
				 Product_MaterialCode, Product_InternationalCode, ItemGroup_Name, Customer_CalculatedAllocation, Product_Description,
				  onCounterDate, MultipleNormal, Product_EAN, TotalAllocation, rrpROI, rrpUK, ListPriceUK, ListPriceROI
				  ,Animation_Name, Animation_Code, CustomerGroup_RetailUplift, CustomerGroup_ManualFixed, Customer_RetailUplift
					FROM dbo.VersionSnapshot AS a INNER JOIN dbo.CustomerGroup AS b ON (a.CustomerGroup_ID = b.ID)
					INNER JOIN Version AS v ON (a.IDVersion = v.ID)
					WHERE (SELECT Name FROM dbo.Version WHERE ID = a.IDVersion) COLLATE DATABASE_DEFAULT IN
						(SELECT VALUE FROM dbo.uf_split(@allocationVersion,','))	
						--WHERE IDVersion = @versionID 
						AND CustomerGroup_ID = @groupID 
						AND Customer_SalesEmployeeName COLLATE DATABASE_DEFAULT
							in (SELECT VALUE FROM dbo.uf_split(@salesEmployee,','))	
	END
	
		
	SELECT * FROM @t

END
	