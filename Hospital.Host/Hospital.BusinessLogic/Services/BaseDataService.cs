using System;
using System.Threading.Tasks;
using Infrastructure.Connection.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hospital.BusinessLogic.Services
{
    public abstract class BaseDataService
    {
        private readonly IDbConnectionWrapper _connection;
        private readonly ILogger<BaseDataService> _logger;

        protected BaseDataService(
            IDbConnectionWrapper connection,
            ILogger<BaseDataService> logger)
        {
            _logger = logger;
            _connection = connection;
        }

        protected async Task<T1> ExecuteSafe<T1>(Func<Task<T1>> action)
        {
            using (_connection.Connection)
            {
                try
                {
                    _connection.Connection.Open();
                    var result = await action();
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed!");
                    return default(T1)!;
                }
                finally
                {
                    _connection.Connection.Close();
                }
            }
        }
    }
}
