using Hospital.Host.Data;
using Hospital.Host.Data.Entities;

namespace Hospital.Host.Repositories.Interfaces
{
    public interface IIntervalRepository
    {
        Task<PaginatedItems<Interval>> GetIntervals(int pageIndex, int pageSize);
        Task<Interval?> GetIntervalById(int id);
        Task<PaginatedItems<Interval>> GetFreeIntervalsByDoctorDate(int doctorId, DateTime date);
        Task<int?> AddInterval(TimeSpan start, TimeSpan end);
        Task<int?> UpdateInterval(int id, TimeSpan start, TimeSpan end);
        Task<int?> DeleteInterval(int id);
    }
}
