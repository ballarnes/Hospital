using System;
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

        public async Task<PaginatedItemsResponse<OfficeDto>> GetOffices(int pageIndex, int pageSize)
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

        public async Task<OfficeDto> GetOfficeById(int id)
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

        public async Task<ArrayResponse<OfficeDto>> GetFreeOfficesByDate(DateTime date)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _officeRepository.GetFreeOfficesByDate(date);

                if (result == null)
                {
                    return null;
                }

                return new ArrayResponse<OfficeDto>()
                {
                    TotalCount = result.Count,
                    Data = result.Select(s => _mapper.Map<OfficeDto>(s)).ToList()
                };
            });
        }

        public async Task<IdResponse<int>> AddOffice(int number)
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

        public async Task<int?> UpdateOffice(OfficeDto officeDto)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _officeRepository.UpdateOffice(_mapper.Map<Office>(officeDto));

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