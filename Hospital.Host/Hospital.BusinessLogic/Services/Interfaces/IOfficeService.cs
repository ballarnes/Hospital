using System;
using System.Threading.Tasks;
using Hospital.BusinessLogic.Models.Responses;
using Hospital.DataAccess.Models.Dtos;

namespace Hospital.BusinessLogic.Services.Interfaces
{
    public interface IOfficeService
    {
        Task<PaginatedItemsResponse<OfficeDto>> GetOffices(int pageIndex, int pageSize);
        Task<OfficeDto> GetOfficeById(int id);
        Task<ArrayResponse<OfficeDto>> GetFreeOfficesByDate(DateTime date);
        Task<IdResponse<int>> AddOffice(int number);
        Task<int?> UpdateOffice(OfficeDto officeDto);
        Task<int?> DeleteOffice(int id);
    }
}