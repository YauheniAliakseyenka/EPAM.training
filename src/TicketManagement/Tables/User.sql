CREATE TABLE [dbo].[User]
(
	[Id] INT IDENTITY PRIMARY KEY, 
    [UserName] NVARCHAR(100) NOT NULL, 
    [PasswordHash] NVARCHAR(256) NOT NULL, 
    [Email] NVARCHAR(256) NOT NULL, 
    [Firstname] NVARCHAR(80) NULL, 
    [Surname] NVARCHAR(60) NULL, 
    [Culture] NVARCHAR(10) NULL, 
    [Timezone] NVARCHAR(100) NOT NULL, 
    [Amount] DECIMAL(18, 2) NOT NULL DEFAULT 0, 
    [Salt] NVARCHAR(256) NOT NULL 
)
