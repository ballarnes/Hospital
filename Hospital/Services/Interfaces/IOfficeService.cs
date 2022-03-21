using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Responses;

namespace Hospital.Host.Services.Interfaces
{
    public interface IOfficeService
    {
        Task<PaginatedItemsResponse<OfficeDto>?> GetOffices(int pageIndex, int pageSize);
        Task<OfficeDto?> GetOfficeById(int id);
    }
}
