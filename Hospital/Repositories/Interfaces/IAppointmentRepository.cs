using Hospital.Host.Data;
using Hospital.Host.Data.Entities;

namespace Hospital.Host.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<PaginatedItems<Appointment>> GetAppointments(int pageIndex, int pageSize);
        Task<Appointment?> GetAppointmentById(int id);
        Task<int?> AddAppointment(int doctorId, int intervalId, int officeId, DateTime date, string patientName);
        Task<int?> UpdateAppointment(int id, int doctorId, int intervalId, int officeId, DateTime date, string patientName);
        Task<int?> DeleteAppointment(int id);
    }
}
