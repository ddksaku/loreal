USE [LorealOptimiseClean]
GO
/****** Object:  StoredProcedure [dbo].[up_recreateAllocations]    Script Date: 06/24/2010 16:02:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[up_recreateAllocations]
	@string nvarchar(max)	

as

begin

	declare @animationProductDetail table
	( 
	 ID uniqueidentifier,
	 iterator int IDENTITY(1,1)
	 )
	 
	 insert into @animationProductDetail (ID)
		select cast(Ltrim(rtrim(Value)) as uniqueidentifier) from dbo.uf_split(@string, ',')	
		
	declare @iterator int
	set @iterator =	(select count(*) from @animationProductDetail)
	
	 if @iterator = 0
		return -- nothing happended
		
	 declare @animationProductID uniqueidentifier
	 declare @animationID uniqueidentifier
	 declare @animationProductDetailID uniqueidentifier
	 declare @brandAxe uniqueidentifier
	 declare @signature uniqueidentifier
		
	 while @iterator > 0
	 begin
		set @brandAxe = null
	 
		select @animationProductDetailID = ID from @animationProductDetail
			where iterator = @iterator				
				
		 
		 select @animationProductID = IDAnimationProduct from dbo.AnimationProductDetail
			where ID = @animationProductDetailID
			
		 select @animationID = IDAnimation, @brandAxe = IDBrandAxe, @signature = IDSignature from dbo.AnimationProduct 
			where ID = @animationProductID	
			
		 declare @storeAllocationsToInsert table
		 (	customerID uniqueidentifier, animationProductDetailID uniqueidentifier )
		 
		 insert into @storeAllocationsToInsert
			select distinct a.ID,  @animationProductDetailID from dbo.Customer as a 
				inner join dbo.CustomerGroup as b on (a.IDCustomerGroup = b.ID)
				inner join dbo.AnimationCustomerGroup as c on (b.ID = c.IDCustomerGroup)
				where 
				c.IDAnimation = @animationID and a.Deleted = 0 and a.IncludeInSystem = 1 				
				AND (EXISTS (SELECT s.ID FROM Sale s 
					 LEFT JOIN BrandAxe ba ON ba.ID = s.IDBrandAxe
					 WHERE  s.IDCustomer = a.ID
					 AND ((@brandAxe IS NOT NULL AND IDBrandAxe = @brandAxe) OR (@brandAxe IS NULL AND ba.IDSignature = @signature))
					 AND s.[Date] > DATEADD(year,-1,GETDATE())
				))
			except
			select IDCustomer, IDAnimationProductDetail
			from dbo.CustomerAllocation	
				
				
		 -- delete brandexclusions
		 if @brandAxe is not null
		 begin
		 
			delete from @storeAllocationsToInsert
				where exists (select ID from dbo.CustomerBrandExclusion where IDBrandAxe = @brandAxe
								and IDCustomer = customerID and Excluded = 1)
			
		 end		
			
		  -- insert new allocations
		  INSERT INTO dbo.CustomerAllocation (IDCustomer, IDAnimationProductDetail, ModifiedBy, ModifiedDate)
			select customerID, animationProductDetailID, 'Automatically created', getdate()
				from @storeAllocationsToInsert
			
			
				
		  declare @groupAllocationsToInsert table
		  ( groupID uniqueidentifier, animationProductDetailID uniqueidentifier)
		  
		  insert into @groupAllocationsToInsert	
			select distinct b.ID, @animationProductDetailID from dbo.CustomerGroup as b 
				inner join dbo.AnimationCustomerGroup as c on (b.ID = c.IDCustomerGroup)
				where c.IDAnimation = @animationID 
				AND exists (select cus.ID from dbo.Customer as cus
							where cus.IDCustomerGroup = c.IDCustomerGroup
							AND	(EXISTS (SELECT s.ID FROM Sale s 
								 LEFT JOIN BrandAxe ba ON ba.ID = s.IDBrandAxe
							     WHERE  s.IDCustomer = cus.ID
							     AND ((@brandAxe IS NOT NULL AND IDBrandAxe = @brandAxe) OR (@brandAxe IS NULL AND ba.IDSignature = @signature))
							     AND s.[Date] > DATEADD(year,-1,GETDATE())
								))
							) 
			except
			select IDCustomerGroup, IDAnimationProductDetail
				from dbo.CustomerGroupAllocation		
					
		  -- insert group allocation 
		  INSERT INTO dbo.CustomerGroupAllocation (IDCustomerGroup, IDAnimationProductDetail, ModifiedBy, ModifiedDate)
			select groupID, @animationProductDetailID, 'Automatically created', getdate()
				from @groupAllocationsToInsert	
				
				
	 
			set @iterator = @iterator - 1
			delete from @groupAllocationsToInsert
			delete from @storeAllocationsToInsert
	 end
	 
end
