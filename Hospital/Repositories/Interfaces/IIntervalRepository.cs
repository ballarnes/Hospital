using Hospital.Host.Data;
using Hospital.Host.Models.Dtos;

namespace Hospital.Host.Repositories.Interfaces
{
    public interface IIntervalRepository
    {
        Task<PaginatedItems<IntervalDto>> GetIntervals(int pageIndex, int pageSize);
        Task<IntervalDto?> GetIntervalById(int id);
    }
}
