using System.Threading.Tasks;
using Hospital.BusinessLogic.Models.Responses;
using Hospital.DataAccess.Models.Dtos;

namespace Hospital.BusinessLogic.Services.Interfaces
{
    public interface ISpecializationService
    {
        Task<PaginatedItemsResponse<SpecializationDto>> GetSpecializations(int pageIndex, int pageSize);
        Task<SpecializationDto> GetSpecializationById(int id);
        Task<IdResponse<int>> AddSpecialization(string name, string description);
        Task<int?> UpdateSpecialization(SpecializationDto specializationDto);
        Task<int?> DeleteSpecialization(int id);
    }
}