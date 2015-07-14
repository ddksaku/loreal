if object_id('tr_customerAllocation_avoidDuplicity') > 0
	drop trigger tr_customerAllocation_avoidDuplicity
go

create trigger tr_customerAllocation_avoidDuplicity
on CustomerAllocation
after update, insert
as
begin

	IF @@ROWCOUNT = 0 RETURN 

	declare @cusAlloID uniqueidentifier
	select @cusAlloID = ID from inserted

	declare @customer uniqueidentifier
	declare @animationProductDetail uniqueidentifier

	select @customer = IDCustomer from inserted
	select @animationProductDetail = IDAnimationProductDetail from inserted

	declare @id uniqueidentifier
	select @id = ID from dbo.CustomerAllocation 
		where IDCustomer = @customer and IDAnimationProductDetail = @animationProductDetail
		and ID <> @cusAlloID
	-- if already exists such combination
	if @id is not null
	begin
		
		DECLARE @errorMsg nvarchar(max)
		DECLARE @newLineChar char(2)
		SET @newLineChar = CHAR(13) + CHAR(10)
		
		--SET @errorMsg = 'This Customer already exists within this allocation.'
		SET @errorMsg = dbo.uf_getSystemMessage('tr_customerAllocation_avoidDuplicity', null, null, null, null, null, null, null, null)
		 
		RAISERROR (@errorMsg, 16, 25)
		ROLLBACK TRAN
		RETURN	

	end

end