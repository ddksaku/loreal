if object_id('tr_AnimationProductDetail_CheckQuantityAgainstSystemFixed') > 0
	drop trigger [tr_AnimationProductDetail_CheckQuantityAgainstSystemFixed]
go

CREATE TRIGGER [dbo].[tr_AnimationProductDetail_CheckQuantityAgainstSystemFixed]
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
	DECLARE @fixedWithinGroups int
	DECLARE @allocationQuantity int		
	DECLARE @animationProductDetailID uniqueidentifier


	SELECT @animationProductDetailID = ID FROM inserted
	SELECT @allocationQuantity = AllocationQuantity FROM inserted
	SELECT @animationProductId = IDAnimationProduct FROM inserted

	
	-- get customer groups 
	DECLARE @groups TABLE (groupID uniqueidentifier, manualFixed int)

	INSERT INTO @groups 
		SELECT IDCustomerGroup, ManualFixedAllocation FROM dbo.CustomerGroupAllocation WHERE IDAnimationProductDetail = @animationProductDetailID

	
	-- sum of all stores capacities
	SELECT @fixedWithinGroups = SUM(ISNULL(manualFixed,0)) FROM @groups
	
	set @fixedWithinGroups = isnull(@fixedWithinGroups, 0)
	
	IF @fixedWithinGroups > @allocationQuantity
	BEGIN
		/*SET @errorMsg = 'Sum of group fixed allocations exceeds allocation quantity' + @newLineChar
			 + '  Quantity: ' + cast(@allocationQuantity as nvarchar(50)) + @newLineChar
			 + '  Sum of Fixed: ' + cast(@fixedWithinGroups as nvarchar(50))*/
		SET @errorMsg = dbo.uf_getSystemMessage('tr_AnimationProductDetail_CheckQuantityAgainstSystemFixed', cast(@allocationQuantity as nvarchar(50)), cast(@fixedWithinGroups as nvarchar(50)), null, null, null, null, null, null)
		RAISERROR (@errorMsg, 16, 17)
		ROLLBACK TRAN
		RETURN			
	END
	ELSE	
	RETURN
END
