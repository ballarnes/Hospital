using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Hospital.BusinessLogic.Services;
using Hospital.BusinessLogic.Services.Interfaces;
using Hospital.DataAccess.Data;
using Hospital.DataAccess.Models.Dtos;
using Hospital.DataAccess.Models.Entities;
using Hospital.DataAccess.Repositories.Interfaces;
using Infrastructure.Connection.Interfaces;
using Moq;
using Xunit;
using AutoMapper;
using FluentAssertions;

namespace Hospital.UnitTests.Services
{
    public class AppointmentServiceTests
    {
        private readonly IAppointmentService _appointmentService;

        private readonly Mock<IAppointmentRepository> _appointmentRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbConnectionWrapper> _dbConnectionWrapper;
        private readonly Mock<IDbConnection> _dbConnection;
        private readonly Mock<ILogger<AppointmentService>> _logger;

        private readonly Appointment _testAppointment = new Appointment()
        {
            Id = 1,
            DoctorId = 1,
            OfficeId = 1,
            StartDate = DateTime.MinValue,
            EndDate = DateTime.MaxValue,
            PatientName = "test"
        };

        public AppointmentServiceTests()
        {
            _appointmentRepository = new Mock<IAppointmentRepository>();
            _mapper = new Mock<IMapper>();
            _dbConnectionWrapper = new Mock<IDbConnectionWrapper>();
            _dbConnection = new Mock<IDbConnection>();
            _logger = new Mock<ILogger<AppointmentService>>();

            _dbConnectionWrapper.Setup(s => s.Connection).Returns(_dbConnection.Object);

            _appointmentService = new AppointmentService(_dbConnectionWrapper.Object, _appointmentRepository.Object, _mapper.Object, _logger.Object);
        }

