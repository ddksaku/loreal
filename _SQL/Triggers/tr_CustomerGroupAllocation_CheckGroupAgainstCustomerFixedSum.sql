if object_id('tr_CustomerGroupAllocation_CheckGroupAgainstCustomerFixedSum') > 0
	drop trigger tr_CustomerGroupAllocation_CheckGroupAgainstCustomerFixedSum
go	

CREATE TRIGGER [dbo].tr_CustomerGroupAllocation_CheckGroupAgainstCustomerFixedSum
ON [dbo].[CustomerGroupAllocation]
AFTER UPDATE, INSERT 
AS 
BEGIN	

	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @gropuFixedAllocation int	
	SELECT @gropuFixedAllocation = ManualFixedAllocation FROM inserted

	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)
		
	DECLARE @animationProductDetailId uniqueidentifier
	DECLARE @animationProductId uniqueidentifier
	DECLARE @animationId uniqueidentifier
	DECLARE @errorMsg nvarchar(max)
	DECLARE @groupID uniqueidentifier
	DECLARE @customerID uniqueidentifier
	DECLARE @customerGroupAllocationID uniqueidentifier

	
	SELECT @animationProductDetailId = IDAnimationProductDetail FROM inserted		
	SELECT @animationProductId = IDAnimationProduct FROM dbo.AnimationProductDetail WHERE ID = @animationProductDetailId
	SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId	
	SELECT @groupID  = IDCustomerGroup FROM inserted
	SELECT @customerGroupAllocationID = ID FROM dbo.CustomerGroupAllocation WHERE IDCustomerGroup = @groupID AND IDAnimationProductDetail = @animationProductDetailId

	
	declare @customers table
	(customer uniqueidentifier, fixed int)
	
	insert into @customers
		select IDCustomer, FixedAllocation
			from dbo.CustomerAllocation a inner join Customer b on (a.IDCustomer = b.ID)
				where b.IDCustomerGroup = @groupID
					and IDAnimationProductDetail = @animationProductDetailId	


	declare @fixedAllocation int

	-- sum of fixed within customers
	select @fixedAllocation = sum(isnull(fixed, 0))
		from @customers	
		
	set @fixedAllocation = isnull(@fixedAllocation, 0)		
	
	IF @gropuFixedAllocation < @fixedAllocation
	BEGIN
		/*SET @errorMsg = 'Sum of customer''s Fixed allocations exceeds customer group manual fixed allocation' + @newLineChar
		 + '  Sum of Fixed: ' + cast(@fixedAllocation as nvarchar(50)) + @newLineChar
		 + '  Manual Fixed CG: ' + cast(@gropuFixedAllocation as nvarchar(50))*/
		SET @errorMsg = dbo.uf_getSystemMessage('tr_CustomerGroupAllocation_CheckGroupAgainstCustomerFixedSum', cast(@fixedAllocation as nvarchar(50)), cast(@gropuFixedAllocation as nvarchar(50)), null, null, null, null, null, null)
		RAISERROR (@errorMsg, 16, 23)
		ROLLBACK TRAN
		RETURN	
	END
END	
	
