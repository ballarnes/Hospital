using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Responses;

namespace Hospital.Host.Services.Interfaces
{
    public interface IOfficeService
    {
        Task<PaginatedItemsResponse<OfficeDto>?> GetOffices(int pageIndex, int pageSize);
        Task<OfficeDto?> GetOfficeById(int id);
        Task<IdResponse<int>?> AddOffice(int number);
        Task<int?> UpdateOffice(int id, int number);
        Task<int?> DeleteOffice(int id);
    }
}
