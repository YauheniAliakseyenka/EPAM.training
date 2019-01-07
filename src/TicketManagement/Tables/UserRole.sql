CREATE TABLE [dbo].[UserRole]
(
	[UserId] NVARCHAR(128) NOT NULL, 
    [RoleId] INT NOT NULL,
	PRIMARY KEY([UserId], [RoleId]), 
    CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [Role]([Id]) 
)
