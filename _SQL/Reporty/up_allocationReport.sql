IF OBJECT_ID('up_allocationReport') > 0
	DROP PROCEDURE up_allocationReport
go

CREATE PROCEDURE up_allocationReport 
	@showCustomerGroups BIT
   ,@allocationVersion NVARCHAR(255)
   ,@customerGroup NVARCHAR(255)   
AS 	

DECLARE @groupID UNIQUEIDENTIFIER
SELECT @groupID = ID FROM dbo.CustomerGroup WHERE Name = @customerGroup

DECLARE @versionID UNIQUEIDENTIFIER
SELECT @versionID = ID FROM Version WHERE Name = @allocationVersion

IF @showCustomerGroups = 0
BEGIN


	DECLARE @t TABLE
		(
			 customer NVARCHAR(255)
			,customerAccount NVARCHAR(255) 
			,customerGroup NVARCHAR(255) 
			,materialCode NVARCHAR(255)
			,itemGroup NVARCHAR(255)
			,calculatedAllocation INT
			,productDescription NVARCHAR(255)
			,onCounterDate DATETIME
			,saleOrg NVARCHAR(255)
		)
		
	IF	@customerGroup = 'All groups'
	BEGIN
		INSERT INTO @t (saleOrg, customer, customerAccount, customerGroup, materialCode, itemGroup, calculatedAllocation, productDescription, onCounterDate)
			SELECT SalesArea_SalesOrganizationName, Customer_Name, Customer_AccountNumber, CustomerGroup_Name, Product_MaterialCode, ItemGroup_Name, Customer_CalculatedAllocation, Product_Description, onCounterDate
				FROM dbo.VersionSnapshot 
					WHERE  IDVersion = @versionID
	END
	ELSE
	BEGIN
		INSERT INTO @t (saleOrg, customer, customerAccount, customerGroup, materialCode, itemGroup, calculatedAllocation, productDescription, onCounterDate)
				SELECT SalesArea_SalesOrganizationName, Customer_Name, Customer_AccountNumber, CustomerGroup_Name, Product_MaterialCode, ItemGroup_Name, Customer_CalculatedAllocation, Product_Description, onCounterDate
					FROM dbo.VersionSnapshot 
						WHERE CustomerGroup_ID = @groupID AND IDVersion = @versionID
	END
	
	
	
	SELECT * FROM @t

END
	