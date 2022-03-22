using Hospital.Host.Connection.Interfaces;
using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Responses;
using Hospital.Host.Repositories.Interfaces;
using Hospital.Host.Services.Interfaces;

namespace Hospital.Host.Services
{
    public class IntervalService : BaseDataService, IIntervalService
    {
        private readonly IIntervalRepository _intervalRepository;
        private readonly IMapper _mapper;

        public IntervalService(
            IDbConnectionWrapper connection,
            IIntervalRepository intervalRepository,
            IMapper mapper,
            ILogger<BaseDataService> logger)
            : base(connection, logger)
        {
            _intervalRepository = intervalRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedItemsResponse<IntervalDto>?> GetIntervals(int pageIndex, int pageSize)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _intervalRepository.GetIntervals(pageIndex, pageSize);

                if (result == null)
                {
                    return null;
                }

                return new PaginatedItemsResponse<IntervalDto>()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    PagesCount = result.PagesCount,
                    TotalCount = result.TotalCount,
                    Data = result.Data
                };
            });
        }

        public async Task<IntervalDto?> GetIntervalById(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _intervalRepository.GetIntervalById(id);
                var mapped = _mapper.Map<IntervalDto>(result);
                return mapped;
            });
        }

        public async Task<IdResponse<int>?> AddInterval(TimeSpan start, TimeSpan end)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _intervalRepository.AddInterval(start, end);

                if (result == default)
                {
                    return null;
                }

                return new IdResponse<int>()
                {
                    Id = result.Value
                };
            });
        }

        public async Task<int?> UpdateInterval(int id, TimeSpan start, TimeSpan end)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _intervalRepository.UpdateInterval(id, start, end);

                if (result == default)
                {
                    return null;
                }

                return result;
            });
        }

        public async Task<int?> DeleteInterval(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _intervalRepository.DeleteInterval(id);

                if (result == default)
                {
                    return null;
                }

                return result;
            });
        }
    }
}