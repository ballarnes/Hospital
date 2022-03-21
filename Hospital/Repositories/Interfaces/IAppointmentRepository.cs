using Hospital.Host.Data;
using Hospital.Host.Models.Dtos;

namespace Hospital.Host.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<PaginatedItems<AppointmentDto>> GetAppointments(int pageIndex, int pageSize);
        Task<AppointmentDto?> GetAppointmentById(int id);
    }
}
