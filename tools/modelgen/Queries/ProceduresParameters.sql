SELECT	p.[parameter_id]	AS	[Id],
	p.[object_id]		AS	[ProcedureId],
	p.[name]		AS	[Name],
	p.[system_type_id]	AS	[Type],
	p.[is_nullable]		AS	[IsNullable]
	FROM sys.parameters p
	INNER JOIN sys.procedures sp ON	p.[object_id] = sp.[object_id]