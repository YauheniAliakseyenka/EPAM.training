CREATE TABLE [dbo].[EventArea]
(
	[Id] INT IDENTITY PRIMARY KEY,
	[EventId] INT NOT NULL,
	[Description] NVARCHAR(200) NOT NULL,
	[CoordX] INT NOT NULL,
	[CoordY] INT NOT NULL,
	[Price] DECIMAL(18, 2) NOT NULL, 
    [AreaDefaultId] INT NOT NULL 
)
