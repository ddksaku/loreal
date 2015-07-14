IF OBJECT_ID ('tr_Animation_StatusChanged', 'TR') IS NOT NULL
   DROP TRIGGER tr_Animation_StatusChanged;
GO

CREATE TRIGGER tr_Animation_StatusChanged
ON Animation
AFTER UPDATE 
AS 
BEGIN

	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @animationID uniqueidentifier
	SELECT @animationID = ID FROM inserted
	
	DECLARE @oldValue int
	DECLARE @newValue int

	SELECT @oldValue = [Status] FROM deleted
	SELECT @newValue = [Status] FROM inserted
	
	IF @oldValue <> @newValue
	BEGIN
			
		exec uf_allocate_animationID @animationID
		
	END	
	
	RETURN
END