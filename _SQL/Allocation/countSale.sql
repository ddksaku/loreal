--use LorealOptimise
go

IF OBJECT_ID (N'countSale') IS NOT NULL
    DROP function countSale ;
GO

create function countSale 
	(@customerId uniqueidentifier, 
	 @customerGroupId uniqueidentifier, 
	 @brandAxeId uniqueidentifier,
	 @saleAreaAnimationId uniqueidentifier,
	 @animationProductDetailId uniqueidentifier,
	 @signatureId uniqueidentifier )
RETURNS float
as
begin
	DECLARE @return float
	SET @return = 0
	
	DECLARE @customerGroupRetailUplift float
	DECLARE @customerGroupSalesArea uniqueidentifier
	DECLARE @customerRetailUplift float
	DECLARE @salesAreaId uniqueidentifier
	DECLARE @retailMultiplier float
		SELECT @retailMultiplier = RetailMultiplier FROM SalesArea WHERE ID = @salesAreaId
		

	-- CUSTOMER
	IF @customerId IS NOT NULL
	BEGIN		
		
		-- get retail uplift
		SELECT @customerRetailUplift = RetailUplift FROM CustomerAllocation
			 WHERE IDCustomer = @customerId AND IDAnimationProductDetail = @animationProductDetailId

		-- if this is empty, check customer group allocation retail uplift
		IF @customerRetailUplift IS NULL
		BEGIN
			DECLARE @groupId uniqueidentifier
			SELECT @groupId = IDCustomerGroup FROM Customer WHERE ID = @customerId

			SELECT @customerRetailUplift = RetailUplift FROM dbo.CustomerGroupAllocation
				WHERE IDCustomerGroup = @groupId AND IDAnimationProductDetail = @animationProductDetailId

		END

		-- get customer group sales area
		SELECT @customerGroupSalesArea = IDSalesArea FROM dbo.CustomerGroup WHERE ID =
		(SELECT IDCustomerGroup FROM dbo.Customer WHERE ID = @customerId)

		-- get salesArea
		SELECT @salesAreaId = ISNULL(IDSalesArea_AllocationSalesArea, @customerGroupSalesArea) FROM Customer
			WHERE ID  = @customerId 

		-- get retail multiplier
		SELECT @retailMultiplier = RetailMultiplier FROM SalesArea WHERE ID = @salesAreaId

		
		-- count sum of sales
		IF @brandAxeId IS NOT NULL
		BEGIN
			select @return = SUM(ISNULL(@customerRetailUplift,1) * (ISNULL(ManualValue, ISNULL(EPOSValue,ISNULL(CaCatValue*ISNULL(@retailMultiplier,1),0)))))
				FROM Sale 
					WHERE Date Between DATEADD(year, -1, getdate())  and getdate()  -- sale date within 12 months
						  AND IDCustomer = @customerId
						  AND IDBrandAxe = @brandAxeId	
		END
		ELSE IF @signatureId IS NOT NULL
		BEGIN
			-- table with brands
			declare @brandsFotSalesTable TABLE (brandAxeId uniqueidentifier)
			
			INSERT INTO @brandsFotSalesTable
				SELECT ID
					FROM  dbo.BrandAxe WHERE IDSignature = @signatureId AND Brand = 1

			select @return = SUM(ISNULL(@customerRetailUplift,1) * (ISNULL(ManualValue, ISNULL(EPOSValue,ISNULL(CaCatValue*ISNULL(@retailMultiplier,1),0)))))
				FROM Sale 
					WHERE Date Between DATEADD(year, -1, getdate())  and getdate()  -- sale date within 12 months
						  AND IDCustomer = @customerId
						  AND IDBrandAxe IN (SELECT brandAxeId FROM  @brandsFotSalesTable) 						

		END
		ELSE IF @brandAxeId IS NULL
		BEGIN
			select @return = SUM(ISNULL(@customerRetailUplift,1) * (ISNULL(ManualValue, ISNULL(EPOSValue,ISNULL(CaCatValue*ISNULL(@retailMultiplier,1),0)))))
				FROM Sale 
					WHERE Date Between DATEADD(year, -1, getdate())  and getdate()  -- sale date within 12 months
						  AND IDCustomer = @customerId
		END
		
	

	END
	-- CUSTOMER GROUP
	ELSE IF @customerGroupId IS NOT NULL
	BEGIN
		DECLARE @temp float
		SET @temp = 0
	
		-- get retail uplift
		SELECT @customerGroupRetailUplift = RetailUplift FROM CustomerGroupAllocation WHERE IDCustomerGroup = @customerGroupId
		
		-- get sales area
		SELECT @salesAreaId = IDSalesArea FROM CustomerGroup WHERE ID = @customerGroupId

		
	
		DECLARE @custID uniqueidentifier
		
		-- create cursor
		DECLARE customers CURSOR
		FOR SELECT ID FROM Customer WHERE IDCustomerGroup = @customerGroupId
		OPEN customers
		FETCH NEXT FROM customers into @custID

		DECLARE @customerSalesArea uniqueidentifier

		WHILE @@FETCH_STATUS = 0
		BEGIN
			
			-- get customer group sales area
			SELECT @customerGroupSalesArea = IDSalesArea FROM dbo.CustomerGroup WHERE ID =
			(SELECT IDCustomerGroup FROM dbo.Customer WHERE ID = @custID)

			SELECT @customerSalesArea = ISNULL(IDSalesArea_AllocationSalesArea, @customerGroupSalesArea) FROM Customer WHERE ID = @custID

			-- just customers within sales area set in AnimationProductDetail
			IF @customerSalesArea = @saleAreaAnimationId
			BEGIN
				
				-- get customer retail multiplier
		         SELECT @retailMultiplier = RetailMultiplier FROM SalesArea WHERE ID = @customerSalesArea		
				
				-- get customer retail uplift
				SELECT @customerRetailUplift = ISNULL(RetailUplift, 1) FROM dbo.CustomerAllocation WHERE IDCustomer = @custID AND IDAnimationProductDetail = @animationProductDetailId
				
				IF @brandAxeId IS NOT NULL
				BEGIN
					-- count sum of sales
					select @temp =  SUM(ISNULL(@customerRetailUplift,1) * (ISNULL(ManualValue, ISNULL(EPOSValue,ISNULL(CaCatValue*ISNULL(@retailMultiplier,1),0)))))
						FROM Sale 
							WHERE Date Between DATEADD(year, -1, getdate())  and getdate()  
								  AND IDCustomer = @custID
								  AND IDBrandAxe = @brandAxeId	
				END
				ELSE IF @signatureID IS NOT NULL
				BEGIN
					-- table with brands
					declare @brandsFotSalesTableCG TABLE (brandAxeId uniqueidentifier)
					
					INSERT INTO @brandsFotSalesTableCG
						SELECT ID
							FROM  dbo.BrandAxe WHERE IDSignature = @signatureId AND Brand = 1
					
					-- count sum of sales
					select @temp =  SUM(ISNULL(@customerRetailUplift,1) * (ISNULL(ManualValue, ISNULL(EPOSValue,ISNULL(CaCatValue*ISNULL(@retailMultiplier,1),0)))))
						FROM Sale 
							WHERE Date Between DATEADD(year, -1, getdate())  and getdate()  
								  AND IDCustomer = @custID
								  AND IDBrandAxe IN (SELECT brandAxeId FROM  @brandsFotSalesTableCG) 
					
				END
				ELSE
				BEGIN
					select @temp = SUM(ISNULL(@customerRetailUplift,1) * (ISNULL(ManualValue, ISNULL(EPOSValue,ISNULL(CaCatValue*ISNULL(@retailMultiplier,1),0)))))
					FROM Sale 
					WHERE Date Between DATEADD(year, -1, getdate())  and getdate()  -- sale date within 12 months
						  AND IDCustomer = @custID
				END
				
				SET @return = @return + ISNULL(@temp,0)

			END
			FETCH NEXT FROM customers into @custID			
		END

		-- multiple by customer group retail uplift
		SET @return = @return * ISNULL(@customerGroupRetailUplift, 1)
		
		CLOSE customers
		DEALLOCATE customers		

	END	

RETURN @return
end;