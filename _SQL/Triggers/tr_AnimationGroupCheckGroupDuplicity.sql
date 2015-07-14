if object_id('tr_AnimationGroupCheckGroupDuplicity') > 0
	drop trigger tr_AnimationGroupCheckGroupDuplicity
go

create trigger tr_AnimationGroupCheckGroupDuplicity
on dbo.AnimationCustomerGroup
for insert
as
begin


	declare @animationID uniqueidentifier
	declare @id uniqueidentifier
	declare @groupID uniqueidentifier
	declare @errorMsg nvarchar(max)
	declare @uID uniqueidentifier
	select @animationID = IDAnimation from inserted
	select @groupID = IDCustomerGroup from inserted
	select @uID = ID from inserted
	
	select @id = ID from  dbo.AnimationCustomerGroup
	where IDAnimation = @animationID and IDCustomerGroup = @groupID
	and ID <> @uID
	
	
	if @id is not null
	begin			
			SET @errorMsg =  dbo.uf_getSystemMessage('tr_AnimationGroupCheckGroupDuplicity', null, null, null, null, null, null, null, null)
			RAISERROR (@errorMsg, 16, 31)
			ROLLBACK TRAN
			RETURN		
	end

end