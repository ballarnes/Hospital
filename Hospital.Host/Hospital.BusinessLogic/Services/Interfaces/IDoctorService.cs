using System.Threading.Tasks;
using Hospital.BusinessLogic.Models.Responses;
using Hospital.DataAccess.Models.Dtos;

namespace Hospital.BusinessLogic.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<PaginatedItemsResponse<DoctorDto>> GetDoctors(int pageIndex, int pageSize);
        Task<DoctorDto> GetDoctorById(int id);
        Task<PaginatedItemsResponse<DoctorDto>> GetDoctorsBySpecializationId(int id);
        Task<IdResponse<int>> AddDoctor(string name, string surname, int specializationId);
        Task<int?> UpdateDoctor(DoctorDto doctorDto);
        Task<int?> DeleteDoctor(int id);
    }
}