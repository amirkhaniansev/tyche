SELECT	p.[parameter_id]	AS	[Id],
	p.[object_id]		AS	[FunctionId],
	p.[name]		AS	[Name],
	p.[system_type_id]	AS	[Type],
	p.[is_nullable]		AS	[IsNullable]
	FROM sys.parameters p 
	INNER JOIN sys.objects obj ON p.[object_id] = obj.[object_id]
	WHERE obj.[type] = 'IF'