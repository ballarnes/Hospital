using Hospital.Host.Data;
using Hospital.Host.Models.Dtos;

namespace Hospital.Host.Repositories.Interfaces
{
    public interface IOfficeRepository
    {
        Task<PaginatedItems<OfficeDto>> GetOffices(int pageIndex, int pageSize);
        Task<OfficeDto?> GetOfficeById(int id);
    }
}
