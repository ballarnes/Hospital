IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddOrUpdateDoctors')
DROP PROCEDURE AddOrUpdateDoctors
GO
CREATE PROCEDURE AddOrUpdateDoctors
    @id int,
    @name nvarchar(50),
	@surname nvarchar(100),
	@specializationId int
AS
IF @id = 0
	INSERT INTO Doctors(Id, [Name], [Surname], [SpecializationId])
	VALUES(@id, @name, @surname, @specializationId)
ELSE
	UPDATE Doctors
	SET
	Id = @id,
	[Name] = @name,
	[Surname] = @surname,
	[SpecializationId] = @specializationId
SET @id = @@IDENTITY
RETURN @id