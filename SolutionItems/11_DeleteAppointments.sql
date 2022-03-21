IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteAppointments')
DROP PROCEDURE DeleteAppointments
GO
CREATE PROCEDURE DeleteAppointments
    @id int
AS
DELETE Appointments
WHERE Id = @id