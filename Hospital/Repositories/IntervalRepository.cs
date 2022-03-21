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
    }
}
