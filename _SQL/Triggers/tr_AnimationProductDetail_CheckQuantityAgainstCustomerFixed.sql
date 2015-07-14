if object_id('tr_AnimationProductDetail_CheckQuantityAgainstCustomerFixed') > 0	
	drop trigger tr_AnimationProductDetail_CheckQuantityAgainstCustomerFixed
go


CREATE TRIGGER [dbo].tr_AnimationProductDetail_CheckQuantityAgainstCustomerFixed
ON [dbo].[AnimationProductDetail]
AFTER UPDATE 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 
	
	SET NOCOUNT ON

	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)
	DECLARE @errorMsg nvarchar(max)

	DECLARE @animationProductId uniqueidentifier
	DECLARE @fixedWithinCustomers int
	DECLARE @allocationQuantity int		
	DECLARE @animationProductDetailID uniqueidentifier


	SELECT @animationProductDetailID = ID FROM inserted
	SELECT @allocationQuantity = AllocationQuantity FROM inserted
	SELECT @animationProductId = IDAnimationProduct FROM inserted

	
	-- get customer groups 
	DECLARE @groups TABLE (groupID uniqueidentifier)

	INSERT INTO @groups 
		SELECT IDCustomerGroup FROM dbo.CustomerGroupAllocation WHERE IDAnimationProductDetail = @animationProductDetailID

	
	-- get customers and their fixed
	DECLARE @customers TABLE (customerID uniqueidentifier, fixed int)

	INSERT INTO @customers (customerID)
		SELECT ID FROM Customer
			WHERE IDCustomerGroup IN (SELECT groupID FROM @groups)

	-- update their fixed allocations
		UPDATE @customers SET fixed = b.FixedAllocation
			FROM @customers as a inner join dbo.CustomerAllocation as b 
				ON (a.customerID = b.IDCustomer AND b.IDAnimationProductDetail = @animationProductDetailId)

	
	-- sum of all stores gfixed
	SELECT @fixedWithinCustomers = SUM(ISNULL(fixed,0)) FROM @customers
	
	set @fixedWithinCustomers = isnull(@fixedWithinCustomers, 0)
	
	IF @fixedWithinCustomers > @allocationQuantity
	BEGIN
		/*SET @errorMsg = 'Sum of customer fixed allocations exceeds allocation quantity' + @newLineChar
			 + '  Quantity: ' + cast(@allocationQuantity as nvarchar(50)) + @newLineChar
			 + '  Sum of Fixed: ' + cast(@fixedWithinCustomers as nvarchar(50))*/
		SET @errorMsg = dbo.uf_getSystemMessage('tr_AnimationProductDetail_CheckQuantityAgainstCustomerFixed', cast(@allocationQuantity as nvarchar(50)), cast(@fixedWithinCustomers as nvarchar(50)), null, null, null, null, null, null) 
		RAISERROR (@errorMsg, 16, 18)
		ROLLBACK TRAN
		RETURN			
	END
	ELSE	
	RETURN
END
