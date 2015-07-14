
if object_id('uf_getSystemMessage') > 0
	drop function uf_getSystemMessage
go

create function uf_getSystemMessage (@messageCode nvarchar(255), @p1 nvarchar(255), @p2 nvarchar(max), @p3 nvarchar(255) , @p4 nvarchar(255), @p5 nvarchar(255), @p6 nvarchar(255), @p7 nvarchar(255), @p8 nvarchar(255))	
returns nvarchar(max)
begin

	declare @output nvarchar(max)

	declare @newLineChar char(2)
	set @newLineChar = CHAR(13) + CHAR(10)


	select @output = MessageContent from dbo.SystemMessage where Code = @messageCode

	-- add new lines
	set  @output = replace(@output, '&newLineChar', @newLineChar)

	-- replace parametres
	if @p1 is not null
		set @output = replace(@output, '&p1', @p1)
		
	if @p2 is not null
		set @output = replace(@output, '&p2', @p2)
		
	if @p3 is not null
		set @output = replace(@output, '&p3', @p3)
		
	if @p4 is not null
		set @output = replace(@output, '&p4', @p4)	
		
	if @p5 is not null
		set @output = replace(@output, '&p5', @p5)	
		
	if @p6 is not null
		set @output = replace(@output, '&p6', @p6)	
		
	if @p7 is not null
		set @output = replace(@output, '&p7', @p7)	
		
	if @p8 is not null
		set @output = replace(@output, '&p8', @p8)					

	return @output

end