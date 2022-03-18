IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddOrUpdateAppointments')
DROP PROCEDURE AddOrUpdateAppointments
GO
CREATE PROCEDURE AddOrUpdateAppointments
    @id int,
    @doctorId int,
	@intervalId int,
	@officeId int,
	@date date,
	@patientName nvarchar(50)
AS
IF @id = 0
	INSERT INTO Appointments(Id, [DoctorId], [IntervalId], [OfficeId], [Date], [PatientName])
	VALUES(@id, @doctorId, @intervalId, @officeId, @date, @patientName)
ELSE
	UPDATE Appointments
	SET
	Id = @id,
	[DoctorId] = @doctorId,
	[IntervalId] = @intervalId,
	[OfficeId] = @officeId,
	[Date] = @date,
	[PatientName] = @patientName
SET @id = @@IDENTITY
RETURN @id