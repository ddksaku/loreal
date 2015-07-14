drop table SystemMessage

create table SystemMessage
(
	ID uniqueidentifier,
	Code nvarchar(100),
	Name nvarchar(255),
	MessageContent nvarchar(max),
	MessageDescription nvarchar(max)
)	

alter table dbo.SystemMessage
add constraint U_code UNIQUE (Code)

insert into SystemMessage
	values (newid(), 'tr_AnimationProduct_AllocationLogicChanged01', 'Trigger Alert - Animation Product - Allocation Logic Changed (ItemType)'
		,'Animation allocation logic changed in allocate by item type &newLineChar Animation: &p1  ( &p2 ) &newLineChar Product: &p3 ( &p4  )'
		,'&p1 = Animation Name, &p2 = AnimationCode, &p3 = Product Name, &p4 = Product Code')		
		
insert into dbo.SystemMessage
	values (newid(), 'tr_AnimationProduct_AllocationLogicChanged02', 'Trigger Alert - Animation Product - Allocation Logic Changed (Signature)'		
		,'Animation allocation logic changed in allocate by signature &newLineChar Animation: &p1  ( &p2 ) &newLineChar Product: &p3 ( &p4  )'
		,'&p1 = Animation Name, &p2 = AnimationCode, &p3 = Product Name, &p4 = Product Code')
		
insert into dbo.SystemMessage
	values (newid(), 'tr_AnimationProduct_AllocationLogicChanged03', 'Trigger Alert - Animation Product - Allocation Logic Changed (Brand/Axe)'		
		,'Animation allocation logic changed in allocate by brand/Axe &newLineChar Animation: &p1  ( &p2 ) &newLineChar Product: &p3 ( &p4  )'
		,'&p1 = Animation Name, &p2 = AnimationCode, &p3 = Product Name, &p4 = Product Code')		
		
insert into dbo.SystemMessage
	values (newid(), 'tr_AnimationProduct_AllocationLogicChanged04', 'Trigger Alert - Animation Product - Allocation Logic Changed (Category)'		
		,'Animation allocation logic changed in allocate by category &newLineChar Animation: &p1  ( &p2 ) &newLineChar Product: &p3 ( &p4  )'
		,'&p1 = Animation Name, &p2 = AnimationCode, &p3 = Product Name, &p4 = Product Code')	
		
insert into dbo.SystemMessage
	values (newid(), 'tr_AnimationProductDetail_AllocationQuantity', 'Trigger - Animation Product Detail - check quantity against capacity '
		,'Allocation quantity exceeds maximum capacity &newLineChar  Quantity: &p1  &newLineChar  Capacity: &p2'
		,'&p1 = Allocation quantity, &p2 = capacity within stores')		
			
			
insert into dbo.SystemMessage
	values (newid(), 'tr_AnimationProductDetail_AllocationQuantityAlert', 'Trigger Alert - Animation Product Detail - allocation qunatity changed',
			'Allocation quantity has been changed &newLineChar Animation:  &p1  ( &p2 ) &newLineChar Product: &p3 (&p4)' 		
							,'&p1 = Animation name, &p2 = Animation code, &p3 = Product name, &p4 = Product Code')
							
insert into dbo.SystemMessage
	values (newid(), 'tr_AnimationProductDetail_CheckQuantityAgainstCustomerFixed', 'Trigger - Animation Product Detail - check quantity against sum of customer fixed'
			,'Sum of customer fixed allocations exceeds allocation quantity &newLineChar Quantity: &p1  &newLineChar  Sum of Fixed: &p2'
			 ,'&p1 = allocation quantity, &p2 = sum of customer''s fixed')
			 
insert into dbo.SystemMessage
	values (newid(), 'tr_AnimationProductDetail_CheckQuantityAgainstSystemFixed', 'Trigger - Animation Product Detail - check quantity against sum of group fixed'	
			,'Sum of group fixed allocations exceeds allocation quantity &newLineChar Quantity: &p1 &newLineChar Sum of Fixed: &p2'
			,'&p1 = allocation quantity, &p2 = sum of group fixed')		 
			
