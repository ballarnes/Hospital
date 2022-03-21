using Hospital.Host.Data;
using Hospital.Host.Models.Dtos;

namespace Hospital.Host.Repositories.Interfaces
{
    public interface ISpecializationRepository
    {
        Task<PaginatedItems<SpecializationDto>> GetSpecializations(int pageIndex, int pageSize);
        Task<SpecializationDto?> GetSpecializationById(int id);
    }
}
