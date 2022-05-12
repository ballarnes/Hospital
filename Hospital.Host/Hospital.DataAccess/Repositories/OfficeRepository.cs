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
using Hospital.DataAccess.Infrastructure;

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
            var parameters = new DynamicParameters();
            parameters.Add("@number", number, DbType.Int32);
            parameters.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await _connection.Connection.ExecuteAsync("AddOrUpdateOffices", parameters, commandType: CommandType.StoredProcedure);

            var result = parameters.Get<int>("@id");

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> UpdateOffice(Office office)
        {
            var result = await _connection.Connection.ExecuteAsync(
                "AddOrUpdateOffices",
                StoredProcedureManager.GetParameters(office),
                commandType: CommandType.StoredProcedure);

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> DeleteOffice(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.Int32);

            var result = await _connection.Connection.ExecuteAsync("DeleteOffices", parameters, commandType: CommandType.StoredProcedure);

            if (result == default)
            {
                return null;
            }

            return result;
        }
    }
}
