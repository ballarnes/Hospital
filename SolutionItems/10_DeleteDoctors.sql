IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteDoctors')
DROP PROCEDURE DeleteDoctors
GO
CREATE PROCEDURE DeleteDoctors
    @id int
AS
DELETE Doctors
WHERE Id = @id