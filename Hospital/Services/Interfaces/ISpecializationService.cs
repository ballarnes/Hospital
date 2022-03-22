using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Responses;

namespace Hospital.Host.Services.Interfaces
{
    public interface ISpecializationService
    {
        Task<PaginatedItemsResponse<SpecializationDto>?> GetSpecializations(int pageIndex, int pageSize);
        Task<SpecializationDto?> GetSpecializationById(int id);
        Task<IdResponse<int>?> AddSpecialization(string name, string description);
        Task<int?> UpdateSpecialization(int id, string name, string description);
        Task<int?> DeleteSpecialization(int id);
    }
}
