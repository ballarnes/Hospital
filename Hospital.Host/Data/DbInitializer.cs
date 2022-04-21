using Hospital.Host.Data.Entities;

namespace Hospital.Host.Data
{
    public class DbInitializer
    {
        private static IEnumerable<Office> GetPreconfiguredOffices()
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

        private static IEnumerable<Interval> GetPreconfiguredIntervals()
        {
            return new List<Interval>()
            {
                new Interval()
                {
                    Start = new TimeSpan(9, 0, 0),
                    End = new TimeSpan(9, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(9, 30, 0),
                    End = new TimeSpan(10, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(10, 0, 0),
                    End = new TimeSpan(10, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(10, 30, 0),
                    End = new TimeSpan(11, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(11, 0, 0),
                    End = new TimeSpan(11, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(11, 30, 0),
                    End = new TimeSpan(12, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(12, 0, 0),
                    End = new TimeSpan(12, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(12, 30, 0),
                    End = new TimeSpan(13, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(14, 0, 0),
                    End = new TimeSpan(14, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(14, 30, 0),
                    End = new TimeSpan(15, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(15, 30, 0),
                    End = new TimeSpan(16, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(16, 0, 0),
                    End = new TimeSpan(16, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(16, 30, 0),
                    End = new TimeSpan(17, 0, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(17, 0, 0),
                    End = new TimeSpan(17, 30, 0)
                },
                new Interval()
                {
                    Start = new TimeSpan(17, 30, 0),
                    End = new TimeSpan(18, 0, 0)
                }
            };
        }

        private static IEnumerable<Specialization> GetPreconfiguredSpecializations()
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

        private static IEnumerable<Doctor> GetPreconfiguredDoctors()
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

        public static async void FillTablesIfEmpty(IConfiguration configuration, IHost host)
        {
            using (var connection = new SqlConnection(configuration["ConnectionString"]))
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

                        await AddObject(command, nameof(Office), host);

                        command.Parameters.Clear();
                    }
                }

                if (connection.Query<int>("SELECT COUNT(*) FROM Intervals").FirstOrDefault() == 0)
                {
                    command.CommandText = "AddOrUpdateIntervals";

                    var startParam = new SqlParameter
                    {
                        ParameterName = "@start",
                        SqlDbType = SqlDbType.Time
                    };

                    var endParam = new SqlParameter
                    {
                        ParameterName = "@end",
                        SqlDbType = SqlDbType.Time
                    };

                    foreach (var interval in GetPreconfiguredIntervals())
                    {
                        startParam.Value = interval.Start;
                        endParam.Value = interval.End;

                        command.Parameters.Add(startParam);
                        command.Parameters.Add(endParam);

                        await AddObject(command, nameof(Interval), host);

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

                        await AddObject(command, nameof(Specialization), host);

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

                        await AddObject(command, nameof(Doctor), host);

                        command.Parameters.Clear();
                    }
                }

                connection.Close();
            }
        }

        private static async Task AddObject(SqlCommand command, string objectName, IHost host)
        {
            var services = host.Services.CreateScope().ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var returnParam = new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };

                command.Parameters.Add(returnParam);

                await command.ExecuteNonQueryAsync();
                logger.LogInformation($"Created new {objectName} with ID: [{returnParam.Value}]");
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "An error occurred updating the table.");
            }
        }
    }
}
