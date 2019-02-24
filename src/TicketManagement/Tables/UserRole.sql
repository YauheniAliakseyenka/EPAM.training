CREATE TABLE [dbo].[UserRole]
(
	[UserId] INT NOT NULL, 
    [RoleId] INT NOT NULL,
	PRIMARY KEY ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [Role]([Id]) 
)
