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
    }
}
