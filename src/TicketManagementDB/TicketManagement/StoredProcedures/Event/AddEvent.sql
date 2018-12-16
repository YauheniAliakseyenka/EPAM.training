CREATE PROCEDURE [dbo].[AddEvent]
	@Name nvarchar(120),
	@Description nvarchar(MAX),
	@LayoutId int,
	@Date datetime,
	@EventInsertedID int = 0,
	@layoutChanged int = 0
AS
BEGIN TRAN

BEGIN TRY
	declare @AreaCount int
	declare @Seats int

	select @Seats = count(*)
		from
		(
		select Seat.AreaId from Area
		join Seat on Seat.AreaId = Area.Id
		where Area.LayoutId = 1
		group by Seat.AreaId
		) t

	select @AreaCount = Count(Area.Id) from Area
		where Area.LayoutId =1

	if @AreaCount ! = @Seats
		THROW 51000, 'Attempt to create event without seats', 1

	IF @layoutChanged = 0 
		BEGIN
			INSERT INTO [Event] VALUES(@Name, @Description, @LayoutId, @Date)
			SET @EventInsertedID = SCOPE_IDENTITY()
		END	

	BEGIN
		INSERT INTO [EventArea] 
		(
		[EventArea].[EventId], 
		[EventArea].[Description], 
		[EventArea].[CoordX], 
		[EventArea].[CoordY], 
		[EventArea].[Price],
		[EventArea].[AreaDefaultId]
		)
		(SELECT @EventInsertedID, [Area].[Description], [Area].[CoordX], [Area].[CoordY], '0', [Area].[Id] FROM [Area]
		WHERE [Area].[LayoutId] = @LayoutId)
	END

	BEGIN
		INSERT INTO [EventSeat]
		(
		[EventSeat].[EventAreaId],
		[EventSeat].[Number],
		[EventSeat].[Row],
		[EventSeat].[State]
		)
		(SELECT [EventArea].[Id], [Seat].[Number], [Seat].[Row], '0' FROM [Seat]
		join [EventArea] on [EventArea].[AreaDefaultId] = [Seat].[AreaId]
		WHERE [EventArea].EventId = @EventInsertedID)
	END

	SELECT @EventInsertedID

	COMMIT
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN;
		THROW 51000, 'Attempt to create event without seats', 1;
END CATCH




