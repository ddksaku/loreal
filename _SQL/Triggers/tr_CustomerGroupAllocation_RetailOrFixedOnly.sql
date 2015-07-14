IF OBJECT_ID ('tr_CustomerGroupAllocation_RetailOrFixedOnly', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerGroupAllocation_RetailOrFixedOnly;
GO

CREATE TRIGGER tr_CustomerGroupAllocation_RetailOrFixedOnly
ON CustomerGroupAllocation
AFTER UPDATE, INSERT 
AS 
BEGIN
	IF @@ROWCOUNT = 0 RETURN 
	
	DECLARE @fixedAllocation int
	SELECT @fixedAllocation = ManualFixedAllocation FROM inserted

	declare @retailUplift float
	select @retailUplift = RetailUplift FROM inserted

	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)
	
	DECLARE @errorMsg nvarchar(max)
		
	IF @fixedAllocation IS NOT NULL AND @retailUplift IS NOT NULL
		BEGIN
			--SET @errorMsg = 'Just one from Retail Uplift and Fixed Allocation can be filled.' + @newLineChar	 
			SET @errorMsg = dbo.uf_getSystemMessage('tr_CustomerGroupAllocation_RetailOrFixedOnly', null, null, null, null, null, null, null, null)
			
			RAISERROR (@errorMsg, 16, 16)
			ROLLBACK TRAN
			RETURN	
		END
	
	
	RETURN
END