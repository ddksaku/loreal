create function dbo.countGroupCapacity(@animationProductDetailId uniqueidentifier, @groupId uniqueidentifier)
returns int
as
begin

declare @output int

DECLARE @animationProductId uniqueidentifier
SELECT @animationProductId = IDAnimationProduct FROM AnimationProductDetail WHERE ID = @animationProductDetailId

DECLARE @animationId uniqueidentifier
SELECT @animationId = IDAnimation FROM AnimationProduct WHERE ID = @animationProductId

DECLARE @animationTypeId uniqueidentifier
SELECT @animationTypeId = IDAnimationType FROM Animation WHERE ID = @animationId

DECLARE @itemTypeId uniqueidentifier
SELECT @itemTypeId = IDItemType FROM AnimationProduct WHERE ID = @animationProductId

DECLARE @priorityId  uniqueidentifier
SELECT @priorityId = IDPriority FROM Animation WHERE ID = @animationId

SELECT @output = SUM(Capacity) FROM dbo.CustomerCapacity
	WHERE IDAnimationType = @animationTypeId AND IDPriority = @priorityId AND IDItemType = @itemTypeId
		AND IDCustomer IN (SELECT ID FROM Customer WHERE IDCustomerGroup = @groupId)

return @output
end