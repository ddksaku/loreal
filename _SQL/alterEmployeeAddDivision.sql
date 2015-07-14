alter table dbo.SalesEmployee
add IDDivision uniqueidentifier
go

alter table dbo.SalesEmployee
add constraint FK_SalesEmployee_Division FOREIGN KEY (IDDivision)
REFERENCES Division (ID)

UPDATE dbo.SalesEmployee set IDDivision = (select top 1 ID from Division)
go

alter table dbo.SalesEmployee
alter column IDDivision uniqueidentifier not null
