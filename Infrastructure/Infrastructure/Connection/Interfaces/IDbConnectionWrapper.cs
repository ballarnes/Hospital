using System.Data;

namespace Infrastructure.Connection.Interfaces
{
    public interface IDbConnectionWrapper
    {
        IDbConnection Connection { get; }
    }
}