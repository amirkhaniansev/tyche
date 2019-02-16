/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Notifications
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *               <DavidPetr>       <david.petrosyan11100@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * Full notice : https://github.com/amirkhaniansev/tyche/tree/master/LICENSE
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
**/

CREATE TABLE [dbo].[Notifications]
(
	[Id]			BIGINT	IDENTITY(1,1)	NOT NULL,
	[Type]			INT						NULL,
	[UserId]		INT						NOT NULL,
	[Info]			NVARCHAR(MAX)			NULL,
	[ChatRoomId]	INT						NOT NULL,
	[IsSeen]		BIT DEFAULT(0)			NOT NULL
	
	CONSTRAINT [PK_NOTIFICATION_ID] PRIMARY KEY([Id]),
	CONSTRAINT [FK_NOTIFICATION_USER_ID] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
		ON DELETE CASCADE,
	CONSTRAINT [FK_NOTIFICATION_CHAT_ROOM_ID] FOREIGN KEY ([ChatRoomId]) REFERENCES [ChatRooms]([Id])
		ON DELETE CASCADE	
)