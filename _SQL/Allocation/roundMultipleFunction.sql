create function dbo.RoundMultiple (@quantity int, @multipleNormal int)
returns int
as
begin
	return @quantity/@multipleNormal
end