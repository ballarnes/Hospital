using Hospital.Host.Connection.Interfaces;
using Hospital.Host.Data;
using Hospital.Host.Data.Entities;
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

        public async Task<PaginatedItems<Interval>> GetIntervals(int pageIndex, int pageSize)
        {
            var totalCount = await _connection.Connection
                    .QueryAsync<int>("SELECT COUNT(*) FROM Intervals");

            var result = await _connection.Connection
                .QueryAsync<Interval>($"SELECT * FROM Intervals ORDER BY Id OFFSET {pageIndex * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            return new PaginatedItems<Interval>()
            {
                PagesCount = (int)Math.Round((Convert.ToDecimal(totalCount.FirstOrDefault()) / pageSize), MidpointRounding.ToPositiveInfinity),
                TotalCount = totalCount.FirstOrDefault(),
                Data = result
            };
        }

        public async Task<Interval?> GetIntervalById(int id)
        {
            var result = await _connection.Connection
                .QueryAsync<Interval>($"SELECT * FROM Intervals WHERE Id = {id}");

            var interval = result.FirstOrDefault();

            if (interval == null)
            {
                return null;
            }

            return new Interval()
            {
                Id = interval.Id,
                Start = interval.Start,
                End = interval.End
            };
        }

        public async Task<PaginatedItems<Interval>> GetFreeIntervalsByDoctorDate(int doctorId, DateTime date)
        {
            var allIntervals = await _connection.Connection
                .QueryAsync<Interval>(@$"
                SELECT * FROM Intervals
                ORDER BY Id");

            var appointmentsByDate = await _connection.Connection
                .QueryAsync<Appointment, Interval, Appointment>($@"
                SELECT * FROM Appointments a
                INNER JOIN Intervals i ON a.IntervalId = i.Id
                WHERE (a.DoctorId = {doctorId} AND a.Date LIKE '{date.ToString("yyyy-MM-dd")}')", (a, i) =>
                {
                    var appointment = a;

                    if (appointment.Interval == null)
                    {
                        appointment.Interval = i;
                    }

                    return appointment;
                });

            var freeIntervals = new List<Interval>(allIntervals);

            foreach (var appointment in appointmentsByDate)
            {
                if (freeIntervals.Any(i => i.Id.Equals(appointment.Interval.Id)))
                {
                    freeIntervals.Remove(freeIntervals.First(i => i.Id.Equals(appointment.Interval.Id)));
                }
            }

            return new PaginatedItems<Interval>()
            {
                PagesCount = 1,
                TotalCount = freeIntervals.Count(),
                Data = freeIntervals
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
