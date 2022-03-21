using Hospital.Host.Connection.Interfaces;
using Hospital.Host.Data;
using Hospital.Host.Models.Dtos;
using Hospital.Host.Repositories.Interfaces;

namespace Hospital.Host.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IDbConnectionWrapper _connection;

        public AppointmentRepository(
            IDbConnectionWrapper connection)
        {
            _connection = connection;
        }

        public async Task<PaginatedItems<AppointmentDto>> GetAppointments(int pageIndex, int pageSize)
        {
            var totalCount = await _connection.Connection
                    .QueryAsync<int>("SELECT COUNT(*) FROM Appointments");

            var result = await _connection.Connection
                .QueryAsync<AppointmentDto, DoctorDto, IntervalDto, OfficeDto, AppointmentDto>(@$"
                SELECT a.*, d.*, i.*, o.*
                FROM Appointments a
                INNER JOIN Doctors d ON a.DoctorId = d.Id
                INNER JOIN Intervals i ON a.IntervalId = i.Id
                INNER JOIN Offices o ON a.OfficeId = o.Id
                ORDER BY a.Id
                OFFSET {pageIndex * pageSize} ROWS
                FETCH NEXT {pageSize} ROWS ONLY", (a, d, i, o) =>
                {
                    var appointment = a;

                    if (appointment.Doctor == null)
                    {
                        appointment.Doctor = new DoctorDto();
                    }

                    if (appointment.Interval == null)
                    {
                        appointment.Interval = new IntervalDto();
                    }

                    if (appointment.Office == null)
                    {
                        appointment.Office = new OfficeDto();
                    }

                    appointment.Doctor = d;
                    appointment.Interval = i;
                    appointment.Office = o;
                    return appointment;
                });

            return new PaginatedItems<AppointmentDto>()
            {
                PagesCount = (int)Math.Round((Convert.ToDecimal(totalCount.FirstOrDefault()) / pageSize), MidpointRounding.ToPositiveInfinity),
                TotalCount = totalCount.FirstOrDefault(),
                Data = result
            };
        }

        public async Task<AppointmentDto?> GetAppointmentById(int id)
        {
            var result = await _connection.Connection
                .QueryAsync<AppointmentDto, DoctorDto, IntervalDto, OfficeDto, AppointmentDto>(@$"
                SELECT a.*, d.*, i.*, o.*
                FROM Appointments a
                INNER JOIN Doctors d ON a.DoctorId = d.Id
                INNER JOIN Intervals i ON a.IntervalId = i.Id
                INNER JOIN Offices o ON a.OfficeId = o.Id
                WHERE d.Id = {id}", (a, d, i, o) =>
                {
                    var appointment = a;

                    if (appointment.Doctor == null)
                    {
                        appointment.Doctor = new DoctorDto();
                    }

                    if (appointment.Interval == null)
                    {
                        appointment.Interval = new IntervalDto();
                    }

                    if (appointment.Office == null)
                    {
                        appointment.Office = new OfficeDto();
                    }

                    appointment.Doctor = d;
                    appointment.Interval = i;
                    appointment.Office = o;
                    return appointment;
                });

            var doctor = result.FirstOrDefault();

            if (doctor == null)
            {
                return null;
            }

            return new AppointmentDto()
            {
                Id = doctor.Id,
                Date = doctor.Date,
                Doctor = doctor.Doctor,
                DoctorId = doctor.DoctorId,
                Interval = doctor.Interval,
                PatientName = doctor.PatientName,
                IntervalId = doctor.IntervalId,
                OfficeId = doctor.OfficeId,
                Office = doctor.Office
            };
        }
    }
}
