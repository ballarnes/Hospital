IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteDoctors')
DROP PROCEDURE DeleteDoctors
GO
CREATE PROCEDURE DeleteDoctors
    @id int
AS
DELETE Doctors
WHERE Id = @id

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Doctors_DELETE')
DROP TRIGGER Doctors_DELETE;

GO

CREATE TRIGGER Doctors_DELETE ON Doctors
AFTER DELETE AS
INSERT INTO DoctorsChangeLog (DoctorId, [Name], [Surname], [SpecializationId], Operation)
SELECT Id, [Name], [Surname], [SpecializationId], 'DELETE'
FROM DELETED