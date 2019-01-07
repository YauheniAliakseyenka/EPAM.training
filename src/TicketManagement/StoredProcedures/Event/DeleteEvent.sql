CREATE PROCEDURE [dbo].[DeleteEvent]
	@Id INT
AS
BEGIN
	DELETE FROM dbo.Event WHERE dbo.Event.Id = @Id
END