insert into dbo.SystemMessage
	values (newid(), 'tr_AnimationProductDetail_DivisibilityByMultiple', 'Trigger - Animation Product Detail - check divisibility by multiple',
			'Allocation quantity is not divisible by product multiple &newLineChar  Quantity: &p1  &newLineChar  Multiple: &p2'
			,'&p1 = allocation quantity, &p2 = multiple')		
			
insert into dbo.SystemMessage
	values (newid(),  'tr_Customer_ClosedCustomer', 'Trigger Alert - Closed store',
			'There is a newly closed store (customer). &newLineChar Customer: &p1 (&p2)',
			'&p1 = Customer Name, &p2 = Customer Accout Code')	
			
insert into dbo.SystemMessage
	values (newid(), 'tr_customerAllocation_avoidDuplicity', 'Trigger - Customer already on allocation'	,
			'This Customer already exists within this allocation.',
			'')	
			
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerAllocation_CheckAgainstGroupFixed01', 'Trigger - Customer Allocation - check customer fixed against group manual fixed'
				,'Fixed allocation exceeds customer group manual fixed allocation &newLineChar  Fixed: &p1 &newLineChar Manual Fixed CG: &p2'
				,'&p1 = Customer fixed, &p2 = customer group manual fixed')	
				
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerAllocation_CheckAgainstGroupFixed02', 'Trigger - Customer Allocation - check customer fixed against group system fixed'
				,'Fixed allocation exceeds customer group system fixed allocation &newLineChar  Fixed: &p1 &newLineChar System Fixed CG: &p2'
				,'&p1 = Customer fixed, &p2 = customer group manual fixed')		
				
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerAllocation_CheckFixedAgainstAllocationQuantity', 'Trigger - Customer Allocation - check sum of fixed against allocation quantity'
			,'Sum of fixed allocations exceeds allocation quantity &newLineChar Quantity: &p1 &newLineChar Sum of Fixed: &p2 '
			,'&p1 = Allocation Quantity, &p2 = sum of fixed')	
			
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerAllocation_CheckFixedAgainstCapacity', 'Trigger - Customer Allocation - check sum of fixed against sum of capacity'	
			,'Sum of fixed allocations exceeds maximum capacity &newLineChar Capacity: &p1  &newLineChar Sum of Fixed: &p2'
			,'&p1 = sum of capacity, &p2 = sum of fixed'	)		
			
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerAllocation_CheckFixedAgainstCustomerCapacity', 'Trigger - Customer Allocation - check fixed against capacity'		
			,'Customer fixed allocation exceeds its capacity &newLineChar Capacity: &p1 &newLineChar  Fixed: &p2 '
			,'&p1 = customer capacity, &p2 = fixed allocation' )
			
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerGroupAllocation_CheckGroupAgainstCustomerFixedSum', 'Trigger - Customer Allocation - check sum of customer fixed against group system fixed'
			,'Sum of customer''s Fixed allocations exceeds customer group manual fixed allocation &newLineChar Sum of Fixed: &p1 &newLineChar Manual Fixed CG: &p2'
			,'&p1 = sum of customer''s fixec, &p2 = group manual fixed')	
			
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerAllocation_DivisibilityOfFixedAllocation', 'Trigger - Customer Allocation - divisibility of fixed',
	        'Fixed quantity is not divisible by product multiple &newLineChar Fixed: &p1 &newLineChar Multiple: &p2'
	        ,'&p1 = fixed allocation, &p2 = multiple') 
	        
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerAllocation_RetailOrFixedOnly', 'Trigger - Customer Allocation - retail or fixed only'
			, 'Just one from Retail Uplift and Fixed Allocation can be filled. &newLineChar'
			,'')
			
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerCapacity_CapacityChanged', 'Trigger Alert - Customer Capacity - capacity changed',
			'Customer capacity changed on following animation: &newLineChar Animation: &p1 (&p2) &newLineChar Customer: &p3 (&p4) &newLineChar Priority: &p5 &newLineChar Item Type: &p6 &newLineChar Animation Type: &p7'
			,'&p1 = Animation Name, &p2 = Animation Code, &p3 = customer name, &p4 = customer Code, &p5 = priority, &p6 = item type, &p7 = animation Type')				   			
			
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerCapacity_CapacityDecreased01', 'Trigger - Customer Capacity - capacity decreased - check fixed against new capacity'
			,'Fixed allocation exceeds new capacity &newLineChar Fixed allocation:  &p1 &newLineChar  Capacity:  &p2'
			,'&p1 = fixed allocation, &p2 = capacity')
			
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerCapacity_CapacityDecreased02', 'Trigger - Customer Capacity - capacity decreased - check group fixed against capacity'
			,'Fixed allocation exceeds customer group capacity &newLineChar System Fixed allocation: &p1 &newLineChar Capacity: &p2'
			,'&p1 = group system fixed, &p2 = capacity')
			
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerCapacity_CapacityDecreased03', 'Trigger - Customer Capacity - capacity decreased - check group fixed against capacity'
			,'Fixed allocation exceeds customer group capacity &newLineChar Manual Fixed allocation: &p1 &newLineChar Capacity: &p2'
			,'&p1 = group manul fixed, &p2 = capacity')		
			
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerCapacity_CapacityDecreased04', 'Trigger - Customer Capacity - capacity decreased - check quantity against capacity'
			,'Allocation quantity exceeds customer group capacity &newLineChar Allocation quantity: &p1 &newLineChar  Capacity: &p2 '	
			,'&p1 = allocation quantity, &p2 = capacity')			
			
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerGroupAllocation_DivisibilityOfFixedAllocation', 'Trigger - Group Allocation - divisibility of fixed allocation'
			,'Fixed quantity is not divisible by product multiple &newLineChar Fixed: &p1 &newLineChar Multiple: &p2'
			,'&p1 = fixed quantity, &p2 = multiple')	
			
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerGroupAllocation_CheckFixedAgainstAllocationQuantity', 'Trigger - Group Allocation - check fixed against allocation quantity'
			,'Sum of group fixed allocations exceeds allocation quantity &newLineChar Quantity: &p1 &newLineChar Sum of Fixed: &p2'
			,'&p1 = allocation qunatity, &p2 = sum of fixed')
			
			
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerGroupAllocation_CheckFixedAgainstCapacity', 'Trigger - Group Allocation - check fixed against overall capacity',
			'Sum of group fixed allocations exceeds maximum capacity &newLineChar Capacity: &p1 &newLineChar Sum of Fixed: &p2'
			,'&p1 = capacity, &p2 = sum of group fixed')
			
									
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerGroupAllocation_CheckFixedAgainstGroupCapacity', 'Trigger - Group Allocation - check fixed against group capacity'
			,'Group fixed allocation exceeds maximum group capacity &newLineChar Capacity: &p1 &newLineChar  Fixed: &p2'
			,'&p1 = group capacity, &p2 = fixed')			
			
									
