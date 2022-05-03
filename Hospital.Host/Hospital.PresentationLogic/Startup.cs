using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Hospital.BusinessLogic.Services;
using Hospital.BusinessLogic.Services.Interfaces;
using Hospital.DataAccess.Data;
using Hospital.DataAccess.Repositories;
using Hospital.DataAccess.Repositories.Interfaces;
using Infrastructure.Connection;
using Infrastructure.Connection.Interfaces;
using Infrastructure.Extensions;
using Infrastructure.Filters;
using Hospital.PresentationLogic.Configurations;
using Hospital.PresentationLogic.Quickstart.UI;
using IdentityServer4;

namespace Hospital.PresentationLogic
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);

            services.AddMvc();

            // cookie policy to deal with temporary browser incompatibilities
            services.AddSameSiteCookiePolicy();

            services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApis())
                .AddInMemoryClients(IdentityServerConfig.GetClients(Configuration))
                .AddTestUsers(TestUsers.Users)
                .AddDeveloperSigningCredential();

            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ForwardSignOut = IdentityServerConstants.DefaultCookieAuthenticationScheme;

                    options.ClientId = Configuration["GoogleAuth:ClientId"];
                    options.ClientSecret = Configuration["GoogleAuth:ClientSecret"];

                    options.UsePkce = true;
                });

            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hospital HTTP API",
                    Version = "v1",
                    Description = "The Hospital Service HTTP API"
                });

                var authority = Configuration["Authorization:Authority"];
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

            services.AddAuthorization(Configuration);
            services.AddAutoMapper(typeof(Program));

            services.AddTransient<IOfficeRepository, OfficeRepository>();
            services.AddTransient<IOfficeService, OfficeService>();

            services.AddTransient<IIntervalRepository, IntervalRepository>();
            services.AddTransient<IIntervalService, IntervalService>();

            services.AddTransient<ISpecializationRepository, SpecializationRepository>();
            services.AddTransient<ISpecializationService, SpecializationService>();

            services.AddTransient<IDoctorRepository, DoctorRepository>();
            services.AddTransient<IDoctorService, DoctorService>();

            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<IAppointmentService, AppointmentService>();

            services.AddScoped<IDbConnectionWrapper, DbConnectionWrapper>(provider => new DbConnectionWrapper(Configuration["ConnectionString"]));

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .WithOrigins("http://localhost:5000", "http://localhost:5001")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger()
                .UseSwaggerUI(setup =>
                {
                    setup.SwaggerEndpoint($"{Configuration["PathBase"]}/swagger/v1/swagger.json", "Hospital.API V1");
                    setup.OAuthClientId("hospitalswaggerui");
                    setup.OAuthAppName("Hospital Swagger UI");
                });
            }

            app.UseCookiePolicy();

            app.UseCors("CorsPolicy");

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });

            DbInitializer.FillTablesIfEmpty(Configuration, app);
        }
    }
}