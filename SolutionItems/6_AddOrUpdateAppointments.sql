IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddOrUpdateAppointments')
DROP PROCEDURE AddOrUpdateAppointments
GO
CREATE PROCEDURE AddOrUpdateAppointments
    @doctorId int,
	@intervalId int,
	@officeId int,
	@date date,
	@patientName nvarchar(50),
	@id int = 0
AS
IF @id = 0
	BEGIN
		INSERT INTO Appointments([DoctorId], [IntervalId], [OfficeId], [Date], [PatientName])
		VALUES(@doctorId, @intervalId, @officeId, @date, @patientName)

		SET @id = scope_identity()
		RETURN @id
	END
ELSE
	BEGIN
		UPDATE Appointments
		SET
		[DoctorId] = @doctorId,
		[IntervalId] = @intervalId,
		[OfficeId] = @officeId,
		[Date] = @date,
		[PatientName] = @patientName
		WHERE Id = @id
	END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Appointments_INSERT')
DROP TRIGGER Appointments_INSERT;

GO

CREATE TRIGGER Appointments_INSERT ON Appointments
AFTER INSERT AS
INSERT INTO AppointmentsChangeLog (AppointmentId, [DoctorId], [IntervalId], [OfficeId], [Date], [PatientName], Operation)
SELECT Id, [DoctorId], [IntervalId], [OfficeId], [Date], [PatientName], 'INSERT'
FROM INSERTED

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Appointments_UPDATE')
DROP TRIGGER Appointments_UPDATE;

GO

CREATE TRIGGER Appointments_UPDATE ON Appointments
AFTER UPDATE AS
INSERT INTO AppointmentsChangeLog (AppointmentId, [DoctorId], [IntervalId], [OfficeId], [Date], [PatientName], Operation)
SELECT Id, [DoctorId], [IntervalId], [OfficeId], [Date], [PatientName], 'UPDATE'
FROM INSERTED