insert into dbo.SystemMessage
	values (newid(), 'tr_CustomerGroupAllocation_RetailOrFixedOnly', 'Trigger - Group Allocation - just retail or fixed'
			,'Just one from Retail Uplift and Fixed Allocation can be filled. &newLineChar',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 'tr_Multiple_MultipleChanged', 'Trigger Alert - Multiple - multiple changed',
			'Product multiple changed on product existing on an active animation. &newLineChar Product code: &p1 &newLineChar Animation(s): &p2'
			,'&p1 = Product code, &p2 = animations List')
			
insert into dbo.SystemMessage
	values (newid(), 'tr_multiple_update_checkDuplicity', 'Trigger - Multiple - check duplicity',			
			'There already exists such multiple',
			'')
			
insert into dbo.SystemMessage
	values (newid(), 'tr_Product_Dead', 'Trigger Alert - Product - Product Dead',
			'There is a product on this animation which status has been set to dead &newlineChar Animation: &p1 (&p2) &newLineChar Product: &p3 (&p4)'
			,'&p1 = Animation Name, &p2 = Animation Code, &p3 = Product Name, &p4 = Product Code')
			
			
			
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationDataMissing',
			'Animation Data - Mandatory data missing',
			'Mandatory data is missing for animation product',
			'')				
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationProductMissing',
			'Animation - Product Missing',
			'There is no animation product.',
			'')	
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationAnimationTypeMissing',
			'Animation - Animation type missing',
			'Animation Type is missing',
			'')	
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationPriorityMissing',			
			'Animation - Priority missing',
			'Priority is missing.',
			'')
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationClosedMandatory',
			'Animation Closed - Mandatory data missing',
			'Mandatory data is missing for animation.',
			'')	

