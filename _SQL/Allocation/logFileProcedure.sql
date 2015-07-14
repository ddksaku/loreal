if object_id('logFile') is not null
begin
	DROP procedure logFile
end
go

create procedure logFile
(
	 @string nvarchar(max)
	,@logId uniqueidentifier
)
AS

DECLARE @newLineChar char(2)
SET @newLineChar = CHAR(13) + CHAR(10)

if object_id('dbo.allocationLog') is null
begin
	CREATE TABLE dbo.allocationLog 
	(
	 logId uniqueidentifier
	,dateCreated datetime
	,dateLastChange datetime
	,logText nvarchar(max)
	)
end

IF (select logId from dbo.allocationLog where logId = @logId) is null
BEGIN
	INSERT INTO dbo.allocationLog VALUES (@logId, GETDATE(), GETDATE(), @string)
END
ELSE
BEGIN
	UPDATE dbo.allocationLog SET dateLastChange = GETDATE(), logText = logText + @newLineChar + ISNULL(@string,'')
		WHERE logId = @logId

END


