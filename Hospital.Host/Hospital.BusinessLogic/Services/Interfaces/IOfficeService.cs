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
        Task<PaginatedItemsResponse<OfficeDto>> GetFreeOfficesByIntervalDate(int intervalId, DateTime date);
        Task<IdResponse<int>> AddOffice(int number);
        Task<int?> UpdateOffice(int id, int number);
        Task<int?> DeleteOffice(int id);
    }
}