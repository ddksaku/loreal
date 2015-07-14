

BEGIN TRANSACTION

	DECLARE @ReturnCode INT
	DECLARE @jobId BINARY(16)
	DECLARE @jobExistingId BINARY(16)


	-- find out job
	SELECT @jobExistingId = job_id FROM msdb.dbo.sysjobs WHERE (name = N'LorealProcessSnapshots')

	-- delete job 	
	IF (@jobExistingId IS NOT NULL)
		EXEC msdb.dbo.sp_delete_job  @job_name = N'LorealProcessSnapshots' ;

	-- recreate job and get its ID	
	EXEC @ReturnCode = msdb.dbo.sp_add_job
		@job_name = N'LorealProcessSnapshots',
		@job_id = @jobId OUTPUT

	-- Add the Target Servers
	EXEC msdb.dbo.sp_add_jobserver @job_id = @JobID, @server_name = N'(local)' 


	-- create step
	EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'01LorealProcessSnapshots',
			@step_id=1,
			@cmdexec_success_code=0,
			@on_success_action=1,
			@on_success_step_id=0,
			@on_fail_action=2,
			@on_fail_step_id=0,
			@retry_attempts=0,
			@retry_interval=0,
			@os_run_priority=0, @subsystem=N'TSQL',
			@command = N'EXEC dbo.up_processSnaphots',
			@database_name=N'master',
			@flags=0
	IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
	
	EXEC msdb.dbo.sp_add_schedule
		@schedule_name = N'LorealProcessSnapshotsSchedule' ,
		@freq_type = 4,
		@freq_interval = 1,
		@active_start_time = 010000 ;


	EXEC msdb.dbo.sp_attach_schedule
	   @job_name = N'LorealProcessSnapshots',
	   @schedule_name = N'LorealProcessSnapshotsSchedule' ;



COMMIT TRANSACTION

QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave: