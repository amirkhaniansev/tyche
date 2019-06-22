SELECT	c.[column_id]			AS	[Id],
	c.[object_id]			AS	[FunctionId],
	c.[name]			AS	[Name],
	c.[system_type_id]		AS	[Type],
	c.[is_nullable]			AS	[IsNullable]
	FROM sys.columns c
	INNER JOIN sys.objects obj ON c.[object_id] = obj.[object_id]
	WHERE obj.[type] = 'IF'