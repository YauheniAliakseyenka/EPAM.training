﻿CREATE TABLE [dbo].[Venue]
(
	[Id] INT IDENTITY PRIMARY KEY,
	[Name] NVARCHAR(100) NOT NULL,
	[Address] NVARCHAR(200) NOT NULL,
	[Phone] NVARCHAR(30), 
    [Description] NVARCHAR(200) NOT NULL,
)
