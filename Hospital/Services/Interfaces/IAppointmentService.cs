using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Responses;

namespace Hospital.Host.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<PaginatedItemsResponse<AppointmentDto>?> GetAppointments(int pageIndex, int pageSize);
        Task<AppointmentDto?> GetAppointmentById(int id);
    }
}
