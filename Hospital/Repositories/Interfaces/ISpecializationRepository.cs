using Hospital.Host.Data;
using Hospital.Host.Data.Entities;

namespace Hospital.Host.Repositories.Interfaces
{
    public interface ISpecializationRepository
    {
        Task<PaginatedItems<Specialization>> GetSpecializations(int pageIndex, int pageSize);
        Task<Specialization?> GetSpecializationById(int id);
        Task<int?> AddSpecialization(string name, string description);
        Task<int?> UpdateSpecialization(int id, string name, string description);
        Task<int?> DeleteSpecialization(int id);
    }
}
