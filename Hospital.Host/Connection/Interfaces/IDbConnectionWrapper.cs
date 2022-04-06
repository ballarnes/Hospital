namespace Hospital.Host.Connection.Interfaces
{
    public interface IDbConnectionWrapper
    {
        IDbConnection Connection { get; }
    }
}
