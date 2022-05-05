IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddOrUpdateAppointments')
DROP PROCEDURE AddOrUpdateAppointments
GO
CREATE PROCEDURE AddOrUpdateAppointments
    @doctorId int,
	@officeId int,
	@startDate datetime,
	@endDate datetime,
	@patientName nvarchar(50),
	@id int = 0
AS
IF @id = 0
	BEGIN
		INSERT INTO Appointments([DoctorId], [OfficeId], [StartDate], [EndDate], [PatientName])
		VALUES(@doctorId, @officeId, @startDate, @endDate, @patientName)

		SET @id = scope_identity()
		RETURN @id
	END
ELSE
	BEGIN
		UPDATE Appointments
		SET
		[DoctorId] = @doctorId,
		[OfficeId] = @officeId,
		[StartDate] = @startDate,
		[EndDate] = @endDate,
		[PatientName] = @patientName
		WHERE Id = @id
	END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Appointments_INSERT')
DROP TRIGGER Appointments_INSERT;

GO

CREATE TRIGGER Appointments_INSERT ON Appointments
AFTER INSERT AS
INSERT INTO AppointmentsChangeLog (AppointmentId, [DoctorId], [OfficeId], [StartDate], [EndDate], [PatientName], Operation)
SELECT Id, [DoctorId], [OfficeId], [StartDate], [EndDate], [PatientName], 'INSERT'
FROM INSERTED

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Appointments_UPDATE')
DROP TRIGGER Appointments_UPDATE;

GO

CREATE TRIGGER Appointments_UPDATE ON Appointments
AFTER UPDATE AS
INSERT INTO AppointmentsChangeLog (AppointmentId, [DoctorId], [OfficeId], [StartDate], [EndDate], [PatientName], Operation)
SELECT Id, [DoctorId], [OfficeId], [StartDate], [EndDate], [PatientName], 'UPDATE'
FROM INSERTED