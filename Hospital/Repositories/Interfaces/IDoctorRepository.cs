using Hospital.Host.Data;
using Hospital.Host.Models.Dtos;

namespace Hospital.Host.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        Task<PaginatedItems<DoctorDto>> GetDoctors(int pageIndex, int pageSize);
        Task<DoctorDto?> GetDoctorById(int id);
        Task<int?> AddDoctor(string name, string surname, int specializationId);
        Task<int?> UpdateDoctor(int id, string name, string surname, int specializationId);
        Task<int?> DeleteDoctor(int id);
    }
}
