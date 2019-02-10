﻿CREATE TABLE [dbo].[Verifications]
(
	[Id]			INT IDENTITY(1,1)	NOT NULL,
	[UserId]		INT					NOT NULL,
	[Code]			NVARCHAR(32)		NOT NULL,
	[Created]		DATETIME			NOT NULL,
	[ValidOffset]	INT					NOT NULL

	CONSTRAINT [FK_VERIFICATIONS_USER_ID] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
)