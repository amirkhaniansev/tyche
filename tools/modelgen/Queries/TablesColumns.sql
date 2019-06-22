SELECT	c.[column_id]			AS	[Id],
	c.[object_id]			AS	[TableId],
	c.[name]			AS	[Name],
	c.[system_type_id]		AS	[Type],
	c.[is_nullable]			AS	[IsNullable],
	c.[is_identity]			AS	[IsIdentity]
	FROM sys.columns c
	INNER JOIN sys.tables t ON c.[object_id] = t.[object_id]
	WHERE t.[type] = 'U'