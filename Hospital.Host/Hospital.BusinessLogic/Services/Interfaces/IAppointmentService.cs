using System;
using System.Threading.Tasks;
using Hospital.BusinessLogic.Models.Responses;
using Hospital.DataAccess.Models.Dtos;

namespace Hospital.BusinessLogic.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<PaginatedItemsResponse<AppointmentDto>> GetAppointments(int pageIndex, int pageSize);
        Task<PaginatedItemsResponse<AppointmentDto>> GetUpcomingAppointments(int pageIndex, int pageSize, string name);
        Task<ArrayResponse<AppointmentDto>> GetAppointmentsByDoctorDate(int doctorId, DateTime date);
        Task<AppointmentDto> GetAppointmentById(int id);
        Task<IdResponse<int>> AddAppointment(int doctorId, int officeId, DateTime startDate, DateTime endDate, string patientName);
        Task<int?> UpdateAppointment(AppointmentDto appointmentDto);
        Task<int?> DeleteAppointment(int id);
    }
}