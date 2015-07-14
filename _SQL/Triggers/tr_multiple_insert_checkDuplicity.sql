if object_id('tr_multiple_insert_checkDuplicity') > 0
	drop trigger tr_multiple_insert_checkDuplicity
go


create trigger tr_multiple_insert_checkDuplicity
on dbo.Multiple
instead of insert 
as
begin
	IF @@ROWCOUNT = 0 RETURN 

	declare @multiple int
	declare @product uniqueidentifier

	select @multiple = [Value] from inserted
	select @product = IDProduct from inserted

	-- check if exists such record
	declare @multipleIDExisting uniqueidentifier
	select @multipleIDExisting = ID from Multiple
		where [Value] = @multiple AND IDProduct = @product AND Deleted = 0

	if @multipleIDExisting IS NULL
	begin
		insert into dbo.Multiple
			select * from inserted
	end
	else
	begin
		declare @deleted bit
		declare @manual bit

		select @deleted = Deleted from inserted
		select @manual = [Manual] from inserted

		update dbo.Multiple set ModifiedDate = getdate(), Deleted = @deleted, [Manual] = @manual
			where ID = @multipleIDExisting

	end

	return
end