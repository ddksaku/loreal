IF OBJECT_ID('up_logAuditAlert_markAsSend') > 0
	DROP PROCEDURE up_logAuditAlert_markAsSend
GO

create procedure up_logAuditAlert_markAsSend 
AS 

UPDATE dbo.AuditAlert SET DateSend = GETDATE() 
	WHERE ID IN (SELECT auditAlertID FROM dbo.AuditAlertTempForLog)


--DROP TABLE dbo.AuditAlertTempForLog
TRUNCATE TABLE dbo.AuditAlertTempForLog