namespace Hospital.UnitTests.Services
{
    public class SpecializationServiceTests
    {
        private readonly ISpecializationService _specializationService;

        private readonly Mock<ISpecializationRepository> _specializationRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbConnectionWrapper> _dbConnectionWrapper;
        private readonly Mock<IDbConnection> _dbConnection;
        private readonly Mock<ILogger<SpecializationService>> _logger;

        private readonly Specialization _testSpecialization = new Specialization()
        {
            Id = 1,
            Name = "test",
            Description = "test"
        };

        public SpecializationServiceTests()
        {
            _specializationRepository = new Mock<ISpecializationRepository>();
            _mapper = new Mock<IMapper>();
            _dbConnectionWrapper = new Mock<IDbConnectionWrapper>();
            _dbConnection = new Mock<IDbConnection>();
            _logger = new Mock<ILogger<SpecializationService>>();

            _dbConnectionWrapper.Setup(s => s.Connection).Returns(_dbConnection.Object);

            _specializationService = new SpecializationService(_dbConnectionWrapper.Object, _specializationRepository.Object, _mapper.Object, _logger.Object);
        }

        [Fact]
        public async Task GetSpecializations_Success()
        {
            // arrange
            var testPageIndex = 0;
            var testPageSize = 3;
            var testPagesCount = 1;
            var testTotalCount = 2;

            var paginatedItemsSuccess = new PaginatedItems<Specialization>()
            {
                Data = new List<Specialization>()
                {
                    _testSpecialization
                },
                TotalCount = testTotalCount,
                PagesCount = testPagesCount
            };

            var specializationSuccess = new Specialization()
            {
                Id = _testSpecialization.Id,
                Name = _testSpecialization.Name,
                Description = _testSpecialization.Description
            };

            var specializationDtoSuccess = new SpecializationDto()
            {
                Id = _testSpecialization.Id,
                Name = _testSpecialization.Name,
                Description = _testSpecialization.Description
            };

            _specializationRepository.Setup(s => s.GetSpecializations(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).ReturnsAsync(paginatedItemsSuccess);

            _mapper.Setup(s => s.Map<SpecializationDto>(
                It.Is<Specialization>(i => i.Equals(specializationSuccess)))).Returns(specializationDtoSuccess);

            // act
            var result = await _specializationService.GetSpecializations(testPageIndex, testPageSize);

            // assert
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.PagesCount.Should().Be(testPagesCount);
            result?.PageIndex.Should().Be(testPageIndex);
            result?.PageSize.Should().Be(testPageSize);
            result?.TotalCount.Should().Be(testTotalCount);
        }

        [Fact]
        public async Task GetSpecializations_Failed()
        {
            // arrange
            Task<PaginatedItems<Specialization>>? testResult = null;
            var testPageIndex = int.MaxValue;
            var testPageSize = int.MaxValue;

            _specializationRepository.Setup(s => s.GetSpecializations(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).Returns(testResult!);

            // act
            var result = await _specializationService.GetSpecializations(testPageIndex, testPageSize);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetSpecializationById_Success()
        {
            // arrange
            var specializationSuccess = new Specialization()
            {
                Id = _testSpecialization.Id,
                Name = _testSpecialization.Name,
                Description = _testSpecialization.Description
            };

            var specializationDtoSuccess = new SpecializationDto()
            {
                Id = _testSpecialization.Id,
                Name = _testSpecialization.Name,
                Description = _testSpecialization.Description
            };

            _specializationRepository.Setup(s => s.GetSpecializationById(
                It.Is<int>(i => i == _testSpecialization.Id))).ReturnsAsync(specializationSuccess);

            _mapper.Setup(s => s.Map<SpecializationDto>(
                It.Is<Specialization>(i => i.Equals(specializationSuccess)))).Returns(specializationDtoSuccess);

            // act
            var result = await _specializationService.GetSpecializationById(_testSpecialization.Id);

            // assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(_testSpecialization.Id);
            result?.Name.Should().Be(_testSpecialization.Name);
            result?.Description.Should().Be(_testSpecialization.Description);
        }

        [Fact]
        public async Task GetSpecializationById_Failed()
        {
            // arrange
            Task<Specialization>? testResult = null;

            _specializationRepository.Setup(s => s.GetSpecializationById(
                It.Is<int>(i => i == _testSpecialization.Id))).Returns(testResult!);

            // act
            var result = await _specializationService.GetSpecializationById(_testSpecialization.Id);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddSpecialization_Success()
        {
            // arrange
            var testResult = 1;

            _specializationRepository.Setup(s => s.AddSpecialization(
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _specializationService.AddSpecialization(_testSpecialization.Name, _testSpecialization.Description);

            // assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(testResult);
        }

        [Fact]
        public async Task AddSpecialization_Failed()
        {
            // arrange
            int? testResult = null;

            _specializationRepository.Setup(s => s.AddSpecialization(
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _specializationService.AddSpecialization(_testSpecialization.Name, _testSpecialization.Description);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateSpecialization_Success()
        {
            // arrange
            var testResult = 1;

            _specializationRepository.Setup(s => s.UpdateSpecialization(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _specializationService.UpdateSpecialization(_testSpecialization.Id, _testSpecialization.Name, _testSpecialization.Description);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateSpecialization_Failed()
        {
            // arrange
            int? testResult = null;

            _specializationRepository.Setup(s => s.UpdateSpecialization(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _specializationService.UpdateSpecialization(_testSpecialization.Id, _testSpecialization.Name, _testSpecialization.Description);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteSpecialization_Success()
        {
            // arrange
            var testResult = 1;

            _specializationRepository.Setup(s => s.DeleteSpecialization(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _specializationService.DeleteSpecialization(_testSpecialization.Id);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteSpecialization_Failed()
        {
            // arrange
            int? testResult = null;

            _specializationRepository.Setup(s => s.DeleteSpecialization(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _specializationService.DeleteSpecialization(_testSpecialization.Id);

            // assert
            result.Should().Be(testResult);
        }
    }
}