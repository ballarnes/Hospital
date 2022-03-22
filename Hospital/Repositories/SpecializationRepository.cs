using Hospital.Host.Connection.Interfaces;
using Hospital.Host.Data;
using Hospital.Host.Models.Dtos;
using Hospital.Host.Repositories.Interfaces;

namespace Hospital.Host.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly IDbConnectionWrapper _connection;

        public SpecializationRepository(
            IDbConnectionWrapper connection)
        {
            _connection = connection;
        }

        public async Task<PaginatedItems<SpecializationDto>> GetSpecializations(int pageIndex, int pageSize)
        {
            var totalCount = await _connection.Connection
                    .QueryAsync<int>("SELECT COUNT(*) FROM Specializations");

            var result = await _connection.Connection
                .QueryAsync<SpecializationDto>($"SELECT * FROM Specializations ORDER BY Id OFFSET {pageIndex * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            return new PaginatedItems<SpecializationDto>()
            {
                PagesCount = (int)Math.Round((Convert.ToDecimal(totalCount.FirstOrDefault()) / pageSize), MidpointRounding.ToPositiveInfinity),
                TotalCount = totalCount.FirstOrDefault(),
                Data = result
            };
        }

        public async Task<SpecializationDto?> GetSpecializationById(int id)
        {
            var result = await _connection.Connection
                .QueryAsync<SpecializationDto>($"SELECT * FROM Specializations WHERE Id = {id}");

            var specialization = result.FirstOrDefault();

            if (specialization == null)
            {
                return null;
            }

            return new SpecializationDto()
            {
                Id = specialization.Id,
                Name = specialization.Name,
                Description = specialization.Description
            };
        }

        public async Task<int?> AddSpecialization(string name, string description)
        {
            var command = new SqlCommand("AddOrUpdateSpecializations");
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = (SqlConnection)_connection.Connection;

            var nameParam = new SqlParameter
            {
                ParameterName = "@name",
                SqlDbType = SqlDbType.NVarChar,
                Value = name
            };

            var descriptionParam = new SqlParameter
            {
                ParameterName = "@description",
                SqlDbType = SqlDbType.NVarChar,
                Value = description
            };

            var returnParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };

            command.Parameters.Add(nameParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(returnParam);

            await command.ExecuteNonQueryAsync();

            if (returnParam.Value == null)
            {
                return null;
            }

            return (int)returnParam.Value;
        }

        public async Task<int?> UpdateSpecialization(int id, string name, string description)
        {
            var command = new SqlCommand("AddOrUpdateSpecializations");
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = (SqlConnection)_connection.Connection;

            var idParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.Int,
                Value = id
            };

            var nameParam = new SqlParameter
            {
                ParameterName = "@name",
                SqlDbType = SqlDbType.NVarChar,
                Value = name
            };

            var descriptionParam = new SqlParameter
            {
                ParameterName = "@description",
                SqlDbType = SqlDbType.NVarChar,
                Value = description
            };

            command.Parameters.Add(idParam);
            command.Parameters.Add(nameParam);
            command.Parameters.Add(descriptionParam);

            var result = await command.ExecuteNonQueryAsync();

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> DeleteSpecialization(int id)
        {
            var command = new SqlCommand("DeleteSpecializations");
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
