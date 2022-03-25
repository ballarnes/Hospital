using Hospital.Host.Data;
using Hospital.Host.Data.Entities;

namespace Hospital.Host.Repositories.Interfaces
{
    public interface IOfficeRepository
    {
        Task<PaginatedItems<Office>> GetOffices(int pageIndex, int pageSize);
        Task<Office?> GetOfficeById(int id);
        Task<PaginatedItems<Office>> GetFreeOfficesByIntervalDate(int intervalId, DateTime date);
        Task<int?> AddOffice(int number);
        Task<int?> UpdateOffice(int id, int number);
        Task<int?> DeleteOffice(int id);
    }
}
