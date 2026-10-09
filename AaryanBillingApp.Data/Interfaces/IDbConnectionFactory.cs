using Microsoft.Data.SqlClient;

namespace AaryanBillingApp.Data.Interfaces;

public interface IDbConnectionFactory
{
    SqlConnection CreateConnection();
}