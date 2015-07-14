
if object_id('up_orderExportJob') > 0
	drop procedure up_orderExportJob
go

create procedure up_orderExportJob
(
	@path nvarchar(255)
)
as


DECLARE @fileName nvarchar(max)


	-- exact location (on server) of DTSX file
	SET @fileName = @path + 'OrderExport.dtsx'	

	

BEGIN TRANSACTION

	DECLARE @ReturnCode INT
	DECLARE @jobId BINARY(16)
	DECLARE @jobExistingId BINARY(16)


	-- find out job
	SELECT @jobExistingId = job_id FROM msdb.dbo.sysjobs WHERE (name = N'LorealDTS_OrderExport')

	-- delete job 	
	IF (@jobExistingId IS NOT NULL)
		EXEC msdb.dbo.sp_delete_job  @job_name = N'LorealDTS_OrderExport' ;

	-- recreate job and get its ID	
	EXEC @ReturnCode = msdb.dbo.sp_add_job
		@job_name = N'LorealDTS_OrderExport',
		@job_id = @jobId OUTPUT

	-- Add the Target Servers
	EXEC msdb.dbo.sp_add_jobserver @job_id = @JobID, @server_name = N'(local)' 


	DECLARE @commandString nvarchar(max)

	SET @commandString = '/FILE "' + @fileName + '" /CONFIGFILE ' + @path + '\OrderExportConfig.dtsconfig'

	-- create step
	EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Order Export',
			@step_id=1,
			@cmdexec_success_code=0,
			@on_success_action=1,
			@on_success_step_id=0,
			@on_fail_action=2,
			@on_fail_step_id=0,
			@retry_attempts=0,
			@retry_interval=0,
			@os_run_priority=0, @subsystem=N'SSIS',
			@command=@commandString,
			@database_name=N'master',
			@flags=0
	IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback


COMMIT TRANSACTION

QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave: