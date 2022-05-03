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

namespace Hospital.BusinessLogic.Services
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

        public async Task<PaginatedItemsResponse<IntervalDto>> GetIntervals(int pageIndex, int pageSize)
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
                    Data = result.Data.Select(s => _mapper.Map<IntervalDto>(s)).ToList()
                };
            });
        }

        public async Task<IntervalDto> GetIntervalById(int id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _intervalRepository.GetIntervalById(id);
                var mapped = _mapper.Map<IntervalDto>(result);
                return mapped;
            });
        }

        public async Task<PaginatedItemsResponse<IntervalDto>> GetFreeIntervalsByDoctorDate(int doctorId, DateTime date)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _intervalRepository.GetFreeIntervalsByDoctorDate(doctorId, date);

                if (result == null)
                {
                    return null;
                }

                return new PaginatedItemsResponse<IntervalDto>()
                {
                    PageIndex = 0,
                    PageSize = result.TotalCount,
                    PagesCount = result.PagesCount,
                    TotalCount = result.TotalCount,
                    Data = result.Data.Select(s => _mapper.Map<IntervalDto>(s)).ToList()
                };
            });
        }

        public async Task<IdResponse<int>> AddInterval(TimeSpan start, TimeSpan end)
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