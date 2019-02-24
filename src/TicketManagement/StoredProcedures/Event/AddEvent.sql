CREATE PROCEDURE [dbo].[AddEvent]
	@Title NVARCHAR(120),
	@Description NVARCHAR(MAX),
	@ImageURL NVARCHAR(MAX),
	@LayoutId INT,
	@Date DATETIME,
	@CreatedBy INT,
	@EventInsertedID INT = 0 OUTPUT,
	@layoutChanged INT = 0
AS
BEGIN TRAN

BEGIN TRY
	DECLARE @AreaCount int
	DECLARE @Seats int

	SELECT @Seats = count(*)
		FROM
		(
		SELECT Seat.AreaId FROM Area
		JOIN Seat ON Seat.AreaId = Area.Id
		WHERE Area.LayoutId = @LayoutId
		GROUP BY Seat.AreaId
		) t

	SELECT @AreaCount = Count(Area.Id) FROM Area
		WHERE Area.LayoutId = @LayoutId

	IF @AreaCount ! = @Seats
		THROW 51000, 'Attempt to create event without seats', 1

	IF @layoutChanged = 0 
		BEGIN
			INSERT INTO [Event] VALUES(@Title, @Description, @LayoutId, @Date, @ImageURL, @CreatedBy)
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
		(SELECT @EventInsertedID, [Area].[Description], [Area].[CoordX], [Area].[CoordY], 0, [Area].[Id] FROM [Area]
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
		(SELECT [EventArea].[Id], [Seat].[Number], [Seat].[Row], 0 FROM [Seat]
		JOIN [EventArea] ON [EventArea].[AreaDefaultId] = [Seat].[AreaId]
		WHERE [EventArea].EventId = @EventInsertedID)
	END

	SELECT @EventInsertedID AS Id

	COMMIT
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN;
		THROW 51000, 'Transaction error', 1;
END CATCH




