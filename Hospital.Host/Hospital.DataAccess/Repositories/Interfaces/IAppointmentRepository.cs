using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.DataAccess.Data;
using Hospital.DataAccess.Models.Entities;

namespace Hospital.DataAccess.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<PaginatedItems<Appointment>> GetAppointments(int pageIndex, int pageSize);
        Task<PaginatedItems<Appointment>> GetUpcomingAppointments(int pageIndex, int pageSize, string name);
        Task<List<Appointment>> GetAppointmentsByDoctorDate(int doctorId, DateTime date);
        Task<Appointment> GetAppointmentById(int id);
        Task<int?> AddAppointment(int doctorId, int officeId, DateTime startDate, DateTime endDate, string patientName);
        Task<int?> UpdateAppointment(int id, int doctorId, int officeId, DateTime startDate, DateTime endDate, string patientName);
        Task<int?> DeleteAppointment(int id);
    }
}
