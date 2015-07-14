IF OBJECT_ID ('tr_CustomerAllocation_CheckAgainstGroupFixed', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerAllocation_CheckAgainstGroupFixed;
GO

CREATE TRIGGER tr_CustomerAllocation_CheckAgainstGroupFixed
ON CustomerAllocation
AFTER UPDATE, INSERT 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @fixedAllocation int
	SELECT @fixedAllocation = FixedAllocation FROM inserted

	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)

	IF @fixedAllocation IS NOT NULL
	BEGIN
			
		
		DECLARE @animationProductDetailId uniqueidentifier
		DECLARE @animationProductId uniqueidentifier
		DECLARE @animationId uniqueidentifier
		DECLARE @multipleID uniqueidentifier
		DECLARE @multiple int
		DECLARE @errorMsg nvarchar(max)
		DECLARE @groupID uniqueidentifier
		DECLARE @customerID uniqueidentifier
		DECLARE @customerGroupAllocationID uniqueidentifier
		DECLARE @manualFixed int
		DECLARE @systemFixed int

		SELECT @customerID = IDCustomer FROM inserted
		SELECT @animationProductDetailId = IDAnimationProductDetail FROM inserted
		SELECT @animationProductId = IDAnimationProduct FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId
		SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId
		SELECT @multipleID = IDMultipleNormal FROM dbo.AnimationProduct WHERE ID = @animationProductId
		SELECT @multiple = Value FROM dbo.Multiple WHERE ID = @multipleID
		SELECT @groupID  = IDCustomerGroup FROM dbo.Customer WHERE ID = @customerID
		SELECT @customerGroupAllocationID = ID FROM dbo.CustomerGroupAllocation WHERE IDCustomerGroup = @groupID AND IDAnimationProductDetail = @animationProductDetailId
	
		SELECT @manualFixed = ManualFixedAllocation, @systemFixed = SystemFixedAllocation FROM dbo.CustomerGroupAllocation WHERE ID = @customerGroupAllocationID

		IF 	@manualFixed > 0
		BEGIN
			IF @fixedAllocation > @manualFixed
			BEGIN
				/*SET @errorMsg = 'Fixed allocation exceeds customer group manual fixed allocation' + @newLineChar
				 + '  Fixed: ' + cast(@fixedAllocation as nvarchar(50)) + @newLineChar
				 + '  Manual Fixed CG: ' + cast(@manualFixed as nvarchar(50))*/
				SET @errorMsg = dbo.uf_getSystemMessage('tr_CustomerAllocation_CheckAgainstGroupFixed01', cast(@fixedAllocation as nvarchar(50)), cast(@manualFixed as nvarchar(50)), null, null, null, null, null, null)
				RAISERROR (@errorMsg, 16, 9)
				ROLLBACK TRAN
				RETURN	
			END
		END
		ELSE IF @systemFixed > 0
		BEGIN
			IF @fixedAllocation > @systemFixed 
			BEGIN
				/*SET @errorMsg = 'Fixed allocation exceeds customer group system fixed allocation' + @newLineChar
				 + '  Fixed: ' + cast(@fixedAllocation as nvarchar(50)) + @newLineChar
				 + '  System Fixed CG: ' + cast(@systemFixed as nvarchar(50))*/
				SET @errorMsg = dbo.uf_getSystemMessage('tr_CustomerAllocation_CheckAgainstGroupFixed02', cast(@fixedAllocation as nvarchar(50)), cast(@systemFixed as nvarchar(50)), null, null, null, null, null, null)
				RAISERROR (@errorMsg, 16, 9)
				ROLLBACK TRAN
				RETURN					
			END
		END			

	END
	
	RETURN
END