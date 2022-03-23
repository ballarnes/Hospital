using Hospital.Host.Connection.Interfaces;
using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Responses;
using Hospital.Host.Repositories.Interfaces;
using Hospital.Host.Services.Interfaces;

namespace Hospital.Host.Services
{
    public class OfficeService : BaseDataService, IOfficeService
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;

        public OfficeService(
            IDbConnectionWrapper connection,
            IOfficeRepository officeRepository,
            IMapper mapper,
            ILogger<BaseDataService> logger)
            : base (connection, logger)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedItemsResponse<OfficeDto>?> GetOffices(int pageIndex, int pageSize)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _officeRepository.GetOffices(pageIndex, pageSize);

                if (result == null)
                {
                    return null;
                }

                return new PaginatedItemsResponse<OfficeDto>()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    PagesCount = result.PagesCount,
                    TotalCount = result.TotalCount,
                    Data = result.Data.Select(s => _mapper.Map<OfficeDto>(s)).ToList()
                };
            });
        }

        public async Task<OfficeDto?> GetOfficeById(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _officeRepository.GetOfficeById(id);

                if (result == null)
                {
                    return null;
                }

                var mapped = _mapper.Map<OfficeDto>(result);
                return mapped;
            });
        }

        public async Task<IdResponse<int>?> AddOffice(int number)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _officeRepository.AddOffice(number);

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

        public async Task<int?> UpdateOffice(int id, int number)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _officeRepository.UpdateOffice(id, number);

                if (result == default)
                {
                    return null;
                }

                return result;
            });
        }

        public async Task<int?> DeleteOffice(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _officeRepository.DeleteOffice(id);

                if (result == default)
                {
                    return null;
                }

                return result;
            });
        }
    }
}