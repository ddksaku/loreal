if object_id('tr_insertNewDivision') > 0
	drop trigger tr_insertNewDivision
go

create trigger tr_insertNewDivision
on dbo.Division
after insert
as
begin


	declare @divisionID uniqueidentifier
	select @divisionID = ID from inserted
	
	insert into dbo.ApplicationSettings (ID, SettingKey, SettingValue, IDDivision)
		values(newid(), 'AllocationReportFooter', 'Allocation Report Footer Sample', @divisionID)

	insert into dbo.ApplicationSettings (ID, SettingKey, SettingValue, IDDivision)
		values(newid(), 'StoreAllocationReportFooter', 'Store Allocation Report Footer Sample', @divisionID)


end