using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Entities;
using Dapper;

namespace AaryanBillingApp.Data.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CategoryRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                Id,
                Name,
                Description,
                IsActive,
                CreatedOn,
                UpdatedOn
            FROM Categories
            ORDER BY Name;
            """;

        return await connection.QueryAsync<Category>(sql);
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                Id,
                Name,
                Description,
                IsActive,
                CreatedOn,
                UpdatedOn
            FROM Categories
            WHERE Id = @Id;
            """;

        return await connection.QuerySingleOrDefaultAsync<Category>(
            sql,
            new { Id = id });
    }

    public async Task<int> AddAsync(Category category)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            INSERT INTO Categories
            (
                Name,
                Description,
                IsActive
            )
            VALUES
            (
                @Name,
                @Description,
                @IsActive
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        return await connection.ExecuteScalarAsync<int>(sql, category);
    }

    public async Task UpdateAsync(Category category)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            UPDATE Categories
            SET
                Name = @Name,
                Description = @Description,
                IsActive = @IsActive,
                UpdatedOn = GETDATE()
            WHERE Id = @Id;
            """;

        await connection.ExecuteAsync(sql, category);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            DELETE FROM Categories
            WHERE Id = @Id;
            """;

        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<bool> CategoryExistsAsync(
        string name,
        int? excludeId = null)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT COUNT(*)
            FROM Categories
            WHERE Name = @Name
              AND (@ExcludeId IS NULL OR Id <> @ExcludeId);
            """;

        int count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Name = name,
                ExcludeId = excludeId
            });

        return count > 0;
    }
}