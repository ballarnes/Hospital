using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Responses;

namespace Hospital.Host.Services.Interfaces
{
    public interface ISpecializationService
    {
        Task<PaginatedItemsResponse<SpecializationDto>?> GetSpecializations(int pageIndex, int pageSize);
        Task<SpecializationDto?> GetSpecializationById(int id);
    }
}
