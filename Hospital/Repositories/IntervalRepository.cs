using Hospital.Host.Connection.Interfaces;
using Hospital.Host.Data;
using Hospital.Host.Models.Dtos;
using Hospital.Host.Repositories.Interfaces;

namespace Hospital.Host.Repositories
{
    public class IntervalRepository : IIntervalRepository
    {
        private readonly IDbConnectionWrapper _connection;

        public IntervalRepository(
            IDbConnectionWrapper connection)
        {
            _connection = connection;
        }

        public async Task<PaginatedItems<IntervalDto>> GetIntervals(int pageIndex, int pageSize)
        {
            var totalCount = await _connection.Connection
                    .QueryAsync<int>("SELECT COUNT(*) FROM Intervals");

            var result = await _connection.Connection
                .QueryAsync<IntervalDto>($"SELECT * FROM Intervals ORDER BY Id OFFSET {pageIndex * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            return new PaginatedItems<IntervalDto>()
            {
                PagesCount = (int)Math.Round((Convert.ToDecimal(totalCount.FirstOrDefault()) / pageSize), MidpointRounding.ToPositiveInfinity),
                TotalCount = totalCount.FirstOrDefault(),
                Data = result
            };
        }

        public async Task<IntervalDto?> GetIntervalById(int id)
        {
            var result = await _connection.Connection
                .QueryAsync<IntervalDto>($"SELECT * FROM Intervals WHERE Id = {id}");

            var interval = result.FirstOrDefault();

            if (interval == null)
            {
                return null;
            }

            return new IntervalDto()
            {
                Id = interval.Id,
                Start = interval.Start,
                End = interval.End
            };
        }

        public async Task<int?> AddInterval(TimeSpan start, TimeSpan end)
        {
            var command = new SqlCommand("AddOrUpdateIntervals");
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = (SqlConnection)_connection.Connection;

            var startParam = new SqlParameter
            {
                ParameterName = "@start",
                SqlDbType = SqlDbType.Time,
                Value = start
            };

            var endParam = new SqlParameter
            {
                ParameterName = "@end",
                SqlDbType = SqlDbType.Time,
                Value = end
            };

            var returnParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };

            command.Parameters.Add(startParam);
            command.Parameters.Add(endParam);
            command.Parameters.Add(returnParam);

            await command.ExecuteNonQueryAsync();

            if (returnParam.Value == null)
            {
                return null;
            }

            return (int)returnParam.Value;
        }

        public async Task<int?> UpdateInterval(int id, TimeSpan start, TimeSpan end)
        {
            var command = new SqlCommand("AddOrUpdateIntervals");
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = (SqlConnection)_connection.Connection;

            var idParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.Int,
                Value = id
            };

            var startParam = new SqlParameter
            {
                ParameterName = "@start",
                SqlDbType = SqlDbType.Time,
                Value = start
            };

            var endParam = new SqlParameter
            {
                ParameterName = "@end",
                SqlDbType = SqlDbType.Time,
                Value = end
            };

            command.Parameters.Add(idParam);
            command.Parameters.Add(startParam);
            command.Parameters.Add(endParam);

            var result = await command.ExecuteNonQueryAsync();

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> DeleteInterval(int id)
        {
            var command = new SqlCommand("DeleteIntervals");
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
