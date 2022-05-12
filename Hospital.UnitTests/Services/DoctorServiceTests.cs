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
    public class DoctorServiceTests
    {
        private readonly IDoctorService _doctorService;

        private readonly Mock<IDoctorRepository> _doctorRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbConnectionWrapper> _dbConnectionWrapper;
        private readonly Mock<IDbConnection> _dbConnection;
        private readonly Mock<ILogger<DoctorService>> _logger;

        private readonly Doctor _testDoctor = new Doctor()
        {
            Id = 1,
            Name = "test",
            Surname = "test",
            SpecializationId = 1,
            Specialization = new Specialization()
        };

        public DoctorServiceTests()
        {
            _doctorRepository = new Mock<IDoctorRepository>();
            _mapper = new Mock<IMapper>();
            _dbConnectionWrapper = new Mock<IDbConnectionWrapper>();
            _dbConnection = new Mock<IDbConnection>();
            _logger = new Mock<ILogger<DoctorService>>();

            _dbConnectionWrapper.Setup(s => s.Connection).Returns(_dbConnection.Object);

            _doctorService = new DoctorService(_dbConnectionWrapper.Object, _doctorRepository.Object, _mapper.Object, _logger.Object);
        }

        [Fact]
        public async Task GetDoctors_Success()
        {
            // arrange
            var testPageIndex = 0;
            var testPageSize = 3;
            var testPagesCount = 1;
            var testTotalCount = 2;

            var paginatedItemsSuccess = new PaginatedItems<Doctor>()
            {
                Data = new List<Doctor>()
                {
                    _testDoctor
                },
                TotalCount = testTotalCount,
                PagesCount = testPagesCount
            };

            var doctorSuccess = new Doctor()
            {
                Id = _testDoctor.Id,
                Name = _testDoctor.Name,
                Surname = _testDoctor.Surname,
                SpecializationId = _testDoctor.SpecializationId
            };

            var doctorDtoSuccess = new DoctorDto()
            {
                Id = _testDoctor.Id,
                Name = _testDoctor.Name,
                Surname = _testDoctor.Surname,
                SpecializationId = _testDoctor.SpecializationId
            };

            _doctorRepository.Setup(s => s.GetDoctors(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).ReturnsAsync(paginatedItemsSuccess);

            _mapper.Setup(s => s.Map<DoctorDto>(
                It.Is<Doctor>(i => i.Equals(doctorSuccess)))).Returns(doctorDtoSuccess);

            // act
            var result = await _doctorService.GetDoctors(testPageIndex, testPageSize);

            // assert
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.PagesCount.Should().Be(testPagesCount);
            result?.PageIndex.Should().Be(testPageIndex);
            result?.PageSize.Should().Be(testPageSize);
            result?.TotalCount.Should().Be(testTotalCount);
        }

        [Fact]
        public async Task GetDoctors_Failed()
        {
            // arrange
            Task<PaginatedItems<Doctor>>? testResult = null;
            var testPageIndex = int.MaxValue;
            var testPageSize = int.MaxValue;

            _doctorRepository.Setup(s => s.GetDoctors(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).Returns(testResult!);

            // act
            var result = await _doctorService.GetDoctors(testPageIndex, testPageSize);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetDoctorById_Success()
        {
            // arrange
            var doctorSuccess = new Doctor()
            {
                Id = _testDoctor.Id,
                Name = _testDoctor.Name,
                Surname = _testDoctor.Surname,
                SpecializationId = _testDoctor.SpecializationId
            };

            var doctorDtoSuccess = new DoctorDto()
            {
                Id = _testDoctor.Id,
                Name = _testDoctor.Name,
                Surname = _testDoctor.Surname,
                SpecializationId = _testDoctor.SpecializationId
            };

            _doctorRepository.Setup(s => s.GetDoctorById(
                It.Is<int>(i => i == _testDoctor.Id))).ReturnsAsync(doctorSuccess);

            _mapper.Setup(s => s.Map<DoctorDto>(
                It.Is<Doctor>(i => i.Equals(doctorSuccess)))).Returns(doctorDtoSuccess);

            // act
            var result = await _doctorService.GetDoctorById(_testDoctor.Id);

            // assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(_testDoctor.Id);
            result?.Name.Should().Be(_testDoctor.Name);
            result?.Surname.Should().Be(_testDoctor.Surname);
            result?.SpecializationId.Should().Be(_testDoctor.SpecializationId);
        }

        [Fact]
        public async Task GetDoctorById_Failed()
        {
            // arrange
            Task<Doctor>? testResult = null;

            _doctorRepository.Setup(s => s.GetDoctorById(
                It.Is<int>(i => i == _testDoctor.Id))).Returns(testResult!);

            // act
            var result = await _doctorService.GetDoctorById(_testDoctor.Id);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetDoctorsBySpecializationId_Success()
        {
            // arrange
            var testPageIndex = 0;
            var testPageSize = 2;
            var testPagesCount = 1;
            var testTotalCount = 2;
            var testSpecializationId = _testDoctor.SpecializationId;

            var paginatedItemsSuccess = new PaginatedItems<Doctor>()
            {
                Data = new List<Doctor>()
                {
                    _testDoctor
                },
                TotalCount = testTotalCount,
                PagesCount = testPagesCount
            };

            var doctorSuccess = new Doctor()
            {
                Id = _testDoctor.Id,
                Name = _testDoctor.Name,
                Surname = _testDoctor.Surname,
                SpecializationId = _testDoctor.SpecializationId
            };

            var doctorDtoSuccess = new DoctorDto()
            {
                Id = _testDoctor.Id,
                Name = _testDoctor.Name,
                Surname = _testDoctor.Surname,
                SpecializationId = _testDoctor.SpecializationId
            };

            _doctorRepository.Setup(s => s.GetDoctorsBySpecializationId(
                It.Is<int>(i => i == testSpecializationId))).ReturnsAsync(paginatedItemsSuccess);

            _mapper.Setup(s => s.Map<DoctorDto>(
                It.Is<Doctor>(i => i.Equals(doctorSuccess)))).Returns(doctorDtoSuccess);

            // act
            var result = await _doctorService.GetDoctorsBySpecializationId(testSpecializationId);

            // assert
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.PagesCount.Should().Be(testPagesCount);
            result?.PageIndex.Should().Be(testPageIndex);
            result?.PageSize.Should().Be(testPageSize);
            result?.TotalCount.Should().Be(testTotalCount);
        }

        [Fact]
        public async Task GetDoctorsBySpecializationId_Failed()
        {
            // arrange
            Task<PaginatedItems<Doctor>>? testResult = null;
            var testSpecializationId = int.MaxValue;

            _doctorRepository.Setup(s => s.GetDoctorsBySpecializationId(
                It.Is<int>(i => i == testSpecializationId))).Returns(testResult!);

            // act
            var result = await _doctorService.GetDoctorsBySpecializationId(testSpecializationId);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddDoctor_Success()
        {
            // arrange
            var testResult = 1;

            _doctorRepository.Setup(s => s.AddDoctor(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _doctorService.AddDoctor(_testDoctor.Name, _testDoctor.Surname, _testDoctor.SpecializationId);

            // assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(testResult);
        }

        [Fact]
        public async Task AddDoctor_Failed()
        {
            // arrange
            int? testResult = null;

            _doctorRepository.Setup(s => s.AddDoctor(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _doctorService.AddDoctor(_testDoctor.Name, _testDoctor.Surname, _testDoctor.SpecializationId);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateDoctor_Success()
        {
            // arrange
            var doctorDtoSuccess = new DoctorDto()
            {
                Id = _testDoctor.Id,
                Name = _testDoctor.Name,
                Surname = _testDoctor.Surname,
                SpecializationId = _testDoctor.SpecializationId
            };

            var testResult = 1;

            _doctorRepository.Setup(s => s.UpdateDoctor(
                It.IsAny<Doctor>())).ReturnsAsync(testResult);

            // act
            var result = await _doctorService.UpdateDoctor(doctorDtoSuccess);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateDoctor_Failed()
        {
            // arrange
            var doctorDtoFailed = new DoctorDto()
            {
                Id = int.MinValue,
                Name = string.Empty,
                Surname = string.Empty,
                SpecializationId = int.MinValue
            };

            int? testResult = null;

            _doctorRepository.Setup(s => s.UpdateDoctor(
                It.IsAny<Doctor>())).ReturnsAsync(testResult);

            // act
            var result = await _doctorService.UpdateDoctor(doctorDtoFailed);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteDoctor_Success()
        {
            // arrange
            var testResult = 1;

            _doctorRepository.Setup(s => s.DeleteDoctor(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _doctorService.DeleteDoctor(_testDoctor.Id);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteDoctor_Failed()
        {
            // arrange
            int? testResult = null;

            _doctorRepository.Setup(s => s.DeleteDoctor(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _doctorService.DeleteDoctor(_testDoctor.Id);

            // assert
            result.Should().Be(testResult);
        }
    }
}