insert into dbo.SystemMessage
	values (newid(), 
			'AnimationDummyProducts',
			'Animation - Dummy products',
			'Animation contains some dummy products.',
			'')	

insert into dbo.SystemMessage
	values (newid(), 
			'AnimationProductMandatory',
			'Animation Product - Mandatory data missing',
			'Mandatory data is missing for animation product',
			'')	

insert into dbo.SystemMessage
	values (newid(), 
			'AnimationNoProduct',
			'Animation - No product on animation',
			'Animation product details are missing for some animation products.',
			'')

insert into dbo.SystemMessage
	values (newid(), 
			'AnimationDetailMandatory',
			'Animation - Detail mandatory data missing',
			'Mandatory data is missing for animation product detail',
			'')	
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationGroupAllocationMissing',
			'Animation - Group fro allocation missing',
			'Customer Group is missing for fixed customer group''s allocation.',
			'')	
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationCustomerAllocationMissing',
			'Animation - Customer for allocation missing',
			'Customer is missing for fixed customer''s allocation.',
			'')			
			
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationCustomerGroupGroupRemove',
			'Animation Customer Group - Group remove',
			'This Customer Group cannot be removed because the allocation quantity of some animation products will exceed the available capacity.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationProductEmptyItemGroup',
			'Animation Product - Empty item group',
			'Item Group cannot be empty.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationProductEmptyProduct',
			'Animation Product - Empty product',
			'Product cannot be empty.',
			'')			
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationTypeDelete',
			'Animation Type - Delete',
			'This Animation Type cannot be deleted because it is attached to some animations.',
			'')			
			
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationTypeDeleteWarning',
			'Animation Type - Delete Warning',
			'If you delete this animation type, then all capacities for this animation type will also be deleted. Do you want to proceed?',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'CategoryDelete',
			'Category - Delete',
			'This Category cannot be deleted because it is attached to some animations.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerDelete',
			'Customer - Delete',
			'This customer store cannot be deleted because it is not a dummy store.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerDeleteAttached',
			'Customer - Delete customer attached to animation',
			'This customer store cannot be deleted because it is attached to some animations.',
			'')			
			
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerDeleteWarning',
			'Customer - Delete warning',
			'If you delete this Customer Sotre, then all its sales & capacities data will also be deleted. Do you want to proceed?',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerAllocationFixedOrRetail',
			'Customer Allocation - Fixed or retail',
			'Please fill either fixed allocation or retail uplift. Both values can not be filled',
			'')			
			
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerCapacityDelete',
			'Customer Capacity - Delete',
			'Any Customer Capacity data cannot be deleted. The only way to exclude the records is be to update the capacity value manually to 0.',
			'')			
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerGroupDelete',
			'Customer Group - Delete',
			'This customer group cannot be deleted because it is not manual.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerGroupDeleteAttached',
			'Customer Group - Delete group attached to animation',
			'This customer group cannot be deleted because it is attached to some animations.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerGroupDeleteWithCustomers',
			'Customer Group - Delete with customers',
			'This customer group cannot be deleted because it has some customer stores which cannot be deleted.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationAllocationQuantityMissing',
			'Animation - Allocation quantity missing',
			'Allocation quantity is missing for some animation products.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerGroupAllocationFixedOrRetail',
			'Customer Group Allocation - Fixed or retail',
			'Please fill either fixed allocation or retail uplift. Both values can not be filled',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerGroupItemTypeDelete',
			'Customer Group Item Type - Delete',
			'This Customer Group Item Type cannot be deleted because it is attached to some animations.',
			'')			
			
