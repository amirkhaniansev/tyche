CREATE PROCEDURE [dbo].[uspCreateUser]
	@firstName			NVARCHAR(20),
	@lastName			NVARCHAR(50),
	@username			VARCHAR(55),
	@email				VARCHAR(100),
	@profilePictureUrl	VARCHAR(MAX),
	@passwordHash		VARCHAR(MAX)
AS
	BEGIN
		IF EXISTS (SELECT * FROM [Users] WHERE Username = @username OR Email = @email)
			RETURN 1
		BEGIN TRY
			BEGIN TRANSACTION CREATE_USER
				INSERT INTO Users VALUES(
					@firstName,
					@lastName,
					@username,
					@email,
					@profilePictureUrl,
					@passwordHash,
					0)
			COMMIT TRANSACTION CREATE_USER
			RETURN SCOPE_IDENTITY()
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION CREATE_USER
			RETURN 2
		END CATCH	
	END