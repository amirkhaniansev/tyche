/**
 * GNU General Public License Version 3.0, 29 June 2007
 * usp_VerifyUser
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

/***Type : NoReturnValue***/
CREATE PROCEDURE [dbo].[usp_VerifyUser]
	@userId		INT,
	@code		NVARCHAR(32)
AS	
	BEGIN		
		DECLARE @isVerified BIT
		SELECT @isVerified = IsVerified FROM [Users] WHERE Id = @userId
		IF @isVerified IS NULL
			RETURN 0x7
		if @isVerified = 1
			RETURN 0x8
		BEGIN TRY
			BEGIN TRANSACTION VERIFY
				DECLARE @created		DATETIME
				DECLARE @validOffSet	INT
				SELECT @created = Created, @validOffset = ValidOffset FROM [Verifications]
					WHERE UserId = @userId AND Code = @code
				IF @created + @validOffset > GETDATE()
					RETURN 0x6
				DELETE FROM [Verifications] WHERE UserId = @userId AND Code = @code
				UPDATE [Users] SET IsVerified = 1 WHERE Id = @userId
				RETURN 0x0
			COMMIT TRANSACTION VERIFIY	
		END TRY	
		BEGIN CATCH
			ROLLBACK TRANSACTION VERIFIY
			RETURN 0x4
		END CATCH		 
	END