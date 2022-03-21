using Hospital.Host.Connection.Interfaces;
using Hospital.Host.Data;
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

        public async Task<PaginatedItems<OfficeDto>> GetOffices(int pageIndex, int pageSize)
        {
            var totalCount = await _connection.Connection
                    .QueryAsync<int>("SELECT COUNT(*) FROM Offices");

            var result = await _connection.Connection
                .QueryAsync<OfficeDto>($"SELECT * FROM Offices ORDER BY Id OFFSET {pageIndex * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            return new PaginatedItems<OfficeDto>()
            {
                PagesCount = (int)Math.Round((Convert.ToDecimal(totalCount.FirstOrDefault()) / pageSize), MidpointRounding.ToPositiveInfinity),
                TotalCount = totalCount.FirstOrDefault(),
                Data = result
            };
        }

        public async Task<OfficeDto?> GetOfficeById(int id)
        {
            var result = await _connection.Connection
                .QueryAsync<OfficeDto>($"SELECT * FROM Offices WHERE Id = {id}");

            var office = result.FirstOrDefault();

            if (office == null)
            {
                return null;
            }

            return new OfficeDto()
            {
                Id = office.Id,
                Number = office.Number
            };
        }
    }
}