insert into dbo.SystemMessage
	values (newid(), 
			'ItemGroupDelete',
			'Item Group - Delete',
			'This Item Group cannot be deleted because it is attached to some animations.',
			'')			
			
insert into dbo.SystemMessage
	values (newid(), 
			'ItemTypeDelete',
			'Item Type - Delete',
			'This Item Type cannot be deleted because it is attached to some animations.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'ItemTypeDeleteWarning',
			'Item Type - Delete warning',
			'If you delete this item type, then all capacities for this item type will also be deleted. Do you want to proceed?',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'OrderTypeDelete',
			'Order Type - Delete',
			'This Order Type cannot be deleted because it is attached to some animations.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'PriorityDelete',
			'Priority - Delete',
			'This Animation Priority cannot be deleted because it is attached to some animations.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'PriorityDeleteWarning',
			'Priority - Delete warning',
			'If you delete this priority, then all capacities for this priority will also be deleted. Do you want to proceed?',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'ProductDelete',
			'Product - Delete',
			'This product cannot be deleted because it is not a dummy product or it is currently being used in at least one animation.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'RetailerTypeDelete',
			'Retailer Type - Delete',
			'This Retailer Type cannot be deleted because it is attached to some animations.',
			'')			
			
insert into dbo.SystemMessage
	values (newid(), 
			'SaleDelete',
			'Sale - Delete',
			'Any sale data cannot be deleted. The only way to exclude sales is to update the sales value manually to 0 or exclude sales data for certain Brands/Axes using the Store Brand Exclusion window',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'SaleDriveDelete',
			'Sale Drive - Delete',
			'This Sales Drive cannot be deleted because it is attached to some animations.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'SalesOrganizationDelete',
			'Sales Organization - Delete',
			'This Sales Organization cannot be deleted it has active animations.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'SignatureDelete',
			'Signature - Delete',
			'This Signature cannot be deleted because it is attached to some animations.',
			'')		
		
insert into dbo.SystemMessage
	values (newid(), 
			'ProductAnimationDelete',
			'Product Animation - Delete',
			'Animation products can be deleted only by Markeing or Division Administrator',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'ProductAnimationDeleteWithAllocation',
			'Product Animation - Delete with allocation',
			'This animation product has some allocations. Do you really want to delete it?',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'ProductAnimationDeleteWithStatus',
			'Product Animation - Delete with status',
			'Animation products cannot be deleted because of its Animation status.',
			'')			
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationListDeleteNoAdmin',
			'Animation List - Delete no admin',
			'Animation can be deleted only by Division Administrator.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationListDeleteClearedClosed',
			'Animation List - Delete cleared or closed',
			'Closed or cleared animations cannot be deleted.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'ReplaceAccounNumberSelectStore',
			'Replace Accoun Number - Select store',
			'Select a store to replace from',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'ReplaceAccountNumberReplaced',
			'Replace Account Number - Number replaced',
			'Successfully replaced the dummy store with a SAP store',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'UserDelete',
			'User - Delete',
			'Do you really want to delete this user?',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationCreated',
			'Animation - Created',
			'Animation was successfully created.',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'AnimationUpdated',
			'Animation - Updated',
			'Animation was successfully updated',
			'')			
	
	
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerGroupManagerErrorDeleting',
			'Customer Group Manager - Error on deleting',
			'An error occured when deleting CustomerGroup: &p1',
			'&p1 = exception message')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'AppErrorRestart',
			'Application - Error, need to restart',
			'An error has occured in application. &newLineChar  &p1 You may need to restart the application"',
			'&p1 = error message')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'AppError',
			'Application - Error',
			'An error has occured in application.&newLineChar &p1',
			'&p1 = error message')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'RunAllocationOrderCreated',
			'Ru nAllocation - Order already created',
			'SAP order has already been created, do you really want to re-run allocation?',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'ProductAnimationUpdateException',
			'Product Animation - Update exception',
			'An error occured when inserting the product: &p1',
			'&p1 = exception')	
			
				
			
