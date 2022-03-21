IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteOffices')
DROP PROCEDURE DeleteOffices
GO
CREATE PROCEDURE DeleteOffices
    @id int
AS
DELETE Offices
WHERE Id = @id