if object_id('up_orderExport_orderType') > 0
	drop procedure up_orderExport_orderType
go

create procedure up_orderExport_orderType
	@animationID uniqueidentifier
as

SELECT Code FROM OrderType WHERE ID IN (SELECT IDOrderType_Order FROM Animation WHERE ID = @animationID)