insert into dbo.SystemMessage
	values (newid(), 
			'ReplaceAccountNumberException',
			'Replace Account Number - Exception',
			'An error occured when replacing the dummy store with a SAP store: &p1',
			'&p1 = exception')
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerGroupExceptionNewRow',
			'Customer Group - Exception init new row',
			'An error occured when setting ''SortOrder'' for a new row: &p1',
			'&p1 = exception')
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerStoreExceptionNewRow',
			'Customer Store - Exception init new row',
			'An error occured when initialzing values for a new row: &p1',
			'&p1 = exception')
			
insert into dbo.SystemMessage
	values (newid(), 
			'CustomerStoreExceptionUpdate',
			'Customer Store - Exception on update',
			'An error occured when updating Store Categories: &p1',
			'&p1 = exception')
			
insert into dbo.SystemMessage
	values (newid(), 
			'SalesDriveExceptionNewRow',
			'Sales Drive - Exception init new row',
			'An error occured when setting ''Year'' value for a new row: &p1',
			'&p1 = exception')												
			
insert into dbo.SystemMessage
	values (newid(), 
			'TableViewExceptionNewRow',
			'Table View - Exception init new row',
			'An error occured when setting ''IDDivision'' value for a new row: &p1',
			'&p1 = exception')
			
insert into dbo.SystemMessage
	values (newid(), 
			'TableViewExceptionSql',
			'Table View - Exception Sql',
			'An error occured when inserting an entity: &p1',
			'&p1 = exception')
			
insert into dbo.SystemMessage
	values (newid(), 
			'ProductAnimationProductNotFound',
			'Product Animation - Product not found',
			'No product exists for material code &p1',
			'&p1 = material code')
			
insert into dbo.SystemMessage
	values (newid(), 
			'ProductAnimationDescriptionNotFound',
			'Product Animation - Description not found',
			'No product exists for description  &p1',
			'&p1 = product description')
	
	
insert into dbo.SystemMessage
	values (newid(), 
			'tr_AnimationCapacityChangedCustomer',
			'Animation - Capacity Changed, Customer',
			'Fixed allocation exceeds customer capacity  &newLineChar  Customer: &p1 &newLineChar Fixed: &p2 &newLineChar Capacity: &p3',
			'&p1 = customer name, &p2 = fixed allocation, &p3 = capacity')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'tr_AnimationCapacityChangedGroupSystem',
			'Animation - Capacity Changed, Group System Fixed',
			'System fixed allocation exceeds group capacity &newLineChar Group: &p1 &newLineChar Fixed: &p2 &newLineChar Capacity: &p3',
			'&p1 = group name, &p2 = system fixed, &p3 = group capacity')
			
insert into dbo.SystemMessage
	values (newid(), 
			'tr_AnimationCapacityChangedGroupManual',
			'Animation - Capacity Changed, Group Manual Fixed',
			'Manual fixed allocation exceeds group capacity &newLineChar Group: &p1 &newLineChar Fixed: &p2 &newLineChar Capacity: &p3',
			'&p1 = group name, &p2 = manual fixed, &p3 = group capacity')
			
