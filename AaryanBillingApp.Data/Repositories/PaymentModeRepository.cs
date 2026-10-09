using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Entities;
using Dapper;

namespace AaryanBillingApp.Data.Repositories;

public class PaymentModeRepository : IPaymentModeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public PaymentModeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<PaymentMode>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                Id,
                Name,
                Description,
                IsActive
            FROM PaymentModes
            WHERE IsActive = 1
            ORDER BY Name;
            """;

        return await connection.QueryAsync<PaymentMode>(sql);
    }

    public async Task<PaymentMode?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                Id,
                Name,
                Description,
                IsActive
            FROM PaymentModes
            WHERE Id = @Id;
            """;

        return await connection.QuerySingleOrDefaultAsync<PaymentMode>(
            sql,
            new { Id = id });
    }
}