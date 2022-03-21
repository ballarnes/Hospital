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
		RETURN @id
	END