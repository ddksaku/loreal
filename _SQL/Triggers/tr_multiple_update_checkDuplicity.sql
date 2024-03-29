if object_id('tr_multiple_update_checkDuplicity') > 0
	drop trigger tr_multiple_update_checkDuplicity
go
	
CREATE trigger [dbo].[tr_multiple_update_checkDuplicity]
on [dbo].[Multiple]
after update
as
begin
	IF @@ROWCOUNT = 0 RETURN 

	declare @multiple int
	declare @product uniqueidentifier
	declare @multipleID uniqueidentifier

	select @multipleID = ID from inserted
	select @multiple = [Value] from inserted
	select @product = IDProduct from inserted

	-- check if exists such record
	declare @multipleIDExisting uniqueidentifier
	select @multipleIDExisting = ID from Multiple
		where [Value] = @multiple AND IDProduct = @product AND Deleted = 0

	if @multipleIDExisting IS NOT NULL AND @multipleIDExisting <> @multipleID
	begin
			DECLARE @errorMsg nvarchar(max)
			DECLARE @newLineChar char(2)
			SET @newLineChar = CHAR(13) + CHAR(10)
			
			--SET @errorMsg = 'There already exists such multiple'
			SET @errorMsg = dbo.uf_getSystemMessage('tr_multiple_update_checkDuplicity', null, null, null, null, null, null, null, null)
			 
			RAISERROR (@errorMsg, 16, 24)
			ROLLBACK TRAN
			RETURN	

	end

	return
end