insert into dbo.SystemMessage
	values (newid(), 
			'tr_AnimationCapacityChangedAllocation',
			'Animation - Capacity Changed, Allocation',
			'Allocation quantity exceeds overall capacity &newLineChar Product: &p1 Allocation Quantity: &p2 &newLineChar Capacity: &p3',
			'&p1 = product name, &p2 = allocation quantity, &p3 = overall capacity')
	
		
insert into dbo.SystemMessage
	values (newid(), 
			'ProductConfirmationImportAlertProductNotFound',
			'Product Confirmation Import - Alert product not found',
			'Product from imported confirmation file was not found &newLineChar Product Code:  &p1 File name: &p2',
			'&p1 = product code, &p2 = file name')
	
			
/*insert into dbo.SystemMessage
	values (newid(), 
			'tr_CustomerGroupItemType',
			'Customer Group Item Type - Change account type',
			'You can''t switch account to warehouse, because there exists an active animation including this group',
			'')		*/
			
insert into dbo.SystemMessage
	values (newid(), 
			'tr_CustomerGroupItemType',
			'Customer Group Item Type - Change account type',
			'Warning: You may wish to fix customer allocations as these could change significantly next time the allocation is run.',
			'')										
			
insert into dbo.SystemMessage
	values (newid(), 
			'tr_animationProduct_multipleChange',
			'Trigger - Animation Product - Multiple Changed',
			'You can''t change multiple because there is a product on the animation whose allocation depends on multiple. First blank multiple, change allocation and fixed quantities.',
			'')
		
insert into dbo.SystemMessage
	values (newid(), 
			'tr_AnimationProduct_NormalMultipleChangedAlert',
			'Trigger - Animation Product - Normal Multiple Changed Alert',
			'Product normal multiple changed on product existing on an active animation. &newLineChar Product code: &p1 &newLineChar Animation: &p2',
			'&p1 = Product Code, &p2 = Animation Name')
		
insert into dbo.SystemMessage
	values (newid(), 
			'tr_AnimationProduct_WarehouseMultipleChangedAlert',
			'Trigger - Animation Product - Warehouse Multiple Changed Alert',
			'Product warehouse multiple changed on product existing on an active animation. &newLineChar Product code: &p1 &newLineChar Animation: &p2',
			'&p1 = Product Code, &p2 = Animation Name')
			
insert into dbo.SystemMessage
	values (newid(), 
			'tr_AnimationGroupCheckGroupDuplicity',
			'Trigger - Animation Customer Group - check duplicity',
			'There is already such Customer group at this animation',
			'')
			
insert into dbo.SystemMessage
	values (newid(), 
			'CloseAnimationWithSAPOrder',
			'Animation - close with SAP order',
			'SAP Order for this animation was already created, do you want to recreate it?',
			'')		
			
insert into dbo.SystemMessage
	values (newid(), 
			'tr_Multiple_ValueChanged_DivisibilityOfAllocationQuantity',
			'Multiple - Some allocation quantity not divisible by multiple',
			'There is at least one allocation quantity which is not divisible by new multiple value.',
			'')
			
insert into dbo.SystemMessage
	values (newid(), 
			'tr_Multiple_ValueChanged_DivisibilityOfGroupFixed',
			'Multiple - Some group fixed not divisible by multiple',
			'There is at least one customer group fixed allocation which is not divisible by new multiple value.',
			'')
			
insert into dbo.SystemMessage
	values (newid(), 
			'tr_Multiple_ValueChanged_DivisibilityOfCustomerFixed',
			'Multiple - Some customer fixed not divisible by multiple',
			'There is at least one customer fixed allocation which is not divisible by new multiple value.',
			'')
		
insert into dbo.SystemMessage
	values (newid(), 
			'tr_AnimationProduct_DivisibilityOfWarehouseMultipleByNormalMultiple',
			'Multiple - warehouse multiple not divisible by normal multiple.',
			'Warehouse multiple must be divisible by normal multiple.',
			'')
	
/*			
insert into dbo.SystemMessage
	values (newid(), 
			'',
			'',
			'',
			'')								
					
*/																																											