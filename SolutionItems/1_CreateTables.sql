IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[Hospital].[Offices]') AND type in (N'U'))

BEGIN
CREATE TABLE Offices(
    Id int identity(1,1) primary key,
	[Number] int not null
) 

END

GO

IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[Hospital].[OfficesChangeLog]') AND type in (N'U'))

BEGIN
CREATE TABLE OfficesChangeLog(
	Id int identity(1,1) primary key,
	[OfficeId] int not null,
	[Number] int not null,
	[Operation] nvarchar(100) not null,
	[User] nvarchar(100) not null default CURRENT_USER,
	[ChangeDate] smalldatetime not null default GETDATE()
)

END

GO

IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[Hospital].[Specializations]') AND type in (N'U'))

BEGIN
CREATE TABLE Specializations(
    Id int identity(1,1) primary key,
	[Name] nvarchar(50) not null,
	[Description] nvarchar(300) not null
) 

END

GO

IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[Hospital].[SpecializationsChangeLog]') AND type in (N'U'))

BEGIN
CREATE TABLE SpecializationsChangeLog(
	Id int identity(1,1) primary key,
	[SpecializationId] int not null,
	[Name] nvarchar(50) not null,
	[Description] nvarchar(300) not null,
	[Operation] nvarchar(100) not null,
	[User] nvarchar(100) not null default CURRENT_USER,
	[ChangeDate] smalldatetime not null default GETDATE()
)

END

GO

IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[Hospital].[Doctors]') AND type in (N'U'))

BEGIN
CREATE TABLE Doctors(
    Id int identity(1,1) primary key,
	[Name] nvarchar(50) not null,
	[Surname] nvarchar(50) not null,
	[SpecializationId] int foreign key references Specializations(Id)
) 

END

GO

IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[Hospital].[DoctorsChangeLog]') AND type in (N'U'))

BEGIN
CREATE TABLE DoctorsChangeLog(
	Id int identity(1,1) primary key,
	[DoctorId] int not null,
	[Name] nvarchar(50) not null,
	[Surname] nvarchar(50) not null,
	[SpecializationId] int not null,
	[Operation] nvarchar(100) not null,
	[User] nvarchar(100) not null default CURRENT_USER,
	[ChangeDate] smalldatetime not null default GETDATE()
)

END

GO

IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[Hospital].[Appointments]') AND type in (N'U'))

BEGIN
CREATE TABLE Appointments(
    Id int identity(1,1) primary key,
	[DoctorId] int foreign key references Doctors(Id) on delete cascade,
	[OfficeId] int foreign key references Offices(Id) on delete cascade,
	[StartDate] datetime not null,
	[EndDate] datetime not null,
	[PatientName] nvarchar(50) not null
) 

END

GO

IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[Hospital].[AppointmentsChangeLog]') AND type in (N'U'))

BEGIN
CREATE TABLE AppointmentsChangeLog(
	Id int identity(1,1) primary key,
	[AppointmentId] int not null,
	[DoctorId] int not null,
	[OfficeId] int not null,
	[StartDate] datetime not null,
	[EndDate] datetime not null,
	[PatientName] nvarchar(50) not null,
	[Operation] nvarchar(100) not null,
	[User] nvarchar(100) not null default CURRENT_USER,
	[ChangeDate] smalldatetime not null default GETDATE()
)

END