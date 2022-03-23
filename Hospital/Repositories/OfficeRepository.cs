using Hospital.Host.Connection.Interfaces;
using Hospital.Host.Data;
using Hospital.Host.Data.Entities;
using Hospital.Host.Models.Dtos;
using Hospital.Host.Repositories.Interfaces;

namespace Hospital.Host.Repositories
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

        public async Task<Office?> GetOfficeById(int id)
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
