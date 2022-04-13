IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddOrUpdateDoctors')
DROP PROCEDURE AddOrUpdateDoctors
GO
CREATE PROCEDURE AddOrUpdateDoctors
    @name nvarchar(50),
	@surname nvarchar(300),
	@specializationId int,
    @id int = 0
AS
IF @id = 0
	BEGIN
		INSERT INTO Doctors([Name], [Surname], [SpecializationId])
		VALUES(@name, @surname, @specializationId)

		SET @id = @@IDENTITY
		RETURN @id
	END
ELSE
	BEGIN
		UPDATE Doctors
		SET
		[Name] = @name,
		[Surname] = @surname,
		[SpecializationId] = @specializationId
		WHERE Id = @id
	END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Doctors_INSERT')
DROP TRIGGER Doctors_INSERT;

GO

CREATE TRIGGER Doctors_INSERT ON Doctors
AFTER INSERT AS
INSERT INTO DoctorsChangeLog (DoctorId, [Name], [Surname], [SpecializationId], Operation)
SELECT Id, [Name], [Surname], [SpecializationId], 'INSERT'
FROM INSERTED

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Doctors_UPDATE')
DROP TRIGGER Doctors_UPDATE;

GO

CREATE TRIGGER Doctors_UPDATE ON Doctors
AFTER UPDATE AS
INSERT INTO DoctorsChangeLog (DoctorId, [Name], [Surname], [SpecializationId], Operation)
SELECT Id, [Name], [Surname], [SpecializationId], 'UPDATE'
FROM INSERTED