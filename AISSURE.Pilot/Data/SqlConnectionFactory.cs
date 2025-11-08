using System.Data;
using System.Data.SqlClient;

namespace AISSURE.Pilot.Data;

/// <summary>
/// Implementazione della factory per connessioni SQL Server
/// </summary>
public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
