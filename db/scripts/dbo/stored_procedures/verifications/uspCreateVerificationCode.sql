CREATE PROCEDURE [dbo].[uspCreateVerificationCode]
	@userId			INT,
	@code			NVARCHAR(32),
	@validOffset	INT
AS
	BEGIN
		INSERT INTO [Verifications] VALUES (
			@userId,
			@code,
			GETDATE(),
			@validOffset)
		RETURN 0
	END