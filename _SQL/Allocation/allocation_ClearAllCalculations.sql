if object_id('allocation_ClearAllCalculations') > 0
begin
	drop procedure allocation_ClearAllCalculations
end	
go

create procedure allocation_ClearAllCalculations (@animationID uniqueidentifier)
as
begin
	
	-- find animation products details
	DECLARE @animationProductDetails table
	( animationProductDetailId uniqueidentifier )

	-- find animation Productt details
	INSERT INTO @animationProductDetails 
		SELECT ID
			FROM AnimationProductDetail
				WHERE IDAnimationProduct 
				IN (select ID FROM AnimationProduct WHERE IDAnimation = @animationID)				
	
	
	-- clear customer allocations
	UPDATE dbo.CustomerAllocation SET CalculatedAllocation = 0
		WHERE IDAnimationProductDetail IN (SELECT animationProductDetailId FROM @animationProductDetails)
				
	-- clear customerGroup allocation
	UPDATE dbo.CustomerGroupAllocation SET CalculatedAllocation = 0
		WHERE IDAnimationProductDetail IN (SELECT animationProductDetailId FROM @animationProductDetails)


end