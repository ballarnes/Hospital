using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Hospital.DataAccess.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.AspNetCore.Builder;
using System.Collections;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace Hospital.DatabaseInitialization
{
    public class DbInitializer
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;
        private readonly List<DictionaryEntry> _commands;

        public DbInitializer(
            string connectionString,
            ILogger logger,
            List<DictionaryEntry> commands)
        {
            _connectionString = connectionString;
            _logger = logger;
            _commands = commands;
        }

        private IEnumerable<Office> GetPreconfiguredOffices()
        {
            return new List<Office>()
            {
                new Office()
                {
                    Number = 101 
                },
                new Office()
                {
                    Number = 102
                },
                new Office()
                {
                    Number = 103
                },
                new Office()
                {
                    Number = 104
                },
                new Office()
                {
                    Number = 105
                }
            };
        }

        private IEnumerable<Specialization> GetPreconfiguredSpecializations()
        {
            return new List<Specialization>()
            {
                new Specialization()
                {
                    Name = "Therapist",
                    Description = "Therapist is the most popular medical specialist who performs the initial examination and diagnosis of diseases."
                },
                new Specialization()
                {
                    Name = "Surgeon",
                    Description = "A surgeon is one of the most responsible and complex professions in the world, whose representatives treat diseases, injuries and pathologies through surgical intervention."
                },
                new Specialization()
                {
                    Name = "Ophthalmologist",
                    Description = "Ophthalmologists treat eye diseases and pathologies, perform vision correction, and prescribe glasses and contact lenses."
                },
                new Specialization()
                {
                    Name = "Nutritionist",
                    Description = "A nutritionist is a doctor who specializes in solving health problems and overweight with the help of a properly selected diet."
                }
            };
        }

        private IEnumerable<Doctor> GetPreconfiguredDoctors()
        {
            return new List<Doctor>()
            {
                new Doctor()
                {
                    Name = "Kerry",
                    Surname = "Bridges",
                    SpecializationId = 1
                },
                new Doctor()
                {
                    Name = "Stephany",
                    Surname = "Dawson",
                    SpecializationId = 1
                },
                new Doctor()
                {
                    Name = "Maria",
                    Surname = "Grant",
                    SpecializationId = 2
                },
                new Doctor()
                {
                    Name = "Donald",
                    Surname = "Webb",
                    SpecializationId = 3
                },
                new Doctor()
                {
                    Name = "David",
                    Surname = "Blair",
                    SpecializationId = 4
                }
            };
        }

        public void InitializeDatabase()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var command in _commands)
                {
                    ExecuteCommand(command, connection);
                }

                connection.Close();
            }
        }

        public void FillTablesIfEmpty()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;

                if (connection.Query<int>("SELECT COUNT(*) FROM Offices").FirstOrDefault() == 0)
                {
                    command.CommandText = "AddOrUpdateOffices";

                    var numberParam = new SqlParameter
                    {
                        ParameterName = "@number",
                        SqlDbType = SqlDbType.Int
                    };

                    foreach (var office in GetPreconfiguredOffices())
                    {
                        numberParam.Value = office.Number;

                        command.Parameters.Add(numberParam);

                        AddObject(command, nameof(Office));

                        command.Parameters.Clear();
                    }
                }

                if (connection.Query<int>("SELECT COUNT(*) FROM Specializations").FirstOrDefault() == 0)
                {
                    command.CommandText = "AddOrUpdateSpecializations";

                    var nameParam = new SqlParameter
                    {
                        ParameterName = "@name",
                        SqlDbType = SqlDbType.NVarChar
                    };

                    var descriptionParam = new SqlParameter
                    {
                        ParameterName = "@description",
                        SqlDbType = SqlDbType.NVarChar
                    };

                    foreach (var specialization in GetPreconfiguredSpecializations())
                    {
                        nameParam.Value = specialization.Name;
                        descriptionParam.Value = specialization.Description;

                        command.Parameters.Add(nameParam);
                        command.Parameters.Add(descriptionParam);

                        AddObject(command, nameof(Specialization));

                        command.Parameters.Clear();
                    }
                }

                if (connection.Query<int>("SELECT COUNT(*) FROM Doctors").FirstOrDefault() == 0)
                {
                    command.CommandText = "AddOrUpdateDoctors";

                    var nameParam = new SqlParameter
                    {
                        ParameterName = "@name",
                        SqlDbType = SqlDbType.NVarChar
                    };

                    var surnameParam = new SqlParameter
                    {
                        ParameterName = "@surname",
                        SqlDbType = SqlDbType.NVarChar
                    };

                    var specizalizationIdParam = new SqlParameter
                    {
                        ParameterName = "@specializationId",
                        SqlDbType = SqlDbType.Int
                    };

                    foreach (var doctor in GetPreconfiguredDoctors())
                    {
                        nameParam.Value = doctor.Name;
                        surnameParam.Value = doctor.Surname;
                        specizalizationIdParam.Value = doctor.SpecializationId;

                        command.Parameters.Add(nameParam);
                        command.Parameters.Add(surnameParam);
                        command.Parameters.Add(specizalizationIdParam);

                        AddObject(command, nameof(Doctor));

                        command.Parameters.Clear();
                    }
                }

                connection.Close();
            }
        }

        private void AddObject(SqlCommand command, string objectName)
        {
            try
            {
                var returnParam = new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };

                command.Parameters.Add(returnParam);

                command.ExecuteNonQuery();

                _logger.LogInformation($"Created new {objectName} with ID: [{returnParam.Value}]");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An error occurred updating the table.");
            }
        }

        private void ExecuteCommand(DictionaryEntry entry, SqlConnection connection)
        {
            try
            {
                var server = new Server(new ServerConnection(connection));

                server.ConnectionContext.ExecuteNonQuery(entry.Value.ToString());

                _logger.LogInformation($"Executed [{entry.Key}] command.");
            }
            catch (ExecutionFailureException ex)
            {
                if (ex.HResult == -2146233087)
                {
                    _logger.LogWarning($"[{entry.Key}]: {ex.InnerException.Message}");
                }
                else
                {
                    _logger.LogError(ex.ToString(), $"An error occurred executing [{entry.Key}] command.");
                }
            }
        }
    }
}