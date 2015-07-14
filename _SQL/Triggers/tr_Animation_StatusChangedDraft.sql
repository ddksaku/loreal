IF OBJECT_ID ('tr_Animation_StatusChangedDraft', 'TR') IS NOT NULL
   DROP TRIGGER tr_Animation_StatusChangedDraft;
GO

CREATE TRIGGER tr_Animation_StatusChangedDraft
ON Animation
AFTER UPDATE 
AS 
BEGIN

	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @animationID uniqueidentifier
	SELECT @animationID = ID FROM inserted	
	
	DECLARE @newValue int	
	declare @oldValue int
	SELECT @oldValue = [Status] FROM deleted
	SELECT @newValue = [Status] FROM inserted
	
	if @oldValue = @newValue
		return
	
	-- 141a.	This will be first set for each customer group when the animation is set as ‘draft’. 
	IF (@newValue = 3)
	BEGIN
				
		exec up_calculateSystemFixedAllocation
			@animationID
		
	END	
	
	RETURN
END