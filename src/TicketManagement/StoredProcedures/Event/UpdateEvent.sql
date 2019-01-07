CREATE PROCEDURE [dbo].[UpdateEvent]
	@Title NVARCHAR(120),
	@Description NVARCHAR(MAX),
	@ImageURL NVARCHAR(MAX),
	@LayoutId INT,
	@Date DATETIME,
	@CreatedBy NVARCHAR(128),
	@Id INT
AS
BEGIN
BEGIN TRAN
BEGIN TRY
	DECLARE @layoutChanged INT = 0
		IF (SELECT [Event].[LayoutId] FROM [Event] WHERE [Event].Id = @id) != @LayoutId
			BEGIN
				DELETE FROM [EventArea] WHERE [EventArea].[EventId] = @Id

				UPDATE [Event] SET Title = @Title, Description = @Description, ImageURL = @ImageURL, Date = @Date, LayoutId = @LayoutId, CreatedBy = @CreatedBy
				WHERE [Event].[Id] = @Id

				SET @layoutChanged = 1

				EXEC AddEvent @Title, @Description, @ImageURL, @LayoutId, @Date, @CreatedBy,@Id, @layoutChanged;
			END
		ELSE
			BEGIN
				UPDATE [Event] SET
				Title = @Title,
				Description = @Description,
				ImageURL = @ImageURL,
				CreatedBy = @CreatedBy,
				Date = @Date
				WHERE [Event].[Id] = @Id
			END
	COMMIT
END TRY
BEGIN CATCH
		ROLLBACK TRAN
END CATCH
END