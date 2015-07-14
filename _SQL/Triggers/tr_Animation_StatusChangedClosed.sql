if object_id('tr_Animation_StatusChangedClosed') > 0
	drop trigger tr_Animation_StatusChangedClosed
go

create trigger tr_Animation_StatusChangedClosed
on dbo.Animation
for update
as
begin

	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @animationID uniqueidentifier
	SELECT @animationID = ID FROM inserted	
	
	DECLARE @newValue int	
	declare @oldValue int
	SELECT @oldValue = [Status] FROM deleted
	SELECT @newValue = [Status] FROM inserted
	
	if @oldValue = @newValue
		return
	
	
	IF (@newValue = 5)
	BEGIN
				
		exec up_createOrderExport
			@animationID
		
	END	
	
	RETURN

end	