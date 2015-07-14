DECLARE @HistoryLogTable VARCHAR(MAX)
DECLARE @delimiter VARCHAR(MAX)

SET @HistoryLogTable = 'Animation, 
      AnimationCustomerGroup,
      AnimationProduct,
      AnimationProductDetail,
      AnimationType,
      BrandAxe,
      Category,
      Customer,
      CustomerAllocation,
      CustomerBrandExclusion,
      CustomerCapacity,
      CustomerCategory,
      CustomerGroup,
      CustomerGroupAllocation,
      CustomerGroupItemType,
      DistributionChannel,
      Division,
      ItemGroup,
      ItemType,
      Multiple,
      OrderType,
      Priority,
      Product,
      ProductConfirmed,
      ProductReceived,
      RetailerType,
      Role,
      Sale,
      SalesArea,
      SalesDrive,
      SalesEmployee,
      SalesOrganization,
      Signature,
      User,
      UserRole,
      Version';
 
DECLARE @length int, @currPos int, @prevPos int
DECLARE @tableName varchar(50) 
DECLARE @NewLine char(2)
SET @NewLine=char(13)+char(10)
SET @length = LEN(@HistoryLogTable) + 1
  
BEGIN
   SET @currPos = 1
   SET @prevPos = @currPos

   WHILE @currPos < @length + 1
   BEGIN
  SET @delimiter = LTRIM(RTRIM(SUBSTRING(@HistoryLogTable + ',', @currPos, 1)));   
   IF @delimiter = ',' 
   BEGIN
  SET @tableName=REPLACE(SUBSTRING(@HistoryLogTable, @prevPos, @currPos - @prevPos), @NewLine, '')
  SET @tableName = REPLACE(@tableName, CHAR(9),'')
  SET @tableName = LTRIM(RTRIM(@tableName))   
  PRINT('Processing table '+@tableName);   
   BEGIN    

  EXEC ('
   IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(''Audit'+@tableName+'''))
   DROP TRIGGER Audit'+@tableName+'')

   exec ('create trigger Audit'+@tableName+'
   on ['+@tableName+'] for insert, update, delete
   as external name [SqlClrIntegration].[Triggers].[HistoryLogTrigger]')
   END
 
  SET @prevPos = @currPos + 1
  END
  SET @currPos = @currPos + 1
   END
END