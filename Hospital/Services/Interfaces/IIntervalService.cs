using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Responses;

namespace Hospital.Host.Services.Interfaces
{
    public interface IIntervalService
    {
        Task<PaginatedItemsResponse<IntervalDto>?> GetIntervals(int pageIndex, int pageSize);
        Task<IntervalDto?> GetIntervalById(int id);
        Task<PaginatedItemsResponse<IntervalDto>?> GetFreeIntervalsByDoctorDate(int doctorId, DateTime date);
        Task<IdResponse<int>?> AddInterval(TimeSpan start, TimeSpan end);
        Task<int?> UpdateInterval(int id, TimeSpan start, TimeSpan end);
        Task<int?> DeleteInterval(int id);
    }
}
