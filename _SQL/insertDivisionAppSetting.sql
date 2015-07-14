insert into dbo.ApplicationSettings (ID, SettingKey, SettingValue, IDDivision)
	select newid(), 'AllocationReportFooter', 'Allocation Report Footer Sample', ID
		from dbo.Division
		
		
insert into dbo.ApplicationSettings (ID, SettingKey, SettingValue, IDDivision)
	select newid(), 'StoreAllocationReportFooter', 'Store Allocation Report Footer Sample', ID
		from dbo.Division	