        [Fact]
        public async Task GetAppointments_Success()
        {
            // arrange
            var testPageIndex = 0;
            var testPageSize = 3;
            var testPagesCount = 1;
            var testTotalCount = 2;

            var paginatedItemsSuccess = new PaginatedItems<Appointment>()
            {
                Data = new List<Appointment>()
                {
                    _testAppointment
                },
                TotalCount = testTotalCount,
                PagesCount = testPagesCount
            };

            var appointmentSuccess = new Appointment()
            {
                Id = _testAppointment.Id,
                DoctorId = _testAppointment.DoctorId,
                OfficeId = _testAppointment.OfficeId,
                StartDate = _testAppointment.StartDate,
                PatientName = _testAppointment.PatientName
            };

            var appointmentDtoSuccess = new AppointmentDto()
            {
                Id = _testAppointment.Id,
                DoctorId = _testAppointment.DoctorId,
                OfficeId = _testAppointment.OfficeId,
                StartDate = _testAppointment.StartDate,
                PatientName = _testAppointment.PatientName
            };

            _appointmentRepository.Setup(s => s.GetAppointments(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).ReturnsAsync(paginatedItemsSuccess);

            _mapper.Setup(s => s.Map<AppointmentDto>(
                It.Is<Appointment>(i => i.Equals(appointmentSuccess)))).Returns(appointmentDtoSuccess);

            // act
            var result = await _appointmentService.GetAppointments(testPageIndex, testPageSize);

            // assert
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.PagesCount.Should().Be(testPagesCount);
            result?.PageIndex.Should().Be(testPageIndex);
            result?.PageSize.Should().Be(testPageSize);
            result?.TotalCount.Should().Be(testTotalCount);
        }

        [Fact]
        public async Task GetAppointments_Failed()
        {
            // arrange
            Task<PaginatedItems<Appointment>>? testResult = null;
            var testPageIndex = int.MaxValue;
            var testPageSize = int.MaxValue;

            _appointmentRepository.Setup(s => s.GetAppointments(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).Returns(testResult!);

            // act
            var result = await _appointmentService.GetAppointments(testPageIndex, testPageSize);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUpcomingAppointments_Success()
        {
            // arrange
            var testPageIndex = 0;
            var testPageSize = 3;
            var testPagesCount = 1;
            var testTotalCount = 2;
            var testName = "Test Test";

            var paginatedItemsSuccess = new PaginatedItems<Appointment>()
            {
                Data = new List<Appointment>()
                {
                    _testAppointment
                },
                TotalCount = testTotalCount,
                PagesCount = testPagesCount
            };

            var appointmentSuccess = new Appointment()
            {
                Id = _testAppointment.Id,
                DoctorId = _testAppointment.DoctorId,
                OfficeId = _testAppointment.OfficeId,
                StartDate = _testAppointment.StartDate,
                EndDate = _testAppointment.EndDate,
                PatientName = _testAppointment.PatientName
            };

            var appointmentDtoSuccess = new AppointmentDto()
            {
                Id = _testAppointment.Id,
                DoctorId = _testAppointment.DoctorId,
                OfficeId = _testAppointment.OfficeId,
                StartDate = _testAppointment.StartDate,
                EndDate = _testAppointment.EndDate,
                PatientName = _testAppointment.PatientName
            };

            _appointmentRepository.Setup(s => s.GetUpcomingAppointments(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize),
                It.Is<string>(i => i == testName))).ReturnsAsync(paginatedItemsSuccess);

            _mapper.Setup(s => s.Map<AppointmentDto>(
                It.Is<Appointment>(i => i.Equals(appointmentSuccess)))).Returns(appointmentDtoSuccess);

            // act
            var result = await _appointmentService.GetUpcomingAppointments(testPageIndex, testPageSize, testName);

            // assert
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.PagesCount.Should().Be(testPagesCount);
            result?.PageIndex.Should().Be(testPageIndex);
            result?.PageSize.Should().Be(testPageSize);
            result?.TotalCount.Should().Be(testTotalCount);
        }

        [Fact]
        public async Task GetUpcomingAppointments_Failed()
        {
            // arrange
            Task<PaginatedItems<Appointment>>? testResult = null;
            var testPageIndex = int.MaxValue;
            var testPageSize = int.MaxValue;
            var testName = string.Empty;

            _appointmentRepository.Setup(s => s.GetUpcomingAppointments(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize),
                It.Is<string>(i => i == testName))).Returns(testResult!);

            // act
            var result = await _appointmentService.GetUpcomingAppointments(testPageIndex, testPageSize, testName);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAppointmentsByDoctorDate_Success()
        {
            // arrange
            var testDoctorId = _testAppointment.DoctorId;
            var testTotalCount = 1;
            var testDate = _testAppointment.StartDate;

            var appointmentsSuccess = new List<Appointment>()
            {
                new Appointment()
                {
                    Id = _testAppointment.Id,
                    DoctorId = _testAppointment.DoctorId,
                    Doctor = _testAppointment.Doctor,
                    StartDate = _testAppointment.StartDate,
                    EndDate = _testAppointment.EndDate,
                    OfficeId = _testAppointment.OfficeId,
                    Office = _testAppointment.Office,
                    PatientName = _testAppointment.PatientName
                }
            };

            var appointmentSuccess = new Appointment()
            {
                Id = _testAppointment.Id,
                DoctorId = _testAppointment.DoctorId,
                OfficeId = _testAppointment.OfficeId,
                StartDate = _testAppointment.StartDate,
                EndDate = _testAppointment.EndDate,
                PatientName = _testAppointment.PatientName
            };

            var appointmentDtoSuccess = new AppointmentDto()
            {
                Id = _testAppointment.Id,
                DoctorId = _testAppointment.DoctorId,
                OfficeId = _testAppointment.OfficeId,
                StartDate = _testAppointment.StartDate,
                EndDate = _testAppointment.EndDate,
                PatientName = _testAppointment.PatientName
            };

            _appointmentRepository.Setup(s => s.GetAppointmentsByDoctorDate(
                It.Is<int>(i => i == testDoctorId),
                It.Is<DateTime>(i => i == testDate))).ReturnsAsync(appointmentsSuccess);

            _mapper.Setup(s => s.Map<AppointmentDto>(
                It.Is<Appointment>(i => i.Equals(appointmentSuccess)))).Returns(appointmentDtoSuccess);

            // act
            var result = await _appointmentService.GetAppointmentsByDoctorDate(testDoctorId, testDate);

            // assert
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.TotalCount.Should().Be(testTotalCount);
        }

        [Fact]
        public async Task GetAppointmentsByDoctorDate_Failed()
        {
            // arrange
            Task<List<Appointment>>? testResult = null;
            var testDoctorId = int.MaxValue;
            var testDate = DateTime.MinValue;

            _appointmentRepository.Setup(s => s.GetAppointmentsByDoctorDate(
                It.Is<int>(i => i == testDoctorId),
                It.Is<DateTime>(i => i == testDate))).Returns(testResult!);

            // act
            var result = await _appointmentService.GetAppointmentsByDoctorDate(testDoctorId, testDate);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAppointmentById_Success()
        {
            // arrange
            var appointmentSuccess = new Appointment()
            {
                Id = _testAppointment.Id,
                DoctorId = _testAppointment.DoctorId,
                OfficeId = _testAppointment.OfficeId,
                StartDate = _testAppointment.StartDate,
                EndDate = _testAppointment.EndDate,
                PatientName = _testAppointment.PatientName
            };

            var appointmentDtoSuccess = new AppointmentDto()
            {
                Id = _testAppointment.Id,
                DoctorId = _testAppointment.DoctorId,
                OfficeId = _testAppointment.OfficeId,
                StartDate = _testAppointment.StartDate,
                EndDate = _testAppointment.EndDate,
                PatientName = _testAppointment.PatientName
            };

            _appointmentRepository.Setup(s => s.GetAppointmentById(
                It.Is<int>(i => i == _testAppointment.Id))).ReturnsAsync(appointmentSuccess);

            _mapper.Setup(s => s.Map<AppointmentDto>(
                It.Is<Appointment>(i => i.Equals(appointmentSuccess)))).Returns(appointmentDtoSuccess);

            // act
            var result = await _appointmentService.GetAppointmentById(_testAppointment.Id);

            // assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(_testAppointment.Id);
            result?.DoctorId.Should().Be(_testAppointment.DoctorId);
            result?.OfficeId.Should().Be(_testAppointment.OfficeId);
            result?.StartDate.Should().Be(_testAppointment.StartDate);
            result?.EndDate.Should().Be(_testAppointment.EndDate);
            result?.PatientName.Should().Be(_testAppointment.PatientName);
        }

        [Fact]
        public async Task GetAppointmentById_Failed()
        {
            // arrange
            Task<Appointment>? testResult = null;

            _appointmentRepository.Setup(s => s.GetAppointmentById(
                It.Is<int>(i => i == _testAppointment.Id))).Returns(testResult!);

            // act
            var result = await _appointmentService.GetAppointmentById(_testAppointment.Id);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAppointment_Success()
        {
            // arrange
            var testResult = 1;

            _appointmentRepository.Setup(s => s.AddAppointment(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _appointmentService.AddAppointment(_testAppointment.DoctorId, _testAppointment.OfficeId, _testAppointment.StartDate, _testAppointment.EndDate, _testAppointment.PatientName);

            // assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(testResult);
        }

        [Fact]
        public async Task AddAppointment_Failed()
        {
            // arrange
            int? testResult = null;

            _appointmentRepository.Setup(s => s.AddAppointment(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _appointmentService.AddAppointment(_testAppointment.DoctorId, _testAppointment.OfficeId, _testAppointment.StartDate, _testAppointment.EndDate, _testAppointment.PatientName);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateAppointment_Success()
        {
            // arrange
            var appointmentDtoSuccess = new AppointmentDto()
            {
                Id = _testAppointment.Id,
                DoctorId = _testAppointment.DoctorId,
                OfficeId = _testAppointment.OfficeId,
                StartDate = _testAppointment.StartDate,
                EndDate = _testAppointment.EndDate,
                PatientName = _testAppointment.PatientName
            };

            var testResult = 1;

            _appointmentRepository.Setup(s => s.UpdateAppointment(
                It.IsAny<Appointment>())).ReturnsAsync(testResult);

            // act
            var result = await _appointmentService.UpdateAppointment(appointmentDtoSuccess);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateAppointment_Failed()
        {
            // arrange
            var appointmentDtoFailed = new AppointmentDto()
            {
                Id = int.MinValue,
                DoctorId = int.MinValue,
                OfficeId = int.MinValue,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue,
                PatientName = string.Empty
            };

            int? testResult = null;

            _appointmentRepository.Setup(s => s.UpdateAppointment(
                It.IsAny<Appointment>())).ReturnsAsync(testResult);

            // act
            var result = await _appointmentService.UpdateAppointment(appointmentDtoFailed);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteAppointment_Success()
        {
            // arrange
            var testResult = 1;

            _appointmentRepository.Setup(s => s.DeleteAppointment(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _appointmentService.DeleteAppointment(_testAppointment.Id);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteAppointment_Failed()
        {
            // arrange
            int? testResult = null;

            _appointmentRepository.Setup(s => s.DeleteAppointment(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _appointmentService.DeleteAppointment(_testAppointment.Id);

            // assert
            result.Should().Be(testResult);
        }
    }
}