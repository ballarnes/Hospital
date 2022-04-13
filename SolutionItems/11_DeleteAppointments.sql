IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteAppointments')
DROP PROCEDURE DeleteAppointments
GO
CREATE PROCEDURE DeleteAppointments
    @id int
AS
DELETE Appointments
WHERE Id = @id

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Appointments_DELETE')
DROP TRIGGER Appointments_DELETE;

GO

CREATE TRIGGER Appointments_DELETE ON Appointments
AFTER DELETE AS
INSERT INTO AppointmentsChangeLog (AppointmentId, [DoctorId], [IntervalId], [OfficeId], [Date], [PatientName], Operation)
SELECT Id, [DoctorId], [IntervalId], [OfficeId], [Date], [PatientName], 'DELETE'
FROM DELETED