using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hospital.DataAccess.Data;
using Hospital.DataAccess.Repositories.Interfaces;
using Hospital.DataAccess.Models.Entities;
using Infrastructure.Connection.Interfaces;
using Dapper;
using Hospital.DataAccess.Infrastructure;
using Hospital.DataAccess.Infrastructure.Interfaces;

namespace Hospital.DataAccess.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IDbConnectionWrapper _connection;
        private readonly IStoredProcedureManager _storedProcedureManager;

        public AppointmentRepository(
            IDbConnectionWrapper connection,
            IStoredProcedureManager storedProcedureManager)
        {
            _connection = connection;
            _storedProcedureManager = storedProcedureManager;
        }

        public async Task<PaginatedItems<Appointment>> GetAppointments(int pageIndex, int pageSize)
        {
            var totalCount = await _connection.Connection
                    .QueryAsync<int>("SELECT COUNT(*) FROM Appointments");

            var result = await _connection.Connection
                .QueryAsync<Appointment, Doctor, Specialization, Office, Appointment>(@$"
                SELECT a.*, d.*, s.*, o.*
                FROM Appointments a
                INNER JOIN Doctors d ON a.DoctorId = d.Id
                INNER JOIN Specializations s ON d.SpecializationId = s.Id
                INNER JOIN Offices o ON a.OfficeId = o.Id
                ORDER BY a.Id
                OFFSET {pageIndex * pageSize} ROWS
                FETCH NEXT {pageSize} ROWS ONLY", (a, d, s, o) =>
                {
                    var appointment = a;

                    if (appointment.Doctor == null)
                    {
                        appointment.Doctor = d;
                    }

                    if (appointment.Doctor.Specialization == null)
                    {
                        appointment.Doctor.Specialization = s;
                    }

                    if (appointment.Office == null)
                    {
                        appointment.Office = o;
                    }

                    return appointment;
                });

            return new PaginatedItems<Appointment>()
            {
                PagesCount = (int)Math.Round((Convert.ToDecimal(totalCount.FirstOrDefault()) / pageSize), MidpointRounding.ToPositiveInfinity),
                TotalCount = totalCount.FirstOrDefault(),
                Data = result
            };
        }

        public async Task<PaginatedItems<Appointment>> GetUpcomingAppointments(int pageIndex, int pageSize, string name)
        {
            var result = await _connection.Connection
                .QueryAsync<Appointment, Doctor, Specialization, Office, Appointment>(@$"
                SELECT a.*, d.*, s.*, o.*
                FROM Appointments a
                INNER JOIN Doctors d ON a.DoctorId = d.Id
                INNER JOIN Specializations s ON d.SpecializationId = s.Id
                INNER JOIN Offices o ON a.OfficeId = o.Id
                WHERE a.PatientName = '{name}' AND DATEADD(day, 1, a.StartDate) >= GETDATE()
                ORDER BY a.StartDate", (a, d, s, o) =>
                {
                    var appointment = a;

                    if (appointment.Doctor == null)
                    {
                        appointment.Doctor = d;
                    }

                    if (appointment.Doctor.Specialization == null)
                    {
                        appointment.Doctor.Specialization = s;
                    }

                    if (appointment.Office == null)
                    {
                        appointment.Office = o;
                    }

                    return appointment;
                });

            var upcomingAppointments = result
                .ToList();

            upcomingAppointments.RemoveAll(a => a.StartDate.Hour < DateTime.Now.Hour && a.StartDate.Date == DateTime.Now.Date);

            var returnAppointments = new List<Appointment>();

            for (var i = 0; i < pageSize; i++)
            {
                if (i + pageIndex * pageSize == upcomingAppointments.Count)
                {
                    break;
                }
                returnAppointments.Add(upcomingAppointments[i + pageIndex * pageSize]);
            }

            return new PaginatedItems<Appointment>()
            {
                PagesCount = (int)Math.Round((Convert.ToDecimal(upcomingAppointments.Count) / pageSize), MidpointRounding.ToPositiveInfinity),
                TotalCount = upcomingAppointments.Count,
                Data = returnAppointments
            };
        }

        public async Task<List<Appointment>> GetAppointmentsByDoctorDate(int doctorId, DateTime date)
        {
            var result = await _connection.Connection
                .QueryAsync<Appointment, Doctor, Specialization, Office, Appointment>(@$"
                SELECT a.*, d.*, s.*, o.*
                FROM Appointments a
                INNER JOIN Doctors d ON a.DoctorId = d.Id
                INNER JOIN Specializations s ON d.SpecializationId = s.Id
                INNER JOIN Offices o ON a.OfficeId = o.Id
                WHERE a.DoctorId = '{doctorId}' AND DATEADD(dd, 0, DATEDIFF(dd, 0, a.StartDate)) = '{date.Date.ToString("yyyy-MM-ddTHH:mm:ss")}'
                ORDER BY a.StartDate", (a, d, s, o) =>
                {
                    var appointment = a;

                    if (appointment.Doctor == null)
                    {
                        appointment.Doctor = d;
                    }

                    if (appointment.Doctor.Specialization == null)
                    {
                        appointment.Doctor.Specialization = s;
                    }

                    if (appointment.Office == null)
                    {
                        appointment.Office = o;
                    }

                    return appointment;
                });

            return result.ToList();
        }

        public async Task<Appointment> GetAppointmentById(int id)
        {
            var result = await _connection.Connection
                .QueryAsync<Appointment, Doctor, Specialization, Office, Appointment>(@$"
                SELECT a.*, d.*, s.*, o.*
                FROM Appointments a
                INNER JOIN Doctors d ON a.DoctorId = d.Id
                INNER JOIN Specializations s ON d.SpecializationId = s.Id
                INNER JOIN Offices o ON a.OfficeId = o.Id
                WHERE a.Id = {id}", (a, d, s, o) =>
                {
                    var appointment = a;

                    if (appointment.Doctor == null)
                    {
                        appointment.Doctor = d;
                    }

                    if (appointment.Doctor.Specialization == null)
                    {
                        appointment.Doctor.Specialization = s;
                    }

                    if (appointment.Office == null)
                    {
                        appointment.Office = o;
                    }

                    return appointment;
                });

            var doctor = result.FirstOrDefault();

            if (doctor == null)
            {
                return null;
            }

            return new Appointment()
            {
                Id = doctor.Id,
                StartDate = doctor.StartDate,
                EndDate = doctor.EndDate,
                Doctor = doctor.Doctor,
                DoctorId = doctor.DoctorId,
                PatientName = doctor.PatientName,
                OfficeId = doctor.OfficeId,
                Office = doctor.Office
            };
        }

        public async Task<int?> AddAppointment(int doctorId, int officeId, DateTime startDate, DateTime endDate, string patientName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@doctorId", doctorId, DbType.Int32);
            parameters.Add("@officeId", officeId, DbType.Int32);
            parameters.Add("@startDate", startDate, DbType.DateTime);
            parameters.Add("@endDate", endDate, DbType.DateTime);
            parameters.Add("@patientName", patientName, DbType.String);
            parameters.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await _connection.Connection.ExecuteAsync("AddOrUpdateAppointments", parameters, commandType: CommandType.StoredProcedure);

            var result = parameters.Get<int>("@id");

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> UpdateAppointment(Appointment appointment)
        {
            var result = await _connection.Connection.ExecuteAsync(
                "AddOrUpdateAppointments",
                _storedProcedureManager.GetParameters(appointment),
                commandType: CommandType.StoredProcedure);

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> DeleteAppointment(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.Int32);

            var result = await _connection.Connection.ExecuteAsync("DeleteAppointments", parameters, commandType: CommandType.StoredProcedure);

            if (result == default)
            {
                return null;
            }

            return result;
        }
    }
}
