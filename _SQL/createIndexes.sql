

IF EXISTS (SELECT name FROM sys.indexes
            WHERE name = N'IX_SalesDate_BrandAxe_Customer')
    DROP INDEX IX_SalesDate_BrandAxe_Customer ON Sale;

CREATE NONCLUSTERED INDEX IX_SalesDate_BrandAxe_Customer
ON dbo.Sale(IDCustomer, IDBrandAxe, Date)


IF EXISTS (SELECT name FROM sys.indexes
            WHERE name = N'IX_Customer_AnimationType_ItemType_Priority')
    DROP INDEX IX_Customer_AnimationType_ItemType_Priority ON CustomerCapacity;

CREATE NONCLUSTERED INDEX IX_Customer_AnimationType_ItemType_Priority
ON dbo.CustomerCapacity(IDCustomer, IDAnimationType, IDPriority, IDItemType)



IF EXISTS (SELECT name FROM sys.indexes
            WHERE name = N'IX_Customer_AnimationDetail')
    DROP INDEX IX_Customer_AnimationDetail ON CustomerAllocation;

CREATE NONCLUSTERED INDEX IX_Customer_AnimationDetail
ON dbo.CustomerAllocation(IDCustomer, IDAnimationProductDetail)

