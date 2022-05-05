using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hospital.DataAccess.Data;
using Hospital.DataAccess.Repositories.Interfaces;
using Infrastructure.Connection.Interfaces;
using Dapper;
using Hospital.DataAccess.Models.Entities;

namespace Hospital.DataAccess.Repositories
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly IDbConnectionWrapper _connection;

        public OfficeRepository(
            IDbConnectionWrapper connection)
        {
            _connection = connection;
        }

        public async Task<PaginatedItems<Office>> GetOffices(int pageIndex, int pageSize)
        {
            var totalCount = await _connection.Connection
                    .QueryAsync<int>("SELECT COUNT(*) FROM Offices");

            var result = await _connection.Connection
                .QueryAsync<Office>($"SELECT * FROM Offices ORDER BY Id OFFSET {pageIndex * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            return new PaginatedItems<Office>()
            {
                PagesCount = (int)Math.Round((Convert.ToDecimal(totalCount.FirstOrDefault()) / pageSize), MidpointRounding.ToPositiveInfinity),
                TotalCount = totalCount.FirstOrDefault(),
                Data = result
            };
        }

        public async Task<Office> GetOfficeById(int id)
        {
            var result = await _connection.Connection
                .QueryAsync<Office>($"SELECT * FROM Offices WHERE Id = {id}");

            var office = result.FirstOrDefault();

            if (office == null)
            {
                return null;
            }

            return new Office()
            {
                Id = office.Id,
                Number = office.Number
            };
        }

        public async Task<List<Office>> GetFreeOfficesByDate(DateTime date)
        {
            var allOffices = await _connection.Connection
                .QueryAsync<Office>(@$"
                SELECT * FROM Offices
                ORDER BY Id");

            var appointmentsByDate = await _connection.Connection
                .QueryAsync<Appointment, Office, Appointment>($@"
                SELECT * FROM Appointments a
                INNER JOIN Offices o ON a.OfficeId = o.Id
                WHERE CONVERT(varchar, a.StartDate, 126) LIKE '{date.ToString("yyyy-MM-ddTHH:mm:ss")}'", (a, o) =>
                {
                    var appointment = a;

                    if (appointment.Office == null)
                    {
                        appointment.Office = o;
                    }

                    return appointment;
                });

            var freeOffices = new List<Office>(allOffices);

            foreach (var appointment in appointmentsByDate)
            {
                if (freeOffices.Any(o => o.Id.Equals(appointment.Office.Id)))
                {
                    freeOffices.Remove(freeOffices.First(o => o.Id.Equals(appointment.Office.Id)));
                }
            }

            return freeOffices.ToList();
        }

        public async Task<int?> AddOffice(int number)
        {
            var command = new SqlCommand("AddOrUpdateOffices");
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = (SqlConnection)_connection.Connection;

            var numberParam = new SqlParameter
            {
                ParameterName = "@number",
                SqlDbType = SqlDbType.Int,
                Value = number
            };

            var returnParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };

            command.Parameters.Add(numberParam);
            command.Parameters.Add(returnParam);

            await command.ExecuteNonQueryAsync();

            if (returnParam.Value == null)
            {
                return null;
            }

            return (int)returnParam.Value;
        }

        public async Task<int?> UpdateOffice(int id, int number)
        {
            var command = new SqlCommand("AddOrUpdateOffices");
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = (SqlConnection)_connection.Connection;

            var idParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.Int,
                Value = id
            };

            var numberParam = new SqlParameter
            {
                ParameterName = "@number",
                SqlDbType = SqlDbType.Int,
                Value = number
            };

            command.Parameters.Add(idParam);
            command.Parameters.Add(numberParam);

            var result = await command.ExecuteNonQueryAsync();

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> DeleteOffice(int id)
        {
            var command = new SqlCommand("DeleteOffices");
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
