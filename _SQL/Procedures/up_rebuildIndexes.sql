if object_id('up_rebuildIndexes') > 0
	drop procedure up_rebuildIndexes
go

create procedure up_rebuildIndexes	
as

	Declare @db   SysName;
	Set @db = DB_NAME()



	create table #indexesToRebuild 
	(
	 tableName nvarchar(255),
	 indexName  nvarchar(255),
	 ID int identity(1,1)
	 )

	insert into #indexesToRebuild (tableName, indexName)
	SELECT CAST(OBJECT_NAME(S.Object_ID, DB_ID(@db)) AS VARCHAR(55)) as TableName , I.name as IndexName
	FROM sys.dm_db_index_physical_stats (DB_ID(@db),NULL,NULL,NULL,'DETAILED' ) S
	LEFT OUTER JOIN sys.indexes I On (I.Object_ID = S.Object_ID and I.Index_ID = S.Index_ID)
	AND S.INDEX_ID > 0
	where avg_fragmentation_in_percent > 20
	ORDER BY avg_fragmentation_in_percent DESC


	declare @iterator int
	declare @table nvarchar(255)
	declare @index nvarchar(255)
	declare @query nvarchar(max)
	select @iterator = max(ID) from #indexesToRebuild

	while @iterator > 0
	begin

		select @table = tableName, @index = indexName
			from #indexesToRebuild where ID = @iterator

		set @query = 'alter index ' + @index +' ON ['+ @table +'] REBUILD'
		exec(@query)

		set @iterator = @iterator - 1
	end


	drop table #indexesToRebuild

