using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Entities;
using Dapper;

namespace AaryanBillingApp.Data.Repositories;

public class SizeRepository : ISizeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SizeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Size>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                Id,
                Name,
                DisplayOrder,
                IsActive,
                IsDeleted,
                CreatedOn,
                UpdatedOn
            FROM Sizes
            WHERE IsDeleted = 0
            ORDER BY DisplayOrder, Name;
            """;

        return await connection.QueryAsync<Size>(sql);
    }
}