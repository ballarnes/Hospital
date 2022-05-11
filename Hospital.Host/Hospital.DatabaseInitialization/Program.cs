using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Resources.NetStandard;
using System.Collections.Generic;

namespace Hospital.DatabaseInitialization
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug)
                    .AddConsole();
            });

            var logger = loggerFactory.CreateLogger<DbInitializer>();
            var commands = new List<DictionaryEntry>();
            var connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

            using (var resxReader = new ResXResourceReader("Commands.resx"))
            {
                foreach (DictionaryEntry entry in resxReader)
                {
                    commands.Add(entry);
                }
            }

            var dbInitializer = new DbInitializer(connectionString, logger, commands);

            dbInitializer.InitializeDatabase();
            dbInitializer.FillTablesIfEmpty();
        }
    }
}