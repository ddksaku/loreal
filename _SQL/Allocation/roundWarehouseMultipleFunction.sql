create function dbo.RoundWarehouseMultiple (@quantity int, @multipleNormal int)
returns int
as
begin
	return round(cast(@quantity as float)/cast(@multipleNormal as float), 0)
end


