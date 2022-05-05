IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddOrUpdateSpecializations')
DROP PROCEDURE AddOrUpdateSpecializations
GO
CREATE PROCEDURE AddOrUpdateSpecializations
    @name nvarchar(50),
	@description nvarchar(300),
	@id int = 0
AS
IF @id = 0
	BEGIN
		INSERT INTO Specializations([Name], [Description])
		VALUES(@name, @description)

		SET @id = scope_identity()
		RETURN @id
	END
ELSE
	BEGIN
		UPDATE Specializations
		SET
		[Name] = @name,
		[Description] = @description
		WHERE Id = @id
	END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Specializations_INSERT')
DROP TRIGGER Specializations_INSERT;

GO

CREATE TRIGGER Specializations_INSERT ON Specializations
AFTER INSERT AS
INSERT INTO SpecializationsChangeLog (SpecializationId, [Name], [Description], Operation)
SELECT Id, [Name], [Description], 'INSERT'
FROM INSERTED

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Specializations_UPDATE')
DROP TRIGGER Specializations_UPDATE;

GO

CREATE TRIGGER Specializations_UPDATE ON Specializations
AFTER UPDATE AS
INSERT INTO SpecializationsChangeLog (SpecializationId, [Name], [Description], Operation)
SELECT Id, [Name], [Description], 'UPDATE'
FROM INSERTED