using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Entities;
using Dapper;

namespace AaryanBillingApp.Data.Repositories;

public class ColorRepository : IColorRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ColorRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Color>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                Id,
                Name,
                IsActive,
                IsDeleted,
                CreatedOn,
                UpdatedOn
            FROM Colors
            WHERE IsDeleted = 0
            ORDER BY Name;
            """;

        return await connection.QueryAsync<Color>(sql);
    }

    public async Task<int> AddAsync(Color color)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        INSERT INTO Colors
        (
            Name,
            ColorCode,
            IsActive,
            IsDeleted,
            CreatedOn
        )
        VALUES
        (
            @Name,
            @ColorCode,
            @IsActive,
            0,
            GETDATE()
        );

        SELECT CAST(SCOPE_IDENTITY() AS INT);
        """;

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                color.Name,
                color.ColorCode,
                color.IsActive
            });
    }
}