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
    public class OfficeServiceTests
    {
        private readonly IOfficeService _officeService;

        private readonly Mock<IOfficeRepository> _officeRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbConnectionWrapper> _dbConnectionWrapper;
        private readonly Mock<IDbConnection> _dbConnection;
        private readonly Mock<ILogger<OfficeService>> _logger;

        private readonly Office _testOffice = new Office()
        {
            Id = 1,
            Number = 1
        };

        public OfficeServiceTests()
        {
            _officeRepository = new Mock<IOfficeRepository>();
            _mapper = new Mock<IMapper>();
            _dbConnectionWrapper = new Mock<IDbConnectionWrapper>();
            _dbConnection = new Mock<IDbConnection>();
            _logger = new Mock<ILogger<OfficeService>>();

            _dbConnectionWrapper.Setup(s => s.Connection).Returns(_dbConnection.Object);

            _officeService = new OfficeService(_dbConnectionWrapper.Object, _officeRepository.Object, _mapper.Object, _logger.Object);
        }

        [Fact]
        public async Task GetOffices_Success()
        {
            // arrange
            var testPageIndex = 0;
            var testPageSize = 3;
            var testPagesCount = 1;
            var testTotalCount = 2;

            var paginatedItemsSuccess = new PaginatedItems<Office>()
            {
                Data = new List<Office>()
                {
                    new Office()
                    {
                        Number = _testOffice.Number,
                    },
                },
                TotalCount = testTotalCount,
                PagesCount = testPagesCount
            };

            var officeSuccess = new Office()
            {
                Number = _testOffice.Number
            };

            var officeDtoSuccess = new OfficeDto()
            {
                Number = _testOffice.Number
            };

            _officeRepository.Setup(s => s.GetOffices(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).ReturnsAsync(paginatedItemsSuccess);

            _mapper.Setup(s => s.Map<OfficeDto>(
                It.Is<Office>(i => i.Equals(officeSuccess)))).Returns(officeDtoSuccess);

            // act
            var result = await _officeService.GetOffices(testPageIndex, testPageSize);

            // assert
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.PagesCount.Should().Be(testPagesCount);
            result?.PageIndex.Should().Be(testPageIndex);
            result?.PageSize.Should().Be(testPageSize);
            result?.TotalCount.Should().Be(testTotalCount);
        }

        [Fact]
        public async Task GetOffices_Failed()
        {
            // arrange
            Task<PaginatedItems<Office>>? testResult = null; 
            var testPageIndex = int.MaxValue;
            var testPageSize = int.MaxValue;

            _officeRepository.Setup(s => s.GetOffices(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).Returns(testResult!);

            // act
            var result = await _officeService.GetOffices(testPageIndex, testPageSize);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetFreeOfficesByDate_Success()
        {
            // arrange
            var testTotalCount = 1;
            var testDate = DateTime.MinValue;

            var freeOfficesSuccess = new List<Office>()
            {
                new Office()
                {
                    Id = _testOffice.Id,
                    Number = _testOffice.Number
                }
            };

            var officeSuccess = new Office()
            {
                Id = _testOffice.Id,
                Number = _testOffice.Number
            };

            var officeDtoSuccess = new OfficeDto()
            {
                Id = _testOffice.Id,
                Number = _testOffice.Number
            };

            _officeRepository.Setup(s => s.GetFreeOfficesByDate(
                It.Is<DateTime>(i => i == testDate))).ReturnsAsync(freeOfficesSuccess);

            _mapper.Setup(s => s.Map<OfficeDto>(
                It.Is<Office>(i => i.Equals(officeSuccess)))).Returns(officeDtoSuccess);

            // act
            var result = await _officeService.GetFreeOfficesByDate(testDate);

            // assert
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.TotalCount.Should().Be(testTotalCount);
        }

        [Fact]
        public async Task GetFreeOfficesByDate_Failed()
        {
            // arrange
            Task<List<Office>>? testResult = null;
            var testDate = DateTime.MinValue;

            _officeRepository.Setup(s => s.GetFreeOfficesByDate(
                It.Is<DateTime>(i => i == testDate))).Returns(testResult!);

            // act
            var result = await _officeService.GetFreeOfficesByDate(testDate);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetOfficeById_Success()
        {
            // arrange
            var officeSuccess = new Office()
            {
                Id = _testOffice.Id,
                Number = _testOffice.Number
            };

            var officeDtoSuccess = new OfficeDto()
            {
                Id = _testOffice.Id,
                Number = _testOffice.Number
            };

            _officeRepository.Setup(s => s.GetOfficeById(
                It.Is<int>(i => i == _testOffice.Id))).ReturnsAsync(officeSuccess);

            _mapper.Setup(s => s.Map<OfficeDto>(
                It.Is<Office>(i => i.Equals(officeSuccess)))).Returns(officeDtoSuccess);

            // act
            var result = await _officeService.GetOfficeById(_testOffice.Id);

            // assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(_testOffice.Id);
            result?.Number.Should().Be(_testOffice.Number);
        }

        [Fact]
        public async Task GetOfficeById_Failed()
        {
            // arrange
            Task<Office>? testResult = null;

            _officeRepository.Setup(s => s.GetOfficeById(
                It.Is<int>(i => i == _testOffice.Id))).Returns(testResult!);

            // act
            var result = await _officeService.GetOfficeById(_testOffice.Id);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddOffice_Success()
        {
            // arrange
            var testResult = 1;

            _officeRepository.Setup(s => s.AddOffice(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _officeService.AddOffice(_testOffice.Id);

            // assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(testResult);
        }

        [Fact]
        public async Task AddOffice_Failed()
        {
            // arrange
            int? testResult = null;

            _officeRepository.Setup(s => s.AddOffice(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _officeService.AddOffice(_testOffice.Id);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateOffice_Success()
        {
            // arrange
            var officeDtoSuccess = new OfficeDto()
            {
                Id = _testOffice.Id,
                Number = _testOffice.Number
            };

            var testResult = 1;

            _officeRepository.Setup(s => s.UpdateOffice(
                It.IsAny<Office>())).ReturnsAsync(testResult);

            // act
            var result = await _officeService.UpdateOffice(officeDtoSuccess);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateOffice_Failed()
        {
            // arrange
            var officeDtoFailed = new OfficeDto()
            {
                Id = int.MinValue,
                Number = int.MinValue
            };

            int? testResult = null;

            _officeRepository.Setup(s => s.UpdateOffice(
                It.IsAny<Office>())).ReturnsAsync(testResult);

            // act
            var result = await _officeService.UpdateOffice(officeDtoFailed);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteOffice_Success()
        {
            // arrange
            var testResult = 1;

            _officeRepository.Setup(s => s.DeleteOffice(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _officeService.DeleteOffice(_testOffice.Id);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteOffice_Failed()
        {
            // arrange
            int? testResult = null;

            _officeRepository.Setup(s => s.DeleteOffice(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _officeService.DeleteOffice(_testOffice.Id);

            // assert
            result.Should().Be(testResult);
        }
    }
}