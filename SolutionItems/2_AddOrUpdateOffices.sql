IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddOrUpdateOffices')
DROP PROCEDURE AddOrUpdateOffices
GO
CREATE PROCEDURE AddOrUpdateOffices
    @id int,
    @number int
AS
IF @id = 0
	INSERT INTO Offices(Id, Number) 
	VALUES(@id, @number)
ELSE
	UPDATE Offices
	SET
	Id = @id,
	[Number] = @number
SET @id = @@IDENTITY
RETURN @id