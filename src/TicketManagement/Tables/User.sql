CREATE TABLE [dbo].[User]
(
	[Id] NVARCHAR(128) NOT NULL PRIMARY KEY, 
    [UserName] NVARCHAR(50) NOT NULL, 
    [PasswordHash] NVARCHAR(128) NOT NULL, 
    [Email] NVARCHAR(256) NOT NULL, 
    [Firstname] NVARCHAR(80) NULL, 
    [Surname] NVARCHAR(60) NULL, 
    [Culture] NVARCHAR(10) NULL, 
    [Timezone] NVARCHAR(50) NULL, 
    [Amount] DECIMAL(18, 2) NOT NULL DEFAULT 0 
)
