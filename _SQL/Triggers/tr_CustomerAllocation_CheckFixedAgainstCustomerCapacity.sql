IF OBJECT_ID ('tr_CustomerAllocation_CheckFixedAgainstCustomerCapacity', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerAllocation_CheckFixedAgainstCustomerCapacity;
GO

CREATE TRIGGER tr_CustomerAllocation_CheckFixedAgainstCustomerCapacity
ON CustomerAllocation
AFTER UPDATE, INSERT 
AS 
BEGIN

	IF @@ROWCOUNT = 0 RETURN 
	
	DECLARE @customeAllocationID uniqueidentifier
	select @customeAllocationID = ID from inserted

	DECLARE @fixedAllocation int
	SELECT @fixedAllocation = FixedAllocation FROM inserted

	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)
	
	DECLARE @customer uniqueidentifier
	SELECT @customer = IDCustomer FROM inserted

	IF @fixedAllocation IS NOT NULL
	BEGIN
		
		DECLARE @animationProductDetailId uniqueidentifier
		DECLARE @animationProductId uniqueidentifier
		DECLARE @animationId uniqueidentifier	
		DECLARE @itemTypeID uniqueidentifier		
		DECLARE @errorMsg nvarchar(max)
		DECLARE @capacityWithinAllStores int
		DECLARE @sumOfFixed int		
		DECLARE @animationTypeID uniqueidentifier	
		DECLARE @priorityID uniqueidentifier

		
		SELECT @animationProductDetailId = IDAnimationProductDetail FROM inserted
		SELECT @animationProductId = IDAnimationProduct FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId
		SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId			
		SELECT @animationTypeID = IDAnimationType FROM dbo.Animation WHERE ID = @animationID
		SELECT @priorityID = IDPriority FROM dbo.Animation WHERE ID = @animationID
		SELECT @itemTypeID = IDItemType FROM dbo.AnimationProduct WHERE ID = @animationProductId
		

		declare @capacity int
				
		-- update their capacities
		/*SELECT @capacity = Capacity
			FROM dbo.CustomerCapacity WHERE IDCustomer = @customer AND IDAnimationType = @animationTypeID AND IDPriority = @priorityID
					AND IDItemType = @itemTypeId*/
			
		SELECT @capacity = dbo.calculate_TotalCapacityCustomer(@customeAllocationID)						
		select @capacity = ISNULL(@capacity, 0)	

		IF @capacity < @fixedAllocation
		BEGIN
			/*SET @errorMsg = 'Customer fixed allocation exceeds its capacity' + @newLineChar
			 + '  Capacity: ' + cast(@capacity as nvarchar(50)) + @newLineChar
			 + '  Fixed: ' + cast(@fixedAllocation as nvarchar(50)) */
			SET @errorMsg = dbo.uf_getSystemMessage('tr_CustomerAllocation_CheckFixedAgainstCustomerCapacity', cast(@capacity as nvarchar(50)), cast(@fixedAllocation as nvarchar(50)) , null, null, null, null, null, null)
			RAISERROR (@errorMsg, 16, 20)
			ROLLBACK TRAN
			RETURN	
		END

	END
	
	RETURN
END