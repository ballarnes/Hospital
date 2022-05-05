using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hospital.DataAccess.Data;
using Hospital.DataAccess.Repositories.Interfaces;
using Hospital.DataAccess.Models.Entities;
using Infrastructure.Connection.Interfaces;
using Dapper;

namespace Hospital.DataAccess.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IDbConnectionWrapper _connection;

        public AppointmentRepository(
            IDbConnectionWrapper connection)
        {
            _connection = connection;
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
            var command = new SqlCommand("AddOrUpdateAppointments");
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = (SqlConnection)_connection.Connection;

            var doctorIdParam = new SqlParameter
            {
                ParameterName = "@doctorId",
                SqlDbType = SqlDbType.Int,
                Value = doctorId
            };

            var officeIdParam = new SqlParameter
            {
                ParameterName = "@officeId",
                SqlDbType = SqlDbType.Int,
                Value = officeId
            };

            var startDateParam = new SqlParameter
            {
                ParameterName = "@startDate",
                SqlDbType = SqlDbType.DateTime,
                Value = startDate
            };

            var endDateParam = new SqlParameter
            {
                ParameterName = "@endDate",
                SqlDbType = SqlDbType.DateTime,
                Value = endDate
            };

            var patientNameParam = new SqlParameter
            {
                ParameterName = "@patientName",
                SqlDbType = SqlDbType.NVarChar,
                Value = patientName
            };

            var returnParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };

            command.Parameters.Add(doctorIdParam);
            command.Parameters.Add(officeIdParam);
            command.Parameters.Add(startDateParam);
            command.Parameters.Add(endDateParam);
            command.Parameters.Add(patientNameParam);
            command.Parameters.Add(returnParam);

            await command.ExecuteNonQueryAsync();

            if (returnParam.Value == null)
            {
                return null;
            }

            return (int)returnParam.Value;
        }

        public async Task<int?> UpdateAppointment(int id, int doctorId, int officeId, DateTime startDate, DateTime endDate, string patientName)
        {
            var command = new SqlCommand("AddOrUpdateAppointments");
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = (SqlConnection)_connection.Connection;

            var idParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.Int,
                Value = id
            };

            var doctorIdParam = new SqlParameter
            {
                ParameterName = "@doctorId",
                SqlDbType = SqlDbType.Int,
                Value = doctorId
            };

            var officeIdParam = new SqlParameter
            {
                ParameterName = "@officeId",
                SqlDbType = SqlDbType.Int,
                Value = officeId
            };

            var startDateParam = new SqlParameter
            {
                ParameterName = "@startDate",
                SqlDbType = SqlDbType.DateTime,
                Value = startDate
            };

            var endDateParam = new SqlParameter
            {
                ParameterName = "@endDate",
                SqlDbType = SqlDbType.DateTime,
                Value = endDate
            };

            var patientNameParam = new SqlParameter
            {
                ParameterName = "@patientName",
                SqlDbType = SqlDbType.NVarChar,
                Value = patientName
            };

            command.Parameters.Add(idParam);
            command.Parameters.Add(doctorIdParam);
            command.Parameters.Add(officeIdParam);
            command.Parameters.Add(startDateParam);
            command.Parameters.Add(endDateParam);
            command.Parameters.Add(patientNameParam);

            var result = await command.ExecuteNonQueryAsync();

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> DeleteAppointment(int id)
        {
            var command = new SqlCommand("DeleteAppointments");
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = (SqlConnection)_connection.Connection;

            var idParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.Int,
                Value = id
            };

            command.Parameters.Add(idParam);

            var result = await command.ExecuteNonQueryAsync();

            if (result == default)
            {
                return null;
            }

            return result;
        }
    }
}
