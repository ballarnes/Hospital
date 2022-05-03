using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Hospital.BusinessLogic.Models.Responses;
using Hospital.BusinessLogic.Services.Interfaces;
using Hospital.DataAccess.Repositories.Interfaces;
using Infrastructure.Connection.Interfaces;
using AutoMapper;
using Hospital.DataAccess.Models.Dtos;

namespace Hospital.BusinessLogic.Services
{
    public class DoctorService : BaseDataService, IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorService(
            IDbConnectionWrapper connection,
            IDoctorRepository doctorRepository,
            IMapper mapper,
            ILogger<BaseDataService> logger)
            : base(connection, logger)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedItemsResponse<DoctorDto>> GetDoctors(int pageIndex, int pageSize)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _doctorRepository.GetDoctors(pageIndex, pageSize);

                if (result == null)
                {
                    return null;
                }

                return new PaginatedItemsResponse<DoctorDto>()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    PagesCount = result.PagesCount,
                    TotalCount = result.TotalCount,
                    Data = result.Data.Select(s => _mapper.Map<DoctorDto>(s)).ToList()
                };
            });
        }

        public async Task<DoctorDto> GetDoctorById(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _doctorRepository.GetDoctorById(id);
                var mapped = _mapper.Map<DoctorDto>(result);
                return mapped;
            });
        }

        public async Task<PaginatedItemsResponse<DoctorDto>> GetDoctorsBySpecializationId(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _doctorRepository.GetDoctorsBySpecializationId(id);

                if (result == null)
                {
                    return null;
                }

                return new PaginatedItemsResponse<DoctorDto>()
                {
                    PageIndex = 0,
                    PageSize = result.TotalCount,
                    PagesCount = result.PagesCount,
                    TotalCount = result.TotalCount,
                    Data = result.Data.Select(s => _mapper.Map<DoctorDto>(s)).ToList()
                };
            });
        }

        public async Task<IdResponse<int>> AddDoctor(string name, string surname, int specializationId)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _doctorRepository.AddDoctor(name, surname, specializationId);

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

        public async Task<int?> UpdateDoctor(int id, string name, string surname, int specializationId)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _doctorRepository.UpdateDoctor(id, name, surname, specializationId);

                if (result == default)
                {
                    return null;
                }

                return result;
            });
        }

        public async Task<int?> DeleteDoctor(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _doctorRepository.DeleteDoctor(id);

                if (result == default)
                {
                    return null;
                }

                return result;
            });
        }
    }
}