if object_id('tr_CustomerBrandExclusion_Excluded') > 0
	drop trigger tr_CustomerBrandExclusion_Excluded
go

create trigger tr_CustomerBrandExclusion_Excluded
on dbo.CustomerBrandExclusion
after update, insert
as
begin

	IF @@ROWCOUNT = 0 RETURN 


	declare @errorMsg nvarchar(max)	
	declare @newLineChar char(2)
	set @newLineChar = CHAR(13) + CHAR(10)

	-- to avoid joining null string
	set @errorMsg = ''

	declare @excludedOld bit
	declare @excludedNew bit

	select @excludedOld = Excluded from deleted
	select @excludedNew = Excluded from inserted

	if @excludedOld <> @excludedNew AND @excludedNew = 1
	begin

		declare @customerID uniqueidentifier
		select @customerID = IDCustomer from deleted

		exec up_checkCapacitiesAfterUpdate			
			 @customerID
			,NULL
			,@errorMsg OUTPUT



		-- raise error
		if Len(@errorMsg) > 1
		begin
			RAISERROR (@errorMsg, 16, 38)
			ROLLBACK TRAN
			RETURN		
		end

	end

end