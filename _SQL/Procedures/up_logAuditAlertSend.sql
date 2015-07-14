IF OBJECT_ID('up_logAuditAlertSend') > 0
	DROP PROCEDURE up_logAuditAlertSend
GO

create procedure up_logAuditAlertSend @divisionId UNIQUEIDENTIFIER
AS 
	
IF OBJECT_ID('dbo.AuditAlertTempForLog') IS NULL
	CREATE TABLE AuditAlertTempForLog (auditAlertID UNIQUEIDENTIFIER)
	
	
DECLARE @dateTo DATETIME
DECLARE @dateFrom DATETIME	
SET @dateTo = GETDATE()
SELECT @dateFrom = DATEADD(DAY, -7, @dateTo)


INSERT INTO dbo.AuditAlertTempForLog
	SELECT ID 
		FROM dbo.AuditAlert
			WHERE DateCreated BETWEEN @dateFrom AND @dateTo
				AND DateSend IS NULL
				AND ID NOT IN (SELECT auditAlertID FROM dbo.AuditAlertTempForLog)