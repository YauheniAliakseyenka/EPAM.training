CREATE PROCEDURE [dbo].[UpdateEvent]
	@Name nvarchar(120),
	@Description nvarchar(200),
	@LayoutId int,
	@Date datetime,
	@Id int
AS
BEGIN
BEGIN TRAN
BEGIN TRY
	DECLARE @layoutChanged int = 0
		IF (SELECT [Event].[LayoutId] FROM [Event] WHERE [Event].Id = @id) != @LayoutId
			BEGIN
				DELETE FROM [EventArea] WHERE [EventArea].[EventId] = @Id

				UPDATE [Event] SET LayoutId = @LayoutId WHERE [Event].[Id] = @Id

				SET @layoutChanged = 1

				EXEC AddEvent @Name, @Description, @LayoutId, @Date, @Id, @layoutChanged;
			END
		ELSE
			BEGIN
				UPDATE [Event] SET
				Name = @Name,
				Description = @Description,
				Date = @Date
				WHERE [Event].[Id] = @Id
			END
	COMMIT
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
END CATCH
END