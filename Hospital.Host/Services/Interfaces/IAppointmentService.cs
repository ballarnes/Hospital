using Hospital.Host.Models.Dtos;
using Hospital.Host.Models.Responses;

namespace Hospital.Host.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<PaginatedItemsResponse<AppointmentDto>?> GetAppointments(int pageIndex, int pageSize);
        Task<PaginatedItemsResponse<AppointmentDto>?> GetUpcomingAppointments(int pageIndex, int pageSize, string name);
        Task<AppointmentDto?> GetAppointmentById(int id);
        Task<IdResponse<int>?> AddAppointment(int doctorId, int intervalId, int officeId, DateTime date, string patientName);
        Task<int?> UpdateAppointment(int id, int doctorId, int intervalId, int officeId, DateTime date, string patientName);
        Task<int?> DeleteAppointment(int id);
    }
}
