

-- *****************************************
-- ** ALLOCATION (AnimationId)
-- **
-- **
-- *****************************************

ALTER procedure [dbo].[uf_allocate_animationID] 
	(
	  @animationId uniqueidentifier
	 )
AS
BEGIN

	DECLARE @newLineChar char(2)
	DECLARE @output int
	DECLARE @zbytek int	
	DECLARE @logId uniqueidentifier
	DECLARE @logString nvarchar(max)
	DECLARE @itemTypeId uniqueidentifier
	DECLARE @animationProductId uniqueidentifier

	
	-- clear allocation calculation
	execute allocation_ClearAllCalculations 
		@animationId		 
		
		

	SET @logId = NEWID()
	SET @newLineChar = CHAR(13) + CHAR(10)	

	IF (SELECT ID FROM Animation WHERE ID = @animationId) IS NULL
	BEGIN
		SET @logString = 'Animation not found';
		EXEC [logFile]
				@logString,
				@logId
	END	
			
	DECLARE @animationProductDetails table
	( animationProductDetailId uniqueidentifier
	 ,quantity int )

	-- find animation Productt details
	INSERT INTO @animationProductDetails 
		SELECT ID, AllocationQuantity
			FROM AnimationProductDetail
				WHERE IDAnimationProduct 
				IN (select ID FROM AnimationProduct WHERE IDAnimation = @animationId)


	DECLARE @kurzorAnimationProductDetailId uniqueidentifier
	DECLARE @kurzorQunatity int
	

	DECLARE animationProductDetailCursor CURSOR
	FOR SELECT animationProductDetailId, quantity FROM @animationProductDetails -- WHERE quantity > 0
	OPEN animationProductDetailCursor
	FETCH NEXT FROM animationProductDetailCursor into @kurzorAnimationProductDetailId, @kurzorQunatity
		
	WHILE @@FETCH_STATUS = 0
	BEGIN		
		-- allocation
		execute uf_allocate_anPrDeID 
			@kurzorAnimationProductDetailId
		   ,@kurzorQunatity
		   ,@logId
		   ,0
		   ,@output OUTPUT
		   ,@zbytek OUTPUT

		-- mame zbytek k rozalokovani
		/*IF @zbytek > 0
		BEGIN
			-- alokace zbytku
			execute uf_allocate_anPrDeID 
				@kurzorAnimationProductDetailId
			   ,@zbytek
			   ,@logId
			   ,1
			   ,@output OUTPUT
			   ,@zbytek OUTPUT		
		END*/





		-- warehouse accounty
		DECLARE @warehouseGroups TABLE
		(
			groupId uniqueidentifier
		   ,fixedAllocation int
		   ,calculatedAllocation int
		)

		SELECT @animationProductID = IDAnimationProduct FROM dbo.AnimationProductDetail WHERE ID = @kurzorAnimationProductDetailId
		SELECT @itemTypeId = IDItemType FROM dbo.AnimationProduct WHERE ID = @animationProductID		

		INSERT INTO @warehouseGroups (groupId)
			SELECT IDCustomerGroup FROM dbo.AnimationCustomerGroup WHERE IDAnimation = @animationId		

		DELETE FROM @warehouseGroups WHERE groupId not in(
			SELECT IDCustomerGroup FROM dbo.CustomerGroupItemType WHERE IDItemType = @itemTypeId AND WarehouseAllocation = 1)

	

		-- dalsi animation product
		FETCH NEXT FROM animationProductDetailCursor into @kurzorAnimationProductDetailId, @kurzorQunatity
	END
	
	CLOSE animationProductDetailCursor
	DEALLOCATE animationProductDetailCursor


	-- update date calculated
	UPDATE dbo.Animation SET DateCalculated = getdate() WHERE ID = @animationId

	-- create snapshot
	--exec dbo.up_createSnapshot @animationId, NULL, 'Created after allocation calculated'
	insert into dbo.VersionSnapshotsToBeCreated (IDAnimation, DatePlanned, Processed, ModifiedBy)
		values(@animationId, getdate(), 0, 'Created after allocation calculated')
	
	
	-- calculate System Fixed Allocation
	exec up_calculateSystemFixedAllocation
			@animationID

END


