if object_id('tr_CustomerAllocation_CheckSumAgainstGroupFixed') > 0
	drop trigger tr_CustomerAllocation_CheckSumAgainstGroupFixed
go

CREATE TRIGGER [dbo].[tr_CustomerAllocation_CheckSumAgainstGroupFixed]
ON [dbo].[CustomerAllocation]
AFTER UPDATE, INSERT 
AS 
BEGIN	

	DECLARE @fixedAllocation int	
	SELECT @fixedAllocation = FixedAllocation FROM inserted
	--set @fixedAllocation = 10000

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
		--set @customerID = '32D41707-F381-4C67-8BA4-AE0B31137530'
		SELECT @animationProductDetailId = IDAnimationProductDetail FROM inserted
		--set @animationProductDetailId = 'D8553B5A-51D1-4ED0-BB57-816AC7B0B060'
		SELECT @animationProductId = IDAnimationProduct FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId
		SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId
		SELECT @multipleID = IDMultipleNormal FROM dbo.AnimationProduct WHERE ID = @animationProductId
		SELECT @multiple = Value FROM dbo.Multiple WHERE ID = @multipleID
		SELECT @groupID  = IDCustomerGroup FROM dbo.Customer WHERE ID = @customerID
		SELECT @customerGroupAllocationID = ID FROM dbo.CustomerGroupAllocation WHERE IDCustomerGroup = @groupID AND IDAnimationProductDetail = @animationProductDetailId
	
		SELECT @manualFixed = ManualFixedAllocation, @systemFixed = SystemFixedAllocation FROM dbo.CustomerGroupAllocation WHERE ID = @customerGroupAllocationID

		declare @customers table
		(customer uniqueidentifier, fixed int)

		
		insert into @customers
			select IDCustomer, FixedAllocation
				from dbo.CustomerAllocation a inner join Customer b on (a.IDCustomer = b.ID)
					where b.IDCustomerGroup = @groupID
						and IDAnimationProductDetail = @animationProductDetailId	


		-- sum of fixed within customers
		select @fixedAllocation = sum(isnull(fixed,0))
			from @customers

		
		IF 	@manualFixed > 0
		BEGIN
			IF @fixedAllocation > @manualFixed
			BEGIN
				SET @errorMsg = 'Sum of customer''s Fixed allocations exceeds customer group manual fixed allocation' + @newLineChar
				 + '  Sum of Fixed: ' + cast(@fixedAllocation as nvarchar(50)) + @newLineChar
				 + '  Manual Fixed CG: ' + cast(@manualFixed as nvarchar(50))
				RAISERROR (@errorMsg, 16, 22)
				ROLLBACK TRAN
				RETURN	
			END
		END
		ELSE IF @systemFixed > 0
		BEGIN
			IF @fixedAllocation > @systemFixed 
			BEGIN
				SET @errorMsg = 'Sum of customer''s Fixed allocations exceeds customer group system fixed allocation' + @newLineChar
				 + '  Sum of Fixed: ' + cast(@fixedAllocation as nvarchar(50)) + @newLineChar
				 + '  System Fixed CG: ' + cast(@manualFixed as nvarchar(50))
				RAISERROR (@errorMsg, 16, 21)
				ROLLBACK TRAN
				RETURN					
			END
		END			

	END
	
	RETURN
END