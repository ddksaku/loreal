USE [LorealOptimiseUnitTest]
GO
/****** Object:  UserDefinedFunction [dbo].[calculate_SumOfStoreAllocation]    Script Date: 07/15/2010 15:53:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[calculate_SumOfStoreAllocation] (@groupID uniqueidentifier, @animationProductDetailID uniqueidentifier)
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
	
	declare @output int
	set @output = 0
	
	declare @customers table (ID uniqueidentifier)
	
	DECLARE @countAll INT
	DECLARE @countNull INT
	
	insert into @customers
		select ID FROM dbo.Customer
			where IDCustomerGroup = @groupID AND IncludeInSystem = 1 AND Deleted = 0
			
	SELECT @countAll = COUNT(*) FROM dbo.CustomerAllocation WHERE IDAnimationProductDetail = @animationProductDetailID 
				AND IDCustomer IN (SELECT ID FROM @customers)
	SELECT @countNull = COUNT(*) FROM dbo.CustomerAllocation WHERE IDAnimationProductDetail = @animationProductDetailID 
				AND IDCustomer IN (SELECT ID FROM @customers) AND FixedAllocation IS null
			
	IF @countAll = @countNull
		SELECT @output = NULL
	ELSE			
		SELECT @output = SUM(ISNULL(CalculatedAllocation,0))
			FROM dbo.CustomerAllocation
				WHERE IDAnimationProductDetail = @animationProductDetailID 
					AND IDCustomer IN (SELECT ID FROM @customers)
	
	
	RETURN isnull(@output,0)

END