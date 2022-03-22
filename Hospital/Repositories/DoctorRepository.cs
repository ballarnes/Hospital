using Hospital.Host.Connection.Interfaces;
using Hospital.Host.Data;
using Hospital.Host.Models.Dtos;
using Hospital.Host.Repositories.Interfaces;

namespace Hospital.Host.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IDbConnectionWrapper _connection;

        public DoctorRepository(
            IDbConnectionWrapper connection)
        {
            _connection = connection;
        }

        public async Task<PaginatedItems<DoctorDto>> GetDoctors(int pageIndex, int pageSize)
        {
            var totalCount = await _connection.Connection
                    .QueryAsync<int>("SELECT COUNT(*) FROM Doctors");

            var result = await _connection.Connection
                .QueryAsync<DoctorDto, SpecializationDto, DoctorDto>($"SELECT d.*, s.* FROM Doctors d INNER JOIN Specializations s ON d.SpecializationId = s.Id ORDER BY d.Id OFFSET {pageIndex * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY", (d, s) =>
                {
                    var doctorDto = d;

                    if (doctorDto.Specialization == null)
                    {
                        doctorDto.Specialization = new SpecializationDto();
                    }

                    doctorDto.Specialization = s;
                    return doctorDto;
                });

            return new PaginatedItems<DoctorDto>()
            {
                PagesCount = (int)Math.Round((Convert.ToDecimal(totalCount.FirstOrDefault()) / pageSize), MidpointRounding.ToPositiveInfinity),
                TotalCount = totalCount.FirstOrDefault(),
                Data = result
            };
        }

        public async Task<DoctorDto?> GetDoctorById(int id)
        {
            var result = await _connection.Connection
                .QueryAsync<DoctorDto, SpecializationDto, DoctorDto>($"SELECT d.*, s.* FROM Doctors d INNER JOIN Specializations s ON d.SpecializationId = s.Id WHERE d.Id = {id}", (d, s) =>
                {
                    var doctorDto = d;

                    if (doctorDto.Specialization == null)
                    {
                        doctorDto.Specialization = new SpecializationDto();
                    }

                    doctorDto.Specialization = s;
                    return doctorDto;
                });

            var doctor = result.FirstOrDefault();

            if (doctor == null)
            {
                return null;
            }

            return new DoctorDto()
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Surname = doctor.Surname,
                SpecializationId = doctor.SpecializationId,
                Specialization = doctor.Specialization
            };
        }

        public async Task<int?> AddDoctor(string name, string surname, int specializationId)
        {
            var command = new SqlCommand("AddOrUpdateDoctors");
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = (SqlConnection)_connection.Connection;

            var nameParam = new SqlParameter
            {
                ParameterName = "@name",
                SqlDbType = SqlDbType.NVarChar,
                Value = name
            };

            var surnameParam = new SqlParameter
            {
                ParameterName = "@surname",
                SqlDbType = SqlDbType.NVarChar,
                Value = surname
            };

            var specizalizationIdParam = new SqlParameter
            {
                ParameterName = "@specializationId",
                SqlDbType = SqlDbType.Int,
                Value = specializationId
            };

            var returnParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };

            command.Parameters.Add(nameParam);
            command.Parameters.Add(surnameParam);
            command.Parameters.Add(specizalizationIdParam);
            command.Parameters.Add(returnParam);

            await command.ExecuteNonQueryAsync();

            if (returnParam.Value == null)
            {
                return null;
            }

            return (int)returnParam.Value;
        }

        public async Task<int?> UpdateDoctor(int id, string name, string surname, int specializationId)
        {
            var command = new SqlCommand("AddOrUpdateDoctors");
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

            var surnameParam = new SqlParameter
            {
                ParameterName = "@surname",
                SqlDbType = SqlDbType.NVarChar,
                Value = surname
            };

            var specizalizationIdParam = new SqlParameter
            {
                ParameterName = "@specializationId",
                SqlDbType = SqlDbType.Int,
                Value = specializationId
            };

            command.Parameters.Add(idParam);
            command.Parameters.Add(nameParam);
            command.Parameters.Add(surnameParam);
            command.Parameters.Add(specizalizationIdParam);

            var result = await command.ExecuteNonQueryAsync();

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> DeleteDoctor(int id)
        {
            var command = new SqlCommand("DeleteDoctors");
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
