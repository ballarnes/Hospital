using System;
using System.Threading.Tasks;
using Hospital.DataAccess.Data;
using Hospital.DataAccess.Models.Entities;

namespace Hospital.DataAccess.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<PaginatedItems<Appointment>> GetAppointments(int pageIndex, int pageSize);
        Task<PaginatedItems<Appointment>> GetUpcomingAppointments(int pageIndex, int pageSize, string name);
        Task<Appointment> GetAppointmentById(int id);
        Task<int?> AddAppointment(int doctorId, int intervalId, int officeId, DateTime date, string patientName);
        Task<int?> UpdateAppointment(int id, int doctorId, int intervalId, int officeId, DateTime date, string patientName);
        Task<int?> DeleteAppointment(int id);
    }
}
