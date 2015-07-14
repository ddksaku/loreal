if object_id('up_recalculateDraftAnimations') > 0
	drop procedure up_recalculateDraftAnimations
go

create procedure up_recalculateDraftAnimations
as
begin
	
	declare @statusDraft int
	set @statusDraft = 3

	declare @animations table
	(
	 animationID uniqueidentifier,
	 iterator INT identity(1,1)
	 )
	 
	 insert into @animations (animationID)
		select ID
			from dbo.Animation
				where [Status] = @statusDraft
				
	declare @animationID uniqueidentifier
	declare @iterator int
	select @iterator = max(iterator) from @animations
	
	while @iterator > 0
	begin
	
		select @animationID = animationID from @animations
			where iterator = @iterator
	
		exec dbo.uf_allocate_animationID @animationID	
	
		set @iterator = @iterator - 1
	
	end

end