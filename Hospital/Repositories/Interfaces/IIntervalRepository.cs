using Hospital.Host.Data;
using Hospital.Host.Models.Dtos;

namespace Hospital.Host.Repositories.Interfaces
{
    public interface IIntervalRepository
    {
        Task<PaginatedItems<IntervalDto>> GetIntervals(int pageIndex, int pageSize);
        Task<IntervalDto?> GetIntervalById(int id);
        Task<int?> AddInterval(TimeSpan start, TimeSpan end);
        Task<int?> UpdateInterval(int id, TimeSpan start, TimeSpan end);
        Task<int?> DeleteInterval(int id);
    }
}
