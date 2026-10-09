using AaryanBillingApp.Data.Interfaces;
using Microsoft.Data.SqlClient;

namespace AaryanBillingApp.Data.Repositories;

public class DbConnectionFactory : IDbConnectionFactory
{
    private const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=AaryanRetail;Trusted_Connection=True;TrustServerCertificate=True;";

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}