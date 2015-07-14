IF OBJECT_ID ('tr_CustomerCapacity_CapacityChanged', 'TR') IS NOT NULL
   DROP TRIGGER tr_CustomerCapacity_CapacityChanged;
GO

CREATE TRIGGER tr_CustomerCapacity_CapacityChanged
ON CustomerCapacity
AFTER UPDATE 
AS 
BEGIN

	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)


	DECLARE @oldValue int
	DECLARE @newValue INT
	DECLARE @modifiedBy NVARCHAR(255)
	SELECT @oldValue = Capacity FROM deleted
	SELECT @newValue = Capacity FROM INSERTED
	SELECT @modifiedBy = ModifiedBy FROM INSERTED	
	DECLARE @productID UNIQUEIDENTIFIER

	IF @oldValue <> @newValue
	BEGIN
		
		-- find potentional impacted animations
		-- g.	Capacity changes for draft, published and closed allocations.

		DECLARE @animations TABLE (animationID UNIQUEIDENTIFIER, divisionID UNIQUEIDENTIFIER, alertMessage NVARCHAR(max))
		DECLARE @priorityID UNIQUEIDENTIFIER
		DECLARE @priorityName NVARCHAR(50)
		DECLARE @itemTypeID UNIQUEIDENTIFIER
		DECLARE @itemTypeName NVARCHAR(50)
		DECLARE @animationTypeID UNIQUEIDENTIFIER
		DECLARE @animationTypeName NVARCHAR(50)
		DECLARE @customerID UNIQUEIDENTIFIER
		DECLARE @customerName NVARCHAR(50)
		DECLARE @customerCode NVARCHAR(20)
		DECLARE @salesAreaID uniqueidentifier
		DECLARE @customerGroupID uniqueidentifier

		
		SELECT @priorityID = IDPriority, @itemTypeID = IDItemType, @animationTypeID = IDAnimationType FROM inserted
		SELECT @customerID = IDCustomer FROM inserted
		SELECT @customerGroupID = IDCustomerGroup FROM dbo.Customer WHERE ID = @customerID
		SELECT @salesAreaID = IDSalesArea_AllocationSalesArea FROM dbo.Customer WHERE ID = @customerID
		SELECT @customerName = Name, @customerCode = AccountNumber FROM dbo.Customer WHERE ID = @customerID
		SELECT @priorityName = Name FROM dbo.Priority WHERE ID = @priorityID
		SELECT @animationTypeName = Name FROM dbo.AnimationType WHERE ID = @animationTypeID
		SELECT @itemTypeName = Name FROM dbo.ItemType WHERE ID = @itemTypeID
				
		
		IF @salesAreaID IS NULL
		BEGIN
			DECLARE @groupID uniqueidentifier
			SELECT @groupID = IDCustomerGroup FROM dbo.Customer WHERE ID = @customerID
			SELECT @salesAreaID = IDSalesArea FROM dbo.CustomerGroup WHERE ID = @groupID
		END

		-- find draft, published and closed animations
		INSERT INTO @animations (animationID, divisionID, alertMessage)
			SELECT a.ID, IDDivision, dbo.uf_getSystemMessage('tr_CustomerCapacity_CapacityChanged', a.Name, a.Code, @customerName, @customerCode, @priorityName, @itemTypeName, @animationTypeName, null)
			
			/*'Customer capacity changed on following animation:' + @newLineChar +
									   'Animation: ' + a.Name + ' (' + a.Code + ')' + @newLineChar +
									   'Customer: ' + @customerName + ' (' + @customerCode + ')' + @newLineChar +
									   'Priority: ' + @priorityName + @newLineChar +
									   'Item Type: ' + @itemTypeName + @newLineChar +
									   'Animation Type: ' + @animationTypeName	*/
			FROM dbo.Animation AS a
				
				WHERE a.IDPriority = @priorityID AND a.IDAnimationType = @animationTypeID AND a.[Status] IN (3, 4, 5)
				
			
		--> alert
		INSERT INTO dbo.AuditAlert  (
		          AlertType ,
		          AlertDescription ,
		          DateCreated ,
		          IDDivision ,
		          OriginalValue ,
		          NewValue ,
		          Processed ,
		          ModifiedBy
		        )
		SELECT 'Capacity', alertMessage, GETDATE(), divisionID, @oldValue, @newValue, 0, @modifiedBy
			FROM @animations
			
		
	
	END
	
	
	RETURN
END