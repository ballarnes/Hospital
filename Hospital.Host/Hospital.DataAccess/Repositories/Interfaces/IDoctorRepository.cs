using System.Threading.Tasks;
using Hospital.DataAccess.Data;
using Hospital.DataAccess.Models.Entities;

namespace Hospital.DataAccess.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        Task<PaginatedItems<Doctor>> GetDoctors(int pageIndex, int pageSize);
        Task<Doctor> GetDoctorById(int id);
        Task<PaginatedItems<Doctor>> GetDoctorsBySpecializationId(int id);
        Task<int?> AddDoctor(string name, string surname, int specializationId);
        Task<int?> UpdateDoctor(int id, string name, string surname, int specializationId);
        Task<int?> DeleteDoctor(int id);
    }
}
