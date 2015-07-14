IF OBJECT_ID ('tr_AnimationProductDetail_Forecast', 'TR') IS NOT NULL
   DROP TRIGGER tr_AnimationProductDetail_Forecast;
GO


CREATE TRIGGER tr_AnimationProductDetail_Forecast
ON AnimationProductDetail
AFTER UPDATE 
AS 
BEGIN

	IF @@ROWCOUNT = 0 RETURN 

	DECLARE @animationProductId uniqueidentifier
	DECLARE @animationId uniqueidentifier
	DECLARE @animationProductDetailID uniqueidentifier	
	DECLARE @allocationQuantity int
	DECLARE @errorMsg nvarchar(max)
	DECLARE @forecast int
	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)

	SELECT @allocationQuantity = AllocationQuantity FROM inserted
	SELECT @animationProductId = IDAnimationProduct FROM inserted
	SELECT @animationID = IDAnimation FROM dbo.AnimationProduct WHERE ID = @animationProductId

	SELECT @forecast = ForecastProcQuantity FROM inserted

	IF @allocationQuantity > @forecast
	BEGIN
		SET @errorMsg = 'Allocation quantity exceeds the Forecast Procurement Quantity' + @newLineChar
		 + '  Quantity: ' + cast(@allocationQuantity as nvarchar(50)) + @newLineChar
		 + '  Forecast: ' + cast(@forecast as nvarchar(50))
		RAISERROR (@errorMsg, 1, 3)			
		--ROLLBACK TRAN		
		RETURN	
	END

	RETURN
END
GO