using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Responses;

namespace Hospital.Host.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<PaginatedItemsResponse<DoctorDto>?> GetDoctors(int pageIndex, int pageSize);
        Task<DoctorDto?> GetDoctorById(int id);
        Task<IdResponse<int>?> AddDoctor(string name, string surname, int specializationId);
        Task<int?> UpdateDoctor(int id, string name, string surname, int specializationId);
        Task<int?> DeleteDoctor(int id);
    }
}
