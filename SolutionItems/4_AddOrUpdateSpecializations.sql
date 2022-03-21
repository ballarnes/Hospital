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

		SET @id = @@IDENTITY
		RETURN @id
	END
ELSE
	BEGIN
		UPDATE Specializations
		SET
		[Name] = @name,
		[Description] = @description
		WHERE Id = @id
		RETURN @id
	END