CREATE PROCEDURE [dbo].[rep_GroupAllocationReport] 
							--@salesDriveId VARCHAR(MAX),
							--@animations NVARCHAR(max),
							--@salesArea NVARCHAR(MAX),
							--@itemGroupId NVARCHAR(MAX)

AS

	SET NOCOUNT ON

DECLARE @SalesID uniqueidentifier,
		@Ind int

SET @Ind = 1
DECLARE @SqlString_1 NVARCHAR(MAX)
DECLARE @SqlString_2 NVARCHAR(MAX)
DECLARE @SqlString_3 NVARCHAR(MAX)

SET @SqlString_1 = N'SELECT T.ID, '

SET @SqlString_2 = N' FROM (SELECT distinct ID FROM AnimationProductDetail) T '

SET @SqlString_3 = N''

DECLARE cur CURSOR FOR 
	SELECT ID FROM SalesArea
OPEN cur
FETCH NEXT FROM cur INTO @SalesID;
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @SqlString_1 = @SqlString_1 + 'T' + CAST(@Ind as VARCHAR(10)) + '.SignOffStatus' + CAST(@Ind as VARCHAR(10)) + ', T' + CAST(@Ind as VARCHAR(10)) + '.RRP' + CAST(@Ind as VARCHAR(10)) + ', T' + CAST(@Ind as VARCHAR(10)) + '.ListPrice' + CAST(@Ind as VARCHAR(10)) + ', T' + CAST(@Ind as VARCHAR(10)) + '.AvailableAllocation' + CAST(@Ind as VARCHAR(10)) + ', ' 
	
	SET @SqlString_3 = @SqlString_3 +
		N'LEFT JOIN
		(SELECT APD.ID, ''Signed Off'' SignOffStatus' + CAST(@Ind as VARCHAR(10)) + ', APD.RRP RRP' + CAST(@Ind as VARCHAR(10)) + ', APD.RRP * SA.RRPToListPriceRate ListPrice' + CAST(@Ind as VARCHAR(10)) + ', 0 AvailableAllocation' + CAST(@Ind as VARCHAR(10)) + '
		FROM AnimationProductDetail APD
		JOIN SalesArea SA
		ON APD.IDSalesArea = SA.ID
		WHERE SA.ID = ''' + CAST(@SalesID as VARCHAR(50)) + ''')T' + CAST(@Ind as VARCHAR(10)) + '
		ON T.ID = T' + CAST(@Ind as VARCHAR(10)) + '.ID '
	
	SET @Ind = @Ind + 1
	FETCH NEXT FROM cur INTO @SalesID;
END

CLOSE cur
DEALLOCATE cur

DECLARE @SqlResult NVARCHAR(MAX)

SET @SqlString_1 = SUBSTRING(@SqlString_1,1,LEN(@SqlString_1)-1)

SET @SqlResult = @SqlString_1 + @SqlString_2 + @SqlString_3

--SELECT @SqlResult

--	SELECT	A.ID,
--			A.Name AnimationName,
--			A.OnCounterDate OnCounterDate,
--			A.SAPDespatchCode SapAnimationCode,
--			IG.Name ItemGroupName,
--			P.MaterialCode MaterialCode, 
--			P.[Description] ProductDescription, 
--			P.EAN EanBarcode, 
--			M.Value Multiple
--	FROM AnimationProductDetail APD
--	JOIN AnimationProduct AP
--	ON APD.IDAnimationProduct = AP.ID
--	JOIN Product P
--	ON AP.IDProduct = P.ID
--	LEFT JOIN Multiple M
--	ON AP.IDMultipleNormal = M.ID
--	JOIN ItemGroup IG
--	ON AP.IDItemGroup = IG.ID
--	JOIN Animation A
--	ON AP.IDAnimation = A.ID
	
SET @SqlResult = '	SELECT	A.ID,
							A.Name AnimationName,
							A.OnCounterDate OnCounterDate,
							A.SAPDespatchCode SapAnimationCode,
							IG.Name ItemGroupName,
							P.MaterialCode MaterialCode, 
							P.[Description] ProductDescription, 
							P.EAN EanBarcode, 
							M.Value Multiple, TT.* 
					FROM AnimationProductDetail APD
					JOIN AnimationProduct AP
					ON APD.IDAnimationProduct = AP.ID
					JOIN Product P
					ON AP.IDProduct = P.ID
					LEFT JOIN Multiple M
					ON AP.IDMultipleNormal = M.ID
					JOIN ItemGroup IG
					ON AP.IDItemGroup = IG.ID
					JOIN Animation A
					ON AP.IDAnimation = A.ID
					JOIN (' + @SqlResult + ')TT
					ON APD.ID = TT.ID
					ORDER BY A.OnCounterDate, A.Name, IG.Name, P.MaterialCode'  


--SELECT @SqlResult
EXECUTE sp_executesql @SqlResult


