if object_id('up_export_animationParametr') > 0
	drop procedure up_export_animationParametr
go

create procedure up_export_animationParametr
	@salesDrive nvarchar(max)
as

--declare @salesDrive nvarchar(max)
--set @salesDrive = '11111111-2222-3333-4444-555555555555, 69DB9592-88BD-45BB-AC44-0F83D8287B91'




declare @t table
(animations nvarchar(255), ID uniqueidentifier)

insert into @t
select Name, ID from Animation where IDSalesDrive in
(select rtrim(ltrim(Value)) from dbo.uf_split(@salesDrive,','))


if (select count(*) from dbo.uf_split(@salesDrive,',') where Value = '11111111-2222-3333-4444-555555555555' ) > 0
begin
	insert into @t 
		select Name, ID from Animation where IDSalesDrive iS NULL
end
	
select 	* from @t
	
	
