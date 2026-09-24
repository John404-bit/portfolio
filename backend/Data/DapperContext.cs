using System.Data;
using Microsoft.Data.SqlClient;

namespace backend.Data;

public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
