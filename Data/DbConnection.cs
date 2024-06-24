using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class DbConnection
{
    private readonly string _connectionString;

    public DbConnection(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Server=HBBTNRL0080\\TONY;Database=mercato;User Id=sa;Password=hojlund2004;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    internal async Task OpenAsync()
    {
        throw new NotImplementedException();
    }

    public static implicit operator DbConnection(SqlConnection v)
    {
        throw new NotImplementedException();
    }
}
