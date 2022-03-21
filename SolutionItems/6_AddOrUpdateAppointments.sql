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

		SET @id = @@IDENTITY
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
		RETURN @id
	END