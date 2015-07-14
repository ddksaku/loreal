IF OBJECT_ID ('tr_AnimationProductDetail_AllocationQuantity', 'TR') IS NOT NULL
   DROP TRIGGER tr_AnimationProductDetail_AllocationQuantity;
GO

CREATE TRIGGER tr_AnimationProductDetail_AllocationQuantity
ON AnimationProductDetail
AFTER UPDATE 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @animationProductId uniqueidentifier
	DECLARE @animationId uniqueidentifier
	DECLARE @animationProductDetailID uniqueidentifier
	DECLARE @animationTypeID uniqueidentifier
	DECLARE @priorityID uniqueidentifier
	DECLARE @itemTypeID uniqueidentifier
	DECLARE @capacityWithinAllStores int
	DECLARE @allocationQuantity int
	DECLARE @errorMsg nvarchar(max)
	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)
	
	SELECT @animationProductDetailID = ID from inserted
	SELECT @allocationQuantity = AllocationQuantity FROM inserted
	SELECT @animationProductId = IDAnimationProduct FROM inserted
	SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId


	if @allocationQuantity > 0
	begin

		-- get customer groups 
		/*DECLARE @groups TABLE (groupID uniqueidentifier)

		INSERT INTO @groups 
			SELECT IDCustomerGroup FROM dbo.AnimationCustomerGroup WHERE IDAnimation = @animationID

		-- get customers and their capaties
		DECLARE @customers TABLE (customerID uniqueidentifier, capacity int)

		INSERT INTO @customers (customerID)
			SELECT ID FROM Customer
				WHERE IDCustomerGroup IN (SELECT groupID FROM @groups)

		-- update their capacities
		UPDATE @customers SET capacity = b.Capacity
			FROM @customers as a inner join dbo.CustomerCapacity as b 
				ON (a.customerID = b.IDCustomer AND b.IDAnimationType = @animationTypeID AND b.IDPriority = @priorityID
					AND b.IDItemType = @itemTypeId)
					
		-- sum of all stores capacities
		SELECT @capacityWithinAllStores = SUM(ISNULL(capacity,0)) FROM @customers	*/		
					
		select @capacityWithinAllStores = dbo.calculate_ProductDetailTotals(@animationProductDetailID)				
		
		set @capacityWithinAllStores = ISNULL(@capacityWithinAllStores, 0)

		IF @allocationQuantity > @capacityWithinAllStores
		BEGIN
			SET @errorMsg = dbo.uf_getSystemMessage('tr_AnimationProductDetail_AllocationQuantity', cast(@allocationQuantity as nvarchar(50)), cast(@capacityWithinAllStores as nvarchar(50)), null, null, null, null, null, null)
			RAISERROR (@errorMsg, 16, 1)
			ROLLBACK TRAN
			RETURN			
		END
		ELSE	
		RETURN
	end
END