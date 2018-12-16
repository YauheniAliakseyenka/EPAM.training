CREATE TABLE [dbo].[Venue]
(
	[Id] int identity primary key,
	[Name] nvarchar(100) NOT NULL,
	[Address] nvarchar(200) NOT NULL,
	[Phone] nvarchar(30), 
    [Description] NVARCHAR(200) NOT NULL,
)
