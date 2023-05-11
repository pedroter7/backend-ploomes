namespace PloomesBackend.Data.Util
{
    public class ConnectionStringGetter : IConnectionStringGetter
    {
        private readonly string _connectionString;

        public ConnectionStringGetter(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
