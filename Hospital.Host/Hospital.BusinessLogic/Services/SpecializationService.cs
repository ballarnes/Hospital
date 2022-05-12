using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Hospital.BusinessLogic.Models.Responses;
using Hospital.BusinessLogic.Services.Interfaces;
using Hospital.DataAccess.Repositories.Interfaces;
using Infrastructure.Connection.Interfaces;
using AutoMapper;
using Hospital.DataAccess.Models.Dtos;
using Hospital.DataAccess.Models.Entities;

namespace Hospital.BusinessLogic.Services
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

        public async Task<PaginatedItemsResponse<SpecializationDto>> GetSpecializations(int pageIndex, int pageSize)
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
                    Data = result.Data.Select(s => _mapper.Map<SpecializationDto>(s)).ToList()
                };
            });
        }

        public async Task<SpecializationDto> GetSpecializationById(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _specializationRepository.GetSpecializationById(id);
                var mapped = _mapper.Map<SpecializationDto>(result);
                return mapped;
            });
        }

        public async Task<IdResponse<int>> AddSpecialization(string name, string description)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _specializationRepository.AddSpecialization(name, description);

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

        public async Task<int?> UpdateSpecialization(SpecializationDto specializationDto)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _specializationRepository.UpdateSpecialization(_mapper.Map<Specialization>(specializationDto));

                if (result == default)
                {
                    return null;
                }

                return result;
            });
        }

        public async Task<int?> DeleteSpecialization(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _specializationRepository.DeleteSpecialization(id);

                if (result == default)
                {
                    return null;
                }

                return result;
            });
        }
    }
}