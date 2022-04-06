using Hospital.Host.Connection.Interfaces;

namespace Hospital.Host
{
    public class DbConnectionWrapper : IDbConnectionWrapper
    {
        private readonly IDbConnection _connection;

        public DbConnectionWrapper(
            string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public IDbConnection Connection => _connection;
    }
}
