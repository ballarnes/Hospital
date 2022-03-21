using Hospital.Host.Connection.Interfaces;
using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Responses;
using Hospital.Host.Repositories.Interfaces;
using Hospital.Host.Services.Interfaces;

namespace Hospital.Host.Services
{
    public class SpecializationService : BaseDataService, ISpecializationService
    {
        private readonly ISpecializationRepository _specializationRepository;
        private readonly IMapper _mapper;

        public SpecializationService(
            IDbConnectionWrapper connection,
            ISpecializationRepository specializationRepository,
            IMapper mapper,
            ILogger<BaseDataService> logger)
            : base(connection, logger)
        {
            _specializationRepository = specializationRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedItemsResponse<SpecializationDto>?> GetSpecializations(int pageIndex, int pageSize)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _specializationRepository.GetSpecializations(pageIndex, pageSize);

                if (result == null)
                {
                    return null;
                }

                return new PaginatedItemsResponse<SpecializationDto>()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    PagesCount = result.PagesCount,
                    TotalCount = result.TotalCount,
                    Data = result.Data
                };
            });
        }

        public async Task<SpecializationDto?> GetSpecializationById(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _specializationRepository.GetSpecializationById(id);
                var mapped = _mapper.Map<SpecializationDto>(result);
                return mapped;
            });
        }
    }
}