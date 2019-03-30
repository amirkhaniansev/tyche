CREATE TABLE [dbo].[BlockedIPs]
(
	[Id]		INT				IDENTITY(1,1) NOT NULL,	
	[IPAddress]	VARCHAR(255)	NOT NULL,
	[StartDate]	DATETIME		NOT NULL,
	[EndDate]	DATETIME		NOT NULL,
	[Reason]	INT				NULL

	
)