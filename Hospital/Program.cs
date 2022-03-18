using Dapper;
using Hospital.Host.Configurations;
using Hospital.Host.Data;
using Hospital.Host.Data.Entities;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<HospitalConfig>(configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseRouting();
}

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapControllers();
});

FillTablesIfEmpty(app);

app.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

void FillTablesIfEmpty(IHost host)
{
    using (var connection = new SqlConnection(configuration["ConnectionString"]))
    {
        connection.Open();

        var command = new SqlCommand();
        command.CommandType = CommandType.StoredProcedure;

        if (connection.Query<int>("SELECT COUNT(*) FROM Offices").FirstOrDefault() == 0)
        {
            command = new SqlCommand("AddOrUpdateOffices", connection);

            var numberParam = new SqlParameter
            {
                ParameterName = "@number",
                SqlDbType = SqlDbType.Int
            };

            foreach (var office in DbInitializer.GetPreconfiguredOffices())
            {
                numberParam.Value = office.Number;

                command.Parameters.Add(numberParam);

                AddObject(command, nameof(Office), app);

                command.Parameters.Clear();
            }
        }

        if (connection.Query<int>("SELECT COUNT(*) FROM Intervals").FirstOrDefault() == 0)
        {
            command = new SqlCommand("AddOrUpdateIntervals", connection);

            var startParam = new SqlParameter
            {
                ParameterName = "@start"
            };

            var endParam = new SqlParameter
            {
                ParameterName = "@end"
            };

            foreach (var interval in DbInitializer.GetPreconfiguredIntervals())
            {
                startParam.Value = interval.Start.ToShortTimeString();
                endParam.Value = interval.End.ToShortTimeString();

                command.Parameters.Add(startParam);
                command.Parameters.Add(endParam);

                AddObject(command, nameof(Interval), app);
            }
        }

        if (connection.Query<int>("SELECT COUNT(*) FROM Specializations").FirstOrDefault() == 0)
        {
            command = new SqlCommand("AddOrUpdateSpecializations", connection);

            var nameParam = new SqlParameter
            {
                ParameterName = "@name"
            };

            var descriptionParam = new SqlParameter
            {
                ParameterName = "@description"
            };

            foreach (var specialization in DbInitializer.GetPreconfiguredSpecializations())
            {
                nameParam.Value = specialization.Name;
                descriptionParam.Value = specialization.Description;

                command.Parameters.Add(nameParam);
                command.Parameters.Add(descriptionParam);

                AddObject(command, nameof(Specialization), app);
            }
        }

        if (connection.Query<int>("SELECT COUNT(*) FROM Doctors").FirstOrDefault() == 0)
        {
            command = new SqlCommand("AddOrUpdateDoctors", connection);

            var nameParam = new SqlParameter
            {
                ParameterName = "@name"
            };

            var surnameParam = new SqlParameter
            {
                ParameterName = "@surname"
            };

            var specizalizationIdParam = new SqlParameter
            {
                ParameterName = "@specializationId"
            };

            foreach (var doctor in DbInitializer.GetPreconfiguredDoctors())
            {
                nameParam.Value = doctor.Name;
                surnameParam.Value = doctor.Surname;
                specizalizationIdParam.Value = doctor.SpecializationId;

                command.Parameters.Add(nameParam);
                command.Parameters.Add(surnameParam);
                command.Parameters.Add(specizalizationIdParam);

                AddObject(command, nameof(Doctor), app);
            }
        }

        connection.Close();
    }
}

void AddObject(SqlCommand command, string objectName, IHost host)
{
    var services = host.Services.CreateScope().ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation($"{command.CommandType} {command.Parameters[0].Value} {command.Parameters[0].Value.GetType()}");
        var result = command.ExecuteScalar();
        logger.LogInformation($"Created new {objectName} with ID: [{result}]");
    }
    catch (SqlException ex)
    {
        logger.LogError(ex, "An error occurred updating the table.");
    }
}