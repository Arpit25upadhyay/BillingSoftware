using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Entities;
using Dapper;

namespace AaryanBillingApp.Data.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public BrandRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Brand>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                Id,
                Name,
                Description,
                IsActive,
                IsDeleted,
                CreatedOn,
                UpdatedOn
            FROM Brands
            WHERE IsDeleted = 0
            ORDER BY Name;
            """;

        return await connection.QueryAsync<Brand>(sql);
    }

    public async Task<int> AddAsync(Brand brand)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            INSERT INTO Brands
            (
                Name,
                Description,
                IsActive,
                IsDeleted,
                CreatedOn
            )
            VALUES
            (
                @Name,
                @Description,
                @IsActive,
                0,
                GETDATE()
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        return await connection.ExecuteScalarAsync<int>(
            sql,
            brand);
    }

    public async Task UpdateAsync(Brand brand)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            UPDATE Brands
            SET
                Name = @Name,
                Description = @Description,
                IsActive = @IsActive,
                UpdatedOn = GETDATE()
            WHERE Id = @Id
              AND IsDeleted = 0;
            """;

        await connection.ExecuteAsync(
            sql,
            brand);
    }

    public async Task<bool> BrandExistsAsync(
        string name,
        int? excludeId = null)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT COUNT(1)
            FROM Brands
            WHERE Name = @Name
              AND IsDeleted = 0
              AND (@ExcludeId IS NULL OR Id <> @ExcludeId);
            """;

        var count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Name = name,
                ExcludeId = excludeId
            });

        return count > 0;
    }
}