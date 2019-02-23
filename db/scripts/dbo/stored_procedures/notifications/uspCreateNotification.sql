/**
 * GNU General Public License Version 3.0, 29 June 2007
 * uspCreateNotification
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


CREATE PROCEDURE [dbo].[uspCreateNotification]
	@type		INT,
	@info		NVARCHAR(MAX),
	@chatRoomId	INT
AS
	BEGIN
		BEGIN TRY
			BEGIN TRANSACTION CREATE_NOTIFICATION
				INSERT INTO [Notifications] VALUES (
					@type,
					@info,
					@chatRoomId)
			COMMIT TRANSACTION CREATE_NOTIFICATION
			RETURN SCOPE_IDENTITY()
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION CREATE_NOTIFICATION
			RETURN 1
		END CATCH
	END	