SELECT  [name]		AS [Name],
	[object_id]	AS [Id]
	FROM sys.tables
	WHERE [Name] != '__RefactorLog'