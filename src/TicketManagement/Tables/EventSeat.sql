﻿CREATE TABLE [dbo].[EventSeat]
(
	[Id] INT IDENTITY PRIMARY KEY,
	[EventAreaId] INT NOT NULL,
	[Row] INT NOT NULL,
	[Number] INT NOT NULL,
	[State] INT NOT NULL
)
