﻿CREATE TABLE [dbo].[ChatRooms]
(
	[Id]			INT			IDENTITY(1,1)	NOT NULL ,
	[Name]			NVARCHAR(100)				NULL,
	[Created]		DATETIME					NOT NULL,
	[IsGroup]		BIT			DEFAULT(0)		NOT NULL,
	[PictureUrl]	VARCHAR(MAX)				NULL
	
	CONSTRAINT [PK_CHATROOM_ID] PRIMARY KEY ([Id])
)