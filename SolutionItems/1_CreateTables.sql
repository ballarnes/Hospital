IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[Hospital].[Offices]') AND type in (N'U'))

BEGIN
CREATE TABLE Offices(
    Id int primary key,
	[Number] int not null
) 

END

GO

IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[Hospital].[Intervals]') AND type in (N'U'))

BEGIN
CREATE TABLE Intervals(
    Id int primary key,
	[Start] time not null,
	[End] time not null
) 

END

GO

IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[Hospital].[Specializations]') AND type in (N'U'))

BEGIN
CREATE TABLE Specializations(
    Id int primary key,
	[Name] nvarchar(50) not null,
	[Description] nvarchar(100) not null
) 

END

GO

IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[Hospital].[Doctors]') AND type in (N'U'))

BEGIN
CREATE TABLE Doctors(
    Id int primary key,
	[Name] nvarchar(50) not null,
	[Surname] nvarchar(50) not null,
	[SpecializationId] int foreign key references Specializations(Id)
) 

END

GO

IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[Hospital].[Appointments]') AND type in (N'U'))

BEGIN
CREATE TABLE Appointments(
    Id int primary key,
	[DoctorId] int foreign key references Doctors(Id) on delete cascade,
	[IntervalId] int foreign key references Intervals(Id) on delete cascade,
	[OfficeId] int foreign key references Offices(Id) on delete cascade,
	[Date] date not null,
	[PatientName] nvarchar(50) not null
) 

END