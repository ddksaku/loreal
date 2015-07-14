if object_id('up_SaveAuditAlert') > 0
begin
	drop procedure up_SaveAuditAlert
end
go

create procedure up_SaveAuditAlert
	@alertType nvarchar(50)
   ,@alertDescription nvarchar(max)
   ,@divisionId uniqueidentifier
   ,@originalValue nvarchar(200) = NULL
   ,@newValue nvarchar(200) = NULL
   ,@comments nvarchar(2000) = NULL
   ,@modifiedBy NVARCHAR(255) = NULL
as
begin

	INSERT INTO dbo.AuditAlert (AlertType, AlertDescription, DateCreated, IDDivision, 
			OriginalValue, NewValue, Processed, Comments, ModifiedBy)
		VALUES (@alertType, @alertDescription, getdate(), @divisionId,
				@originalValue, @newValue, 0, @comments ,@modifiedBy)

end