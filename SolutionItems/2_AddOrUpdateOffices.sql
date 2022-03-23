IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddOrUpdateOffices')
DROP PROCEDURE AddOrUpdateOffices
GO
CREATE PROCEDURE AddOrUpdateOffices
    @number int,
	@id int = 0
AS
IF @id = 0
	BEGIN
		INSERT INTO Offices(Number) 
		VALUES(@number)

		SET @id = @@IDENTITY
		RETURN @id
	END
ELSE
	BEGIN
		UPDATE Offices
		SET
		[Number] = @number
		WHERE Id = @id
	END