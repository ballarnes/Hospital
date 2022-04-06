using Hospital.Host.Data;
using Hospital.Host.Data.Entities;

namespace Hospital.Host.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        Task<PaginatedItems<Doctor>> GetDoctors(int pageIndex, int pageSize);
        Task<Doctor?> GetDoctorById(int id);
        Task<PaginatedItems<Doctor>> GetDoctorsBySpecializationId(int id);
        Task<int?> AddDoctor(string name, string surname, int specializationId);
        Task<int?> UpdateDoctor(int id, string name, string surname, int specializationId);
        Task<int?> DeleteDoctor(int id);
    }
}
