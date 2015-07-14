truncate table CustomerCategory
truncate table Sale
truncate table CustomerCapacity
delete from CustomerAllocation
delete from CustomerGroupAllocation
delete from AnimationProductDetail
delete from AnimationProduct
delete from AnimationCustomerGroup
delete from Animation
delete from CustomerBrandExclusion

delete from Customer
delete from CustomerGroupItemType
delete from CustomerGroup
delete from Multiple
delete from ProductReceived
delete from ProductConfirmed
delete from CustomerGroup
delete from BrandAxe
--delete from ItemGroup
--delete from ItemType
--delete from Priority
--delete from SalesArea
--delete from SalesDrive
delete from SalesEmployee
--delete from SalesOrganization
delete from [Signature]
delete from Product
--delete from AnimationType
delete from EventLog
delete from UserRole
--delete from [Role]
delete from [User]
delete from AuditAlert
if object_id('__initialImport01') > 0
begin
	truncate table __initialImport01;
end
if object_id('__initialImport02') > 0
begin
	truncate table __initialImport02;
end
if object_id('__initialImport03') > 0
begin
	truncate table __initialImport03;
end
--delete from Category
--delete from RetailerType
--delete from OrderType
--delete from Division
--delete from DistributionChannel
--delete from ApplicationSettings
delete from allocationLog
--delete from SystemMessage
truncate table HistoryLog