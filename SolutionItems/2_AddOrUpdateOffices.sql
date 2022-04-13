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
	
GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Offices_INSERT')
DROP TRIGGER Offices_INSERT;

GO

CREATE TRIGGER Offices_INSERT ON Offices
AFTER INSERT AS
INSERT INTO OfficesChangeLog (OfficeId, Number, Operation)
SELECT Id, Number, 'INSERT'
FROM INSERTED

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Offices_UPDATE')
DROP TRIGGER Offices_UPDATE;

GO

CREATE TRIGGER Offices_UPDATE ON Offices
AFTER UPDATE AS
INSERT INTO OfficesChangeLog (OfficeId, Number, Operation)
SELECT Id, Number, 'UPDATE'
FROM INSERTED