using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Responses;

namespace Hospital.Host.Services.Interfaces
{
    public interface IIntervalService
    {
        Task<PaginatedItemsResponse<IntervalDto>?> GetIntervals(int pageIndex, int pageSize);
        Task<IntervalDto?> GetIntervalById(int id);
    }
}
