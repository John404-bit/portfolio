using System.Data;
using Microsoft.Data.SqlClient;

namespace backend.Data;
/*
Laver forbindelse til databasen
*/

public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") // læser vores connectionString
            ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString); //laver en forbindelse til vores SQL server
}
