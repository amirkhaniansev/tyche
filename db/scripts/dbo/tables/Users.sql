CREATE TABLE [dbo].[Users]
(
	[Id]				INT				NOT NULL	IDENTITY(1,1) ,
	[FirstName]			NVARCHAR(20)	NOT NULL,
	[LastName]			NVARCHAR(50)	NULL,
	[Username]			VARCHAR(55)		NOT NULL,
	[Email]				VARCHAR(100)	NOT NULL,
	[ProfilePictureUrl] VARCHAR(MAX)	NULL,
	[PasswordHash]		VARCHAR(MAX)	NOT NULL,
	[IsVerified]	BIT				NOT NULL DEFAULT(0)

	CONSTRAINT	[PK_USER_ID]		PRIMARY KEY ([Id])
)