declare @settingName nvarchar(255)
declare @settingValue nvarchar(10)


-- default requested delivery timing
set @settingName = 'DefaultRequestedDeliveryTiming'
set @settingValue = '1'

insert into dbo.ApplicationSettings
	select newid(), @settingName, @settingValue, ID
		from dbo.Division
		
-- default PLV delivery timing		
set @settingName = 'DefaultPLVDeliveryTiming'
set @settingValue = '2'

insert into dbo.ApplicationSettings
	select newid(), @settingName, @settingValue, ID
		from dbo.Division
		
-- default PLV component timing		
set @settingName = 'DefaultPLVComponentTiming'
set @settingValue = '3'

insert into dbo.ApplicationSettings
	select newid(), @settingName, @settingValue, ID
		from dbo.Division
		
-- default stock timing		
set @settingName = 'DefaultStockTiming'
set @settingValue = '4'

insert into dbo.ApplicationSettings
	select newid(), @settingName, @settingValue, ID
		from dbo.Division		

