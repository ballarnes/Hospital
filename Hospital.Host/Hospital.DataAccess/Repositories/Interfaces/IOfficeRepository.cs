using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.DataAccess.Data;
using Hospital.DataAccess.Models.Entities;

namespace Hospital.DataAccess.Repositories.Interfaces
{
    public interface IOfficeRepository
    {
        Task<PaginatedItems<Office>> GetOffices(int pageIndex, int pageSize);
        Task<Office> GetOfficeById(int id);
        Task<List<Office>> GetFreeOfficesByDate(DateTime date);
        Task<int?> AddOffice(int number);
        Task<int?> UpdateOffice(int id, int number);
        Task<int?> DeleteOffice(int id);
    }
}
