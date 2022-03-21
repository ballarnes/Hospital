using Hospital.Host;
using Hospital.Host.Configurations;
using Hospital.Host.Connection.Interfaces;
using Hospital.Host.Data;
using Hospital.Host.Data.Entities;
using Hospital.Host.Repositories;
using Hospital.Host.Repositories.Interfaces;
using Hospital.Host.Services;
using Hospital.Host.Services.Interfaces;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<HospitalConfig>(configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<IOfficeRepository, OfficeRepository>();
builder.Services.AddTransient<IOfficeService, OfficeService>();

builder.Services.AddTransient<IIntervalRepository, IntervalRepository>();
builder.Services.AddTransient<IIntervalService, IntervalService>();

builder.Services.AddTransient<ISpecializationRepository, SpecializationRepository>();
builder.Services.AddTransient<ISpecializationService, SpecializationService>();

builder.Services.AddTransient<IDoctorRepository, DoctorRepository>();
builder.Services.AddTransient<IDoctorService, DoctorService>();

builder.Services.AddTransient<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddTransient<IAppointmentService, AppointmentService>();

builder.Services.AddScoped<IDbConnectionWrapper, DbConnectionWrapper>(provider => new DbConnectionWrapper(configuration["ConnectionString"]));

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

async void FillTablesIfEmpty(IHost host)
{
    using (var connection = new SqlConnection(configuration["ConnectionString"]))
    {
        connection.Open();

        var command = new SqlCommand();

        command.CommandType = CommandType.StoredProcedure;
        command.Connection = connection;

        if (connection.Query<int>("SELECT COUNT(*) FROM Offices").FirstOrDefault() == 0)
        {
            // command = new SqlCommand("EXEC AddOrUpdateOffices @number", connection);
            command.CommandText = "AddOrUpdateOffices";

            var numberParam = new SqlParameter
            {
                ParameterName = "@number",
                SqlDbType = SqlDbType.Int
            };

            foreach (var office in DbInitializer.GetPreconfiguredOffices())
            {
                numberParam.Value = office.Number;

                command.Parameters.Add(numberParam);

                await AddObject(command, nameof(Office), app);

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

            foreach (var interval in DbInitializer.GetPreconfiguredIntervals())
            {
                startParam.Value = interval.Start.ToShortTimeString();
                endParam.Value = interval.End.ToShortTimeString();

                command.Parameters.Add(startParam);
                command.Parameters.Add(endParam);

                await AddObject(command, nameof(Interval), app);

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

            foreach (var specialization in DbInitializer.GetPreconfiguredSpecializations())
            {
                nameParam.Value = specialization.Name;
                descriptionParam.Value = specialization.Description;

                command.Parameters.Add(nameParam);
                command.Parameters.Add(descriptionParam);

                await AddObject(command, nameof(Specialization), app);

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

            foreach (var doctor in DbInitializer.GetPreconfiguredDoctors())
            {
                nameParam.Value = doctor.Name;
                surnameParam.Value = doctor.Surname;
                specizalizationIdParam.Value = doctor.SpecializationId;

                command.Parameters.Add(nameParam);
                command.Parameters.Add(surnameParam);
                command.Parameters.Add(specizalizationIdParam);

                await AddObject(command, nameof(Doctor), app);

                command.Parameters.Clear();
            }
        }

        connection.Close();
    }
}

async Task AddObject(SqlCommand command, string objectName, IHost host)
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