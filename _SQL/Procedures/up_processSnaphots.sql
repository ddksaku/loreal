if object_id('up_processSnaphots') > 0
	drop procedure up_processSnaphots
go

create procedure up_processSnaphots
as
begin

	declare @snapshots table
	(ID int, iterator int identity(1,1))
	
	insert into @snapshots
		select ID
			from dbo.VersionSnapshotsToBeCreated
				where processed = 0
				
	declare @iterator int
	declare @id int
	declare @animationID uniqueidentifier
	declare @createdBy nvarchar(max)
	select @iterator = max(iterator) from @snapshots		

	while @iterator > 0
	begin
	
		select @id = ID from @snapshots where iterator = @iterator
	
		select @animationID = IDAnimation, @createdBy = ModifiedBy
			from dbo.VersionSnapshotsToBeCreated where ID = @id
	
		exec dbo.up_createSnapshot @animationId, NULL, @createdBy
		
		update dbo.VersionSnapshotsToBeCreated set Processed = 1
			where ID = @id
	
		set @iterator = @iterator - 1
	
	end


end	