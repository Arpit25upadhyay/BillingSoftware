using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Entities;
using Dapper;

namespace AaryanBillingApp.Data.Repositories;

public class UnitRepository : IUnitRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UnitRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Units>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                Id,
                Name,
                Unit,
                IsActive,
                IsDeleted,
                CreatedOn,
                UpdatedOn
            FROM Units
            WHERE IsDeleted = 0
            ORDER BY Name;
            """;

        return await connection.QueryAsync<Units>(sql);
    }
}