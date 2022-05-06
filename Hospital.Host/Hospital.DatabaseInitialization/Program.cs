using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Resources.NetStandard;

namespace Hospital.DatabaseInitialization
{
    internal class Program
    {
        private const int _hrResult = -2146233087;

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

            var logger = loggerFactory.CreateLogger<Program>();

            using (var resxReader = new ResXResourceReader("Commands.resx"))
            {
                using (var connection = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString")))
                {
                    connection.Open();
                    foreach (DictionaryEntry entry in resxReader)
                    {
                        Execute(logger, entry, connection);
                    }
                    connection.Close();
                }
            }
        }

        static void Execute(ILogger logger, DictionaryEntry entry, SqlConnection connection)
        {
            try
            {
                var server = new Server(new ServerConnection(connection));

                server.ConnectionContext.ExecuteNonQuery(entry.Value.ToString());

                logger.LogInformation($"Executed [{entry.Key}] command.");
            }
            catch (ExecutionFailureException ex)
            {
                if (ex.HResult == _hrResult)
                {
                    logger.LogInformation($"[{entry.Key}]: {ex.InnerException.Message}");
                }
                else
                {
                    logger.LogError(ex.ToString(), $"An error occurred executing [{entry.Key}] command.");
                }
            }
        }
    }
}