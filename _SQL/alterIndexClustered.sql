
-- ** SALE
alter table dbo.Sale
add intID int IDENTITY(1,1)

alter table dbo.Sale
drop constraint PK_Sale

alter table dbo.Sale
add constraint PK_Sale
PRIMARY KEY NONCLUSTERED (ID)

create clustered index IX_Clustered 
on dbo.Sale (intID)

ALTER INDEX IX_Clustered ON dbo.Sale
REBUILD;

ALTER INDEX PK_Sale ON dbo.Sale
REBUILD;



-- ** CustomerCapacity
alter table dbo.CustomerCapacity
add intID int IDENTITY(1,1)

alter table dbo.CustomerCapacity
drop constraint PK_CustomerCapacity

alter table dbo.CustomerCapacity
add constraint PK_CustomerCapacity
PRIMARY KEY NONCLUSTERED (ID)

create clustered index IX_CC_Clustered 
on dbo.CustomerCapacity (intID)

ALTER INDEX IX_CC_Clustered ON dbo.CustomerCapacity
REBUILD;

ALTER INDEX PK_CustomerCapacity ON dbo.CustomerCapacity
REBUILD;



-- ** Product

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AnimationProduct_Product]') AND parent_object_id = OBJECT_ID(N'[dbo].[AnimationProduct]'))
ALTER TABLE [dbo].[AnimationProduct] DROP CONSTRAINT [FK_AnimationProduct_Product]


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Multiple_Product]') AND parent_object_id = OBJECT_ID(N'[dbo].[Multiple]'))
ALTER TABLE [dbo].[Multiple] DROP CONSTRAINT [FK_Multiple_Product]

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductConfirmed_Product]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductConfirmed]'))
ALTER TABLE [dbo].[ProductConfirmed] DROP CONSTRAINT [FK_ProductConfirmed_Product]

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductReceived_Product]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductReceived]'))
ALTER TABLE [dbo].[ProductReceived] DROP CONSTRAINT [FK_ProductReceived_Product]


alter table dbo.Product
add intID int IDENTITY(1,1)

alter table dbo.Product
drop constraint PK_Product

alter table dbo.Product
add constraint PK_Product
PRIMARY KEY NONCLUSTERED (ID)

create clustered index IX_P_Clustered 
on dbo.Product (intID)

ALTER INDEX IX_P_Clustered ON dbo.Product
REBUILD;

ALTER INDEX PK_Product ON dbo.Product
REBUILD;




ALTER TABLE [dbo].[AnimationProduct]  WITH CHECK ADD  CONSTRAINT [FK_AnimationProduct_Product] FOREIGN KEY([IDProduct])
REFERENCES [dbo].[Product] ([ID])
GO
ALTER TABLE [dbo].[AnimationProduct] CHECK CONSTRAINT [FK_AnimationProduct_Product]


ALTER TABLE [dbo].[ProductConfirmed]  WITH CHECK ADD  CONSTRAINT [FK_ProductConfirmed_Product] FOREIGN KEY([IDProduct])
REFERENCES [dbo].[Product] ([ID])
GO
ALTER TABLE [dbo].[ProductConfirmed] CHECK CONSTRAINT [FK_ProductConfirmed_Product]


ALTER TABLE [dbo].[ProductReceived]  WITH CHECK ADD  CONSTRAINT [FK_ProductReceived_Product] FOREIGN KEY([IDProduct])
REFERENCES [dbo].[Product] ([ID])
GO
ALTER TABLE [dbo].[ProductReceived] CHECK CONSTRAINT [FK_ProductReceived_Product]


ALTER TABLE [dbo].[Multiple]  WITH CHECK ADD  CONSTRAINT [FK_Multiple_Product] FOREIGN KEY([IDProduct])
REFERENCES [dbo].[Product] ([ID])
GO
ALTER TABLE [dbo].[Multiple] CHECK CONSTRAINT [FK_Multiple_Product]

