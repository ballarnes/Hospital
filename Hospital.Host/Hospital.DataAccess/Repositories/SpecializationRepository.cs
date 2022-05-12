using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Hospital.DataAccess.Data;
using Hospital.DataAccess.Repositories.Interfaces;
using Infrastructure.Connection.Interfaces;
using Dapper;
using Hospital.DataAccess.Models.Entities;
using Hospital.DataAccess.Models.Dtos;
using Hospital.DataAccess.Infrastructure;

namespace Hospital.DataAccess.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly IDbConnectionWrapper _connection;

        public SpecializationRepository(
            IDbConnectionWrapper connection)
        {
            _connection = connection;
        }

        public async Task<PaginatedItems<Specialization>> GetSpecializations(int pageIndex, int pageSize)
        {
            var totalCount = await _connection.Connection
                    .QueryAsync<int>("SELECT COUNT(*) FROM Specializations");

            var result = await _connection.Connection
                .QueryAsync<Specialization>($"SELECT * FROM Specializations ORDER BY Id OFFSET {pageIndex * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            return new PaginatedItems<Specialization>()
            {
                PagesCount = (int)Math.Round((Convert.ToDecimal(totalCount.FirstOrDefault()) / pageSize), MidpointRounding.ToPositiveInfinity),
                TotalCount = totalCount.FirstOrDefault(),
                Data = result
            };
        }

        public async Task<Specialization> GetSpecializationById(int id)
        {
            var result = await _connection.Connection
                .QueryAsync<SpecializationDto>($"SELECT * FROM Specializations WHERE Id = {id}");

            var specialization = result.FirstOrDefault();

            if (specialization == null)
            {
                return null;
            }

            return new Specialization()
            {
                Id = specialization.Id,
                Name = specialization.Name,
                Description = specialization.Description
            };
        }

        public async Task<int?> AddSpecialization(string name, string description)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@name", name, DbType.String);
            parameters.Add("@description", description, DbType.String);
            parameters.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await _connection.Connection.ExecuteAsync("AddOrUpdateSpecializations", parameters, commandType: CommandType.StoredProcedure);

            var result = parameters.Get<int>("@id");

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> UpdateSpecialization(Specialization specialization)
        {
            var result = await _connection.Connection.ExecuteAsync(
                "AddOrUpdateSpecializations",
                StoredProcedureManager.GetParameters(specialization),
                commandType: CommandType.StoredProcedure);

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> DeleteSpecialization(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.Int32);

            var result = await _connection.Connection.ExecuteAsync("DeleteSpecializations", parameters, commandType: CommandType.StoredProcedure);

            if (result == default)
            {
                return null;
            }

            return result;
        }
    }
}
