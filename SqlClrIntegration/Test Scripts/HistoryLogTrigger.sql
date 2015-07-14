--INSERT INTO [LorealOptimiseClean].[dbo].[Role]
--           ([Name])
--     VALUES
--           ('xxx')



begin transaction

	DELETE FROM LorealOptimise.[dbo].AnimationCustomerGroup
	WHERE ID = 'E424CB90-F2DF-44F1-A7F5-CE37781A2434'

rollback transaction
