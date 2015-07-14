IF OBJECT_ID('dbo.uf_createWeeklyReport') > 0
BEGIN
	DROP FUNCTION dbo.uf_createWeeklyReport
END
GO

CREATE FUNCTION dbo.uf_createWeeklyReport (@divisionId UNIQUEIDENTIFIER)
RETURNS NVARCHAR(max)
BEGIN
	
	DECLARE @output NVARCHAR(max)
	DECLARE @dateTo DATETIME
	DECLARE @dateFrom DATETIME
	DECLARE @divisionName NVARCHAR(50)
	DECLARE @divisionCode NVARCHAR(10)
	DECLARE @newLineChar char(2)
	SET @newLineChar = CHAR(13) + CHAR(10)
	DECLARE @recordLimit int
	
	SELECT @recordLimit = cast([SettingValue] as int) from dbo.ApplicationSettings
		where SettingKey = 'AuditAlertRecordLimit'
		
	SET @recordLimit = ISNULL(@recordLimit, 10)	
	
	declare @auditAlertPeriod int
	select @auditAlertPeriod = cast(SettingValue as int) from dbo.ApplicationSettings
		where SettingKey = 'AuditAlertPeriod'
		
	if @auditAlertPeriod is null
		set @auditAlertPeriod = 7
	
	SET @dateTo = GETDATE()
	SELECT @dateFrom = DATEADD(DAY, -@auditAlertPeriod, @dateTo)
	
	SELECT @divisionCode = Code, @divisionName = Name FROM dbo.Division WHERE ID = @divisionId
	
	
	SET @output =  'Audit Alert - Weekly Report'  + @newLineChar
		+ CAST(@dateFrom AS NVARCHAR(50)) + ' - ' + CAST(@dateTo AS NVARCHAR(50)) + @newLineChar +
		+ 'Division: ' + @divisionName + ' (' + @divisionCode + ')' +
		+ @newLineChar + @newLineChar
		
	IF (SELECT COUNT(*) FROM dbo.AuditAlert WHERE IDDivision = @divisionId) = 0
	BEGIN
		SET @output = @output + 'There is nothing to report.'
		RETURN @output
	END
	
	-- get alert types
	DECLARE @alertTypes TABLE (typeName NVARCHAR(255), countOfRecords int)
	
	INSERT INTO @alertTypes (typeName)
		SELECT DISTINCT AlertType
			FROM dbo.AuditAlert
			
	UPDATE @alertTypes SET countOfRecords = 
		 (SELECT count(*) as pocet from dbo.AuditAlert 
				WHERE AlertType COLLATE DATABASE_DEFAULT = typeName COLLATE DATABASE_DEFAULT
				AND DateCreated BETWEEN @dateFrom AND @dateTo
				AND IDDivision = @divisionId)


	DECLARE @alertType NVARCHAR(255)
	DECLARE @tempOutput NVARCHAR(max)
	
	DECLARE cursorAlertType CURSOR	
	FOR SELECT typeName FROM @alertTypes
	
	-- summary
	SET @output = @output + 'Summary:' + @newLineChar
	SET @output = @output + '========' + @newLineChar
	
	DECLARE @summary NVARCHAR(MAX)
	
	SELECT @summary = COALESCE(@summary, @newLineChar) + typeName + ':   ' + CAST(countOfRecords AS NVARCHAR(20)) + ' alert/s' + @newLineChar
		FROM @alertTypes
	
	SET @output = @output + @summary + @newLineChar + 'Details: ' + @newLineChar + '========' + @newLineChar
	
	declare @sqlString nvarchar(max)
	
	
	OPEN cursorAlertType
	FETCH NEXT FROM cursorAlertType INTO @alertType
	
	WHILE @@FETCH_STATUS = 0
	BEGIN			
		SET @tempOutput = ''		
		
		IF (SELECT countOfRecords FROM @alertTypes WHERE typeName COLLATE DATABASE_DEFAULT = @alertType COLLATE DATABASE_DEFAULT) > 0
		BEGIN
			SELECT @output = @output +  @alertType + ' (' + CAST(COUNT(*) AS NVARCHAR(25)) + ' record/s):'  COLLATE DATABASE_DEFAULT
				FROM dbo.AuditAlert
					WHERE IDDivision = @divisionId AND Processed = 0 AND AlertType = @alertType	
						AND DateCreated BETWEEN @dateFrom AND @dateTo
			
			SELECT top 10 @tempOutput = COALESCE(@tempOutput,@newLineChar) + @newLineChar  + CAST(DateCreated AS NVARCHAR(50))+ ' ' + AlertDescription + ' ' + @newLineChar +  'Original value: ' + ISNULL(OriginalValue,'--') + @newLineChar + 'New value: ' + ISNULL(NewValue,'--') + @newLineChar + 'Modified By: ' + ModifiedBy + @newLineChar COLLATE DATABASE_DEFAULT
				FROM dbo.AuditAlert
					WHERE IDDivision = @divisionId AND Processed = 0 AND AlertType = @alertType	
						AND DateCreated BETWEEN @dateFrom AND @dateTo	
						order by DateCreated desc
		
			SET @output = @output + @tempOutput	+ @newLineChar
		END
	
		FETCH NEXT FROM cursorAlertType INTO @alertType
	
	END

	SET @output = @output + @newLineChar + @newLineChar + '(email shows max 10 records for each alert type)' +@newLineChar +'(generated automatically)'

	RETURN @output

END


