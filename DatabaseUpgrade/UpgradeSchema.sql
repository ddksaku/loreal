USE [LorealOptimiseTest]
GO

declare @SQLString nvarchar(500);

declare @ParamDefinition nvarchar(500);
declare @Params nvarchar(500);

declare @Tables table(tablename nvarchar(100));
declare @tablename nvarchar(100);
declare @reftablename nvarchar(100)
declare @columnname nvarchar(100);
declare @isnullable bit;
declare @dropFK nvarchar(200)

declare @clusterColumn nvarchar(100);
declare @pkName nvarchar(100);


-- DROP Any existing foreign keys using uniqueidentifier
declare fk_cursor cursor for
SELECT 'ALTER TABLE ' + TABLE_SCHEMA + '.' + TABLE_NAME +
' DROP CONSTRAINT ' + CONSTRAINT_NAME
FROM information_schema.table_constraints
WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'

open fk_cursor
FETCH NEXT FROM fk_cursor INTO @dropFK  

WHILE @@FETCH_STATUS = 0  
BEGIN  
	exec sp_executesql @dropFK
	
	FETCH NEXT FROM fk_cursor INTO @dropFK  
END
	
CLOSE fk_cursor  
DEALLOCATE fk_cursor 


-- Declare cursor to tablenames
DECLARE table_cursor CURSOR FOR  
SELECT table_name = sysobjects.name
FROM dbo.sysobjects 
WHERE sysobjects.xtype='U' and exists(select * from syscolumns where syscolumns.id = sysobjects.id and syscolumns.name like 'ID')

OPEN table_cursor  
FETCH NEXT FROM table_cursor INTO @tablename  

WHILE @@FETCH_STATUS = 0  
BEGIN  
	
	begin try	
	
		-- Create IntID column to a table
		set @SQLString =
		N'ALTER TABLE [' + @tablename + 
		'] ADD IntID int NOT NULL identity(1,1) '
		exec sp_executesql @SQLString

		-- Create non-clustered Unique index on created IntID column, to create foreign keys referening it later
		set @SQLString = 'CREATE UNIQUE NONCLUSTERED INDEX unq_IntID ON dbo.[' + @tablename + '] (IntID)'
		exec sp_executesql @SQLString
	

	end try
	begin catch
		print @@error
		print @SQLString
	end catch
	
	FETCH NEXT FROM table_cursor INTO @tablename  
END  

CLOSE table_cursor  
DEALLOCATE table_cursor 

-- Creating reference columns
DECLARE columns_cursor CURSOR FOR  
SELECT table_name=sysobjects.name,
       column_name=syscolumns.name,
       isnullable=syscolumns.isnullable
    FROM dbo.sysobjects 
    JOIN dbo.syscolumns ON sysobjects.id = syscolumns.id
    JOIN dbo.systypes ON syscolumns.xusertype=systypes.xusertype
   WHERE sysobjects.xtype='U' and syscolumns.name like 'ID%' and syscolumns.name != 'ID'
ORDER BY sysobjects.name,syscolumns.colid

OPEN columns_cursor  
FETCH NEXT FROM columns_cursor INTO @tablename , @columnname, @isnullable

WHILE @@FETCH_STATUS = 0  
BEGIN  

	if @tablename<>'Animation'
	begin
		FETCH NEXT FROM columns_cursor INTO @tablename , @columnname, @isnullable
		continue
	end
	
	print @tablename + '.' + @columnname

	-- Create IntID... columns, make even not-null columns nullable columns, then change them later
	set @SQLString =
	N'ALTER TABLE ' + @tablename + 
	' ADD Int' + @columnname + ' int null'
	exec sp_executesql @SQLString
	
	-- setting values using original ref columns	
	set @reftablename = rtrim(right(@columnname, LEN(@columnname)-2))
	if CHARINDEX('_', @reftablename, 0) > 0 
	begin
		set @reftablename = left(@reftablename, CHARINDEX('_', @reftablename, 0)-1)
	end
	
	if @reftablename = 'MultipleNormal'
		set @reftablename = 'Multiple'		
	else if @reftablename = 'MutlipleWarehouse'
		set @reftablename = 'Multiple'
	
	set @SQLString = 
	N'UPDATE ' + @tablename  +
	' set Int' + @columnname + '=(Select dbo.[' + @reftablename + '].IntID from dbo.[' + @reftablename + '] where dbo.[' + @reftablename + '].ID = ' + @tablename + '.' + @columnname + ')' 
	exec sp_executesql @SQLString 
	
	-- change to not null column
	if @isnullable = 0
	begin
		set @SQLString =
		N'ALTER TABLE dbo.[' + @tablename + 
		'] ALTER COLUMN Int' + @columnname + ' int not null'
		exec sp_executesql @SQLString
	end
	
	-- alter foreign key, add foreign keys,
	set @SQLString = 
	N'ALTER TABLE dbo.[' + @tablename + 
	'] WITH CHECK ADD CONSTRAINT FK_Int_' + @tablename + '_' + @reftablename + ' FOREIGN KEY(Int' + @columnname + ')' +
	' REFERENCES dbo.[' + @reftablename + '] ([IntID])'
	exec sp_executesql @SQLString
	
	FETCH NEXT FROM columns_cursor INTO @tablename , @columnname, @isnullable
END	
	
CLOSE columns_cursor  
DEALLOCATE columns_cursor 


-- After running this script and modifying all triggers, functions and procedures, 
-- run another script to drop old primary key(ID) and old Clustered_index, 
-- and also alter unq_IntId non-clustered index to clustered index
