create procedure up_createOrderExport @animationID uniqueidentifier
as

declare @folder nvarchar(max)
set @folder = 'C:\inetpub\wwwroot\dev2.memos.cz\LorealOptimise\'

declare @exec_string as varchar(max)
set @exec_string = N'master..xp_cmdshell ' + '''dtexec /f "' + @folder + 'OrderExport.dtsx"  /set \package.variables[animationID].Value;"'+cast(@animationID as nvarchar(55))+'" /conf "'+@folder+'OrderExportConfig.dtsconfig"'''
execute (@exec_string)

