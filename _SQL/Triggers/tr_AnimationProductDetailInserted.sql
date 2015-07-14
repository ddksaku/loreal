if object_id('tr_AnimationProductDetailInserted') > 0
	drop trigger tr_AnimationProductDetailInserted
go

create trigger tr_AnimationProductDetailInserted
on dbo.AnimationProductDetail
AFTER INSERT
AS
begin

	declare @animationProductDetail uniqueidentifier
	declare @string nvarchar(55)	
	
	declare cursor_inserted cursor 
	for select ID from inserted
	
	OPEN cursor_inserted
	
	fetch next from cursor_inserted
	into @animationProductDetail
	
	while @@FETCH_STATUS = 0
	begin
	
		set @string = cast(@animationProductDetail as nvarchar(55))	
		
		exec up_recreateAllocations	@string
	
		fetch next from cursor_inserted
		into @animationProductDetail	
		
	end

	CLOSE cursor_inserted
    DEALLOCATE cursor_inserted

	return
	
	/*
	declare @string nvarchar(max)
	select @string = COALESCE(@string, ',', '') + Cast(ID as nvarchar(55)) 
		from inserted
		
	exec up_recreateAllocations	@string

	return
	*/
	
end	












/*
declare @string nvarchar(max)
	select @string = COALESCE(@string, ',', '') + Cast(ID as nvarchar(55)) 
		from inserted
		
	exec up_recreateAllocations	@string

	return*/