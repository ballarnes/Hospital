using Hospital.Host;
using Hospital.Host.Configurations;
using Hospital.Host.Connection.Interfaces;
using Hospital.Host.Data;
using Hospital.Host.Data.Entities;
using Hospital.Host.Repositories;
using Hospital.Host.Repositories.Interfaces;
using Hospital.Host.Services;
using Hospital.Host.Services.Interfaces;
using Infrastructure.Extensions;
using Infrastructure.Filters;
using Microsoft.OpenApi.Models;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddLog4Net("../../../../SolutionItems/log4net.config");

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hospital HTTP API",
        Version = "v1",
        Description = "The Hospital Service HTTP API"
    });

    var authority = configuration["Authorization:Authority"];
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            Implicit = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                TokenUrl = new Uri($"{authority}/connect/token"),
                Scopes = new Dictionary<string, string>()
                {
                    { "hospital.appointment", "Hospital Appointment Service" },
                    { "hospital.doctor", "Hospital Doctor Service" },
                    { "hospital.interval", "Hospital Interval Service" },
                    { "hospital.office", "Hospital Office Service" },
                    { "hospital.specialization", "Hospital Specialization Service" }
                }
            }
        }
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.Services.Configure<HospitalConfig>(configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization(configuration);

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

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Hospital.API V1");
        setup.OAuthClientId("hospitalswaggerui");
        setup.OAuthAppName("Hospital Swagger UI");
    });

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapControllers();
});

DbInitializer.FillTablesIfEmpty(configuration, app);

app.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}