namespace Hospital.UnitTests.Services
{
    public class IntervalServiceTests
    {
        private readonly IIntervalService _intervalService;

        private readonly Mock<IIntervalRepository> _intervalRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbConnectionWrapper> _dbConnectionWrapper;
        private readonly Mock<IDbConnection> _dbConnection;
        private readonly Mock<ILogger<IntervalService>> _logger;

        private readonly Interval _testInterval = new Interval()
        {
            Id = 1,
            Start = new TimeSpan(0, 0, 0),
            End = new TimeSpan(0, 0, 0)
        };

        public IntervalServiceTests()
        {
            _intervalRepository = new Mock<IIntervalRepository>();
            _mapper = new Mock<IMapper>();
            _dbConnectionWrapper = new Mock<IDbConnectionWrapper>();
            _dbConnection = new Mock<IDbConnection>();
            _logger = new Mock<ILogger<IntervalService>>();

            _dbConnectionWrapper.Setup(s => s.Connection).Returns(_dbConnection.Object);

            _intervalService = new IntervalService(_dbConnectionWrapper.Object, _intervalRepository.Object, _mapper.Object, _logger.Object);
        }

        [Fact]
        public async Task GetIntervals_Success()
        {
            // arrange
            var testPageIndex = 0;
            var testPageSize = 3;
            var testPagesCount = 1;
            var testTotalCount = 2;

            var paginatedItemsSuccess = new PaginatedItems<Interval>()
            {
                Data = new List<Interval>()
                {
                    _testInterval
                },
                TotalCount = testTotalCount,
                PagesCount = testPagesCount
            };

            var intervalSuccess = new Interval()
            {
                Id = _testInterval.Id,
                Start = _testInterval.Start,
                End = _testInterval.End
            };

            var intervalDtoSuccess = new IntervalDto()
            {
                Id = _testInterval.Id,
                Start = _testInterval.Start,
                End = _testInterval.End
            };

            _intervalRepository.Setup(s => s.GetIntervals(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).ReturnsAsync(paginatedItemsSuccess);

            _mapper.Setup(s => s.Map<IntervalDto>(
                It.Is<Interval>(i => i.Equals(intervalSuccess)))).Returns(intervalDtoSuccess);

            // act
            var result = await _intervalService.GetIntervals(testPageIndex, testPageSize);

            // assert
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.PagesCount.Should().Be(testPagesCount);
            result?.PageIndex.Should().Be(testPageIndex);
            result?.PageSize.Should().Be(testPageSize);
            result?.TotalCount.Should().Be(testTotalCount);
        }

        [Fact]
        public async Task GetIntervals_Failed()
        {
            // arrange
            Task<PaginatedItems<Interval>>? testResult = null;
            var testPageIndex = int.MaxValue;
            var testPageSize = int.MaxValue;

            _intervalRepository.Setup(s => s.GetIntervals(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).Returns(testResult!);

            // act
            var result = await _intervalService.GetIntervals(testPageIndex, testPageSize);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetIntervalById_Success()
        {
            // arrange
            var intervalSuccess = new Interval()
            {
                Id = _testInterval.Id,
                Start = _testInterval.Start,
                End = _testInterval.End
            };

            var intervalDtoSuccess = new IntervalDto()
            {
                Id = _testInterval.Id,
                Start = _testInterval.Start,
                End = _testInterval.End
            };

            _intervalRepository.Setup(s => s.GetIntervalById(
                It.Is<int>(i => i == _testInterval.Id))).ReturnsAsync(intervalSuccess);

            _mapper.Setup(s => s.Map<IntervalDto>(
                It.Is<Interval>(i => i.Equals(intervalSuccess)))).Returns(intervalDtoSuccess);

            // act
            var result = await _intervalService.GetIntervalById(_testInterval.Id);

            // assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(_testInterval.Id);
            result?.Start.Should().Be(_testInterval.Start);
            result?.End.Should().Be(_testInterval.End);
        }

        [Fact]
        public async Task GetIntervalById_Failed()
        {
            // arrange
            Task<Interval>? testResult = null;

            _intervalRepository.Setup(s => s.GetIntervalById(
                It.Is<int>(i => i == _testInterval.Id))).Returns(testResult!);

            // act
            var result = await _intervalService.GetIntervalById(_testInterval.Id);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddInterval_Success()
        {
            // arrange
            var testResult = 1;

            _intervalRepository.Setup(s => s.AddInterval(
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>())).ReturnsAsync(testResult);

            // act
            var result = await _intervalService.AddInterval(_testInterval.Start, _testInterval.End);

            // assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(testResult);
        }

        [Fact]
        public async Task AddInterval_Failed()
        {
            // arrange
            int? testResult = null;

            _intervalRepository.Setup(s => s.AddInterval(
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>())).ReturnsAsync(testResult);

            // act
            var result = await _intervalService.AddInterval(_testInterval.Start, _testInterval.End);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateInterval_Success()
        {
            // arrange
            var testResult = 1;

            _intervalRepository.Setup(s => s.UpdateInterval(
                It.IsAny<int>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>())).ReturnsAsync(testResult);

            // act
            var result = await _intervalService.UpdateInterval(_testInterval.Id, _testInterval.Start, _testInterval.End);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateInterval_Failed()
        {
            // arrange
            int? testResult = null;

            _intervalRepository.Setup(s => s.UpdateInterval(
                It.IsAny<int>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<TimeSpan>())).ReturnsAsync(testResult);

            // act
            var result = await _intervalService.UpdateInterval(_testInterval.Id, _testInterval.Start, _testInterval.End);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteInterval_Success()
        {
            // arrange
            var testResult = 1;

            _intervalRepository.Setup(s => s.DeleteInterval(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _intervalService.DeleteInterval(_testInterval.Id);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task DeleteInterval_Failed()
        {
            // arrange
            int? testResult = null;

            _intervalRepository.Setup(s => s.DeleteInterval(
                It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _intervalService.DeleteInterval(_testInterval.Id);

            // assert
            result.Should().Be(testResult);
        }
    }
}