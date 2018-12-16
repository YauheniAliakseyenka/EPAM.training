CREATE PROCEDURE [dbo].[DeleteEvent]
	@Id int
AS
BEGIN
	DELETE FROM dbo.Event WHERE dbo.Event.Id=@Id
END
