using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Dtos;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;
using Dapper;

namespace AaryanBillingApp.Data.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SupplierRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                Id,
                SupplierCode,
                CompanyName,
                ContactPerson,
                Mobile,
                AlternateMobile,
                Email,
                GSTNumber,
                PANNumber,
                Address,
                City,
                State,
                Pincode,
                OpeningBalance,
                PaymentDays,
                Remarks,
                IsActive,
                IsDeleted,
                CreatedOn,
                UpdatedOn
            FROM Suppliers
            WHERE IsDeleted = 0
            ORDER BY CompanyName;
            """;

        return await connection.QueryAsync<Supplier>(sql);
    }

    public async Task<IEnumerable<SupplierListItemDto>> GetSupplierListAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                Id,
                SupplierCode,
                CompanyName,
                ContactPerson,
                Mobile,
                City,
                OpeningBalance,
                PaymentDays,
                IsActive
            FROM Suppliers
            WHERE IsDeleted = 0
            ORDER BY CompanyName;
            """;

        return await connection.QueryAsync<SupplierListItemDto>(sql);
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                Id,
                SupplierCode,
                CompanyName,
                ContactPerson,
                Mobile,
                AlternateMobile,
                Email,
                GSTNumber,
                PANNumber,
                Address,
                City,
                State,
                Pincode,
                OpeningBalance,
                PaymentDays,
                Remarks,
                IsActive,
                IsDeleted,
                CreatedOn,
                UpdatedOn
            FROM Suppliers
            WHERE Id = @Id
              AND IsDeleted = 0;
            """;

        return await connection.QuerySingleOrDefaultAsync<Supplier>(
            sql,
            new { Id = id });
    }

    public async Task<int> AddAsync(Supplier supplier)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            INSERT INTO Suppliers
            (
                SupplierCode,
                CompanyName,
                ContactPerson,
                Mobile,
                AlternateMobile,
                Email,
                GSTNumber,
                PANNumber,
                Address,
                City,
                State,
                Pincode,
                OpeningBalance,
                PaymentDays,
                Remarks,
                IsActive
            )
            VALUES
            (
                @SupplierCode,
                @CompanyName,
                @ContactPerson,
                @Mobile,
                @AlternateMobile,
                @Email,
                @GSTNumber,
                @PANNumber,
                @Address,
                @City,
                @State,
                @Pincode,
                @OpeningBalance,
                @PaymentDays,
                @Remarks,
                @IsActive
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        return await connection.ExecuteScalarAsync<int>(sql, supplier);
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            UPDATE Suppliers
            SET
                SupplierCode = @SupplierCode,
                CompanyName = @CompanyName,
                ContactPerson = @ContactPerson,
                Mobile = @Mobile,
                AlternateMobile = @AlternateMobile,
                Email = @Email,
                GSTNumber = @GSTNumber,
                PANNumber = @PANNumber,
                Address = @Address,
                City = @City,
                State = @State,
                Pincode = @Pincode,
                OpeningBalance = @OpeningBalance,
                PaymentDays = @PaymentDays,
                Remarks = @Remarks,
                IsActive = @IsActive,
                UpdatedOn = GETDATE()
            WHERE Id = @Id;
            """;

        await connection.ExecuteAsync(sql, supplier);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            UPDATE Suppliers
            SET
                IsDeleted = 1,
                UpdatedOn = GETDATE()
            WHERE Id = @Id;
            """;

        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<bool> SupplierExistsAsync(
        string companyName,
        int? excludeId = null)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT COUNT(*)
            FROM Suppliers
            WHERE CompanyName = @CompanyName
              AND IsDeleted = 0
              AND (@ExcludeId IS NULL OR Id <> @ExcludeId);
            """;

        int count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                CompanyName = companyName,
                ExcludeId = excludeId
            });

        return count > 0;
    }
}