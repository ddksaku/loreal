if object_id('tr_groupItemTypeChanged') > 0
	drop trigger tr_groupItemTypeChanged
go

create trigger tr_groupItemTypeChanged
on dbo.CustomerGroupItemType
after update 
as
begin

		IF @@ROWCOUNT = 0 RETURN 

		declare @oldValue bit
		declare @newValue bit
		declare @itemTypeOld uniqueidentifier
		declare @itemTypeNew uniqueidentifier

		select @oldValue = WarehouseAllocation, @itemTypeOld = IDItemType from deleted
		select @newValue = WarehouseAllocation, @itemTypeNew = IDItemType from inserted
		
		DECLARE @newLineChar char(2)
		SET @newLineChar = CHAR(13) + CHAR(10)

		-- if account is changed from normal to warehouse
		if @newValue > @oldValue OR @itemTypeOld <> @itemTypeNew
		begin

			declare @groupID uniqueidentifier
			select @groupID = IDCustomerGroup from inserted

			declare @itemTypeID uniqueidentifier
			select @itemTypeID = IDItemType from inserted

			declare @errorMsg nvarchar(max)

			-- does exist any active animation, which will be affected?
			declare @animations table
			(   ID uniqueidentifier
			  , animationName nvarchar(255)
			  , productName nvarchar(255)
			  , productCode nvarchar(255)
			 )

			insert into @animations (ID, animationName, productName, productCode)
				select distinct b.ID, b.Name,  p.[Description], p.MaterialCode
					from dbo.AnimationProduct as a
					inner join dbo.Product as p on (a.IDProduct = p.ID)
					inner join dbo.Animation as b on (a.IDAnimation = b.ID)
					inner join dbo.AnimationCustomerGroup as c on (b.ID = c.IDAnimation)
					where  b.[Status] in (1,2,3,4) and c.IDCustomerGroup = @groupID
					--where  b.[Status] in (3,4)
					order by b.Name, p.[Description]	

			declare @count int
			select @count = count(*) from @animations

			if @count > 0
			begin
			
				-- compose message
				declare @animationNames table
				(ID uniqueidentifier, name nvarchar(255), iterator int identity(1,1))
				
				insert into @animationNames (ID, name)
					select distinct ID, animationName
						from @animations
						
				declare @iterator int
				select @iterator = max(iterator) from @animationNames	
				
				declare @aniName nvarchar(255)
				declare @summary nvarchar(max)
				set @summary = ''
				
				while @iterator > 0
				begin
				
					select @aniName = name from @animationNames where iterator = @iterator
				
					set @summary = @summary + @aniName 
					
					select @summary = coalesce(@summary + @newLineChar , '') + '   ' + productName + ' (' + productCode + ')'
						FROM @animations where animationName = @aniName
					
					set @summary = @summary + @newLineChar + @newLineChar
					set @iterator = @iterator - 1
					
				end				

				
				SET @errorMsg = dbo.uf_getSystemMessage('tr_CustomerGroupItemType',null, null , null, null, null, null, null, null )
				set @errorMsg = @errorMsg + @newLineChar + @newLineChar + @summary
				RAISERROR (@errorMsg, 16, 29)
				ROLLBACK TRAN
				RETURN	

			end
	 

		end

end