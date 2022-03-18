IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddOrUpdateSpecializations')
DROP PROCEDURE AddOrUpdateSpecializations
GO
CREATE PROCEDURE AddOrUpdateSpecializations
    @id int,
    @name nvarchar(50),
	@description nvarchar(100)
AS
IF @id = 0
	INSERT INTO Specializations(Id, [Name], [Description])
	VALUES(@id, @name, @description)
ELSE
	UPDATE Specializations
	SET
	Id = @id,
	[Name] = @name,
	[Description] = @description
SET @id = @@IDENTITY
RETURN @id