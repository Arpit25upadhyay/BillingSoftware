using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Entities;
using Dapper;

namespace AaryanBillingApp.Data.Repositories;

public class SettingRepository : ISettingRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SettingRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Setting>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        if (connection == null)
            throw new InvalidOperationException(
                "Database connection could not be created.");

        await connection.OpenAsync();

        const string sql = """
            SELECT
                Id,
                SettingKey,
                SettingValue,
                Description
            FROM Settings
            ORDER BY Id;
            """;

        return await connection.QueryAsync<Setting>(sql);
    }

    public async Task<Setting?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        if (connection == null)
            throw new InvalidOperationException(
                "Database connection could not be created.");

        await connection.OpenAsync();

        const string sql = """
            SELECT
                Id,
                SettingKey,
                SettingValue,
                Description
            FROM Settings
            WHERE Id = @Id;
            """;

        return await connection.QueryFirstOrDefaultAsync<Setting>(
            sql,
            new
            {
                Id = id
            });
    }

    public async Task<Setting?> GetByKeyAsync(string settingKey)
    {
        if (string.IsNullOrWhiteSpace(settingKey))
            return null;

        using var connection = _connectionFactory.CreateConnection();

        if (connection == null)
            throw new InvalidOperationException(
                "Database connection could not be created.");

        await connection.OpenAsync();

        const string sql = """
            SELECT
                Id,
                SettingKey,
                SettingValue,
                Description
            FROM Settings
            WHERE SettingKey = @SettingKey;
            """;

        return await connection.QueryFirstOrDefaultAsync<Setting>(
            sql,
            new
            {
                SettingKey = settingKey
            });
    }

    public async Task<bool> UpdateAsync(
        int id,
        string settingValue)
    {
        using var connection = _connectionFactory.CreateConnection();

        if (connection == null)
            throw new InvalidOperationException(
                "Database connection could not be created.");

        await connection.OpenAsync();

        const string sql = """
            UPDATE Settings
            SET SettingValue = @SettingValue
            WHERE Id = @Id;
            """;

        var affectedRows = await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                SettingValue = settingValue ?? string.Empty
            });

        return affectedRows > 0;
    }
}