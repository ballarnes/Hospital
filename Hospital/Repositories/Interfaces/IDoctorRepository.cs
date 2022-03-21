using Hospital.Host.Data;
using Hospital.Host.Models.Dtos;

namespace Hospital.Host.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        Task<PaginatedItems<DoctorDto>> GetDoctors(int pageIndex, int pageSize);
        Task<DoctorDto?> GetDoctorById(int id);
    }
}
