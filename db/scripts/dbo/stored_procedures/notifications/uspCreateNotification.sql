CREATE PROCEDURE [dbo].[uspCreateNotification]
	@type		INT,
	@userId		INT,
	@info		NVARCHAR(MAX),
	@chatRoomId	INT
AS
	BEGIN
		IF NOT EXISTS (SELECT UserId FROM [ChatRoomMembers] WHERE 
				UserId = @userId AND ChatRoomId = @chatRoomId)
			RETURN 1
		INSERT INTO [Notifications] VALUES(
			@type,
			@userId,
			@info,
			@chatRoomId,
			0)
		RETURN 0
	END	