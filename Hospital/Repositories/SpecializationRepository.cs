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
    }
}
