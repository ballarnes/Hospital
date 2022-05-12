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
using Hospital.DataAccess.Infrastructure;

namespace Hospital.DataAccess.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IDbConnectionWrapper _connection;

        public DoctorRepository(
            IDbConnectionWrapper connection)
        {
            _connection = connection;
        }

        public async Task<PaginatedItems<Doctor>> GetDoctors(int pageIndex, int pageSize)
        {
            var totalCount = await _connection.Connection
                    .QueryAsync<int>("SELECT COUNT(*) FROM Doctors");

            var result = await _connection.Connection
                .QueryAsync<Doctor, Specialization, Doctor>($"SELECT d.*, s.* FROM Doctors d INNER JOIN Specializations s ON d.SpecializationId = s.Id ORDER BY d.Id OFFSET {pageIndex * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY", (d, s) =>
                {
                    var doctorDto = d;

                    if (doctorDto.Specialization == null)
                    {
                        doctorDto.Specialization = s;
                    }

                    return doctorDto;
                });

            return new PaginatedItems<Doctor>()
            {
                PagesCount = (int)Math.Round((Convert.ToDecimal(totalCount.FirstOrDefault()) / pageSize), MidpointRounding.ToPositiveInfinity),
                TotalCount = totalCount.FirstOrDefault(),
                Data = result
            };
        }

        public async Task<Doctor> GetDoctorById(int id)
        {
            var result = await _connection.Connection
                .QueryAsync<Doctor, Specialization, Doctor>($"SELECT d.*, s.* FROM Doctors d INNER JOIN Specializations s ON d.SpecializationId = s.Id WHERE d.Id = {id}", (d, s) =>
                {
                    var doctorDto = d;

                    if (doctorDto.Specialization == null)
                    {
                        doctorDto.Specialization = s;
                    }

                    return doctorDto;
                });

            var doctor = result.FirstOrDefault();

            if (doctor == null)
            {
                return null;
            }

            return new Doctor()
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Surname = doctor.Surname,
                SpecializationId = doctor.SpecializationId,
                Specialization = doctor.Specialization
            };
        }

        public async Task<PaginatedItems<Doctor>> GetDoctorsBySpecializationId(int id)
        {
            var totalCount = await _connection.Connection
                    .QueryAsync<int>($"SELECT COUNT(*) FROM Doctors d WHERE d.SpecializationId = {id}");

            var result = await _connection.Connection
                .QueryAsync<Doctor, Specialization, Doctor>($"SELECT d.*, s.* FROM Doctors d INNER JOIN Specializations s ON d.SpecializationId = s.Id WHERE d.SpecializationId = {id}", (d, s) =>
                {
                    var doctorDto = d;

                    if (doctorDto.Specialization == null)
                    {
                        doctorDto.Specialization = s;
                    }

                    return doctorDto;
                });

            return new PaginatedItems<Doctor>()
            {
                PagesCount = 1,
                TotalCount = totalCount.FirstOrDefault(),
                Data = result
            };
        }

        public async Task<int?> AddDoctor(string name, string surname, int specializationId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@name", name, DbType.String);
            parameters.Add("@surname", surname, DbType.String);
            parameters.Add("@specializationId", specializationId, DbType.Int32);
            parameters.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await _connection.Connection.ExecuteAsync("AddOrUpdateDoctors", parameters, commandType: CommandType.StoredProcedure);

            var result = parameters.Get<int>("@id");

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> UpdateDoctor(Doctor doctor)
        {
            var result = await _connection.Connection.ExecuteAsync(
                "AddOrUpdateDoctors",
                StoredProcedureManager.GetParameters(doctor),
                commandType: CommandType.StoredProcedure);

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> DeleteDoctor(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.Int32);

            var result = await _connection.Connection.ExecuteAsync("DeleteDoctors", parameters, commandType: CommandType.StoredProcedure);

            if (result == default)
            {
                return null;
            }

            return result;
        }
    }
}
