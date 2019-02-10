CREATE PROCEDURE [dbo].[upsVerifyUser]
	@userId		INT,
	@code		NVARCHAR(32)
AS	
	BEGIN		
		DECLARE @isVerified BIT
		SELECT @isVerified = IsVerified FROM [Users] WHERE Id = @userId
		IF @isVerified = 1
			RETURN 0
		BEGIN TRY
			BEGIN TRANSACTION VERIFY
				DECLARE @created DATETIME
				DECLARE @validOffSet INT
				SELECT @created = Created, @validOffset = ValidOffset FROM [Verifications]
					WHERE UserId = @userId AND Code = @code
				IF @created + @validOffset > GETDATE()
					RETURN 1
				DELETE FROM [Verifications] WHERE UserId = @userId AND Code = @code
				UPDATE [Users] SET IsVerified = 1 WHERE Id = @userId
				RETURN 0
			COMMIT TRANSACTION VERIFIY	
		END TRY	
		BEGIN CATCH
			ROLLBACK TRANSACTION VERIFIY
			RETURN 2
		END CATCH		 
	END