using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Dtos;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;
using Dapper;
using System.Data;

namespace AaryanBillingApp.Data.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CustomerRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT
            Id,
            CustomerCode,
            Name,
            Mobile,
            Email,
            Gender,
            DateOfBirth,
            Address,
            City,
            State,
            Pincode,
            GSTNumber,
            CreditLimit,
            OpeningBalance,
            LoyaltyPoints,
            IsActive,
            CreatedOn,
            UpdatedOn
        FROM Customers
        WHERE IsDeleted = 0
        ORDER BY Name;
        """;

        return await connection.QueryAsync<Customer>(sql);
    }

    public async Task<IEnumerable<CustomerListItemDto>> GetCustomerListAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT
            Id,
            CustomerCode,
            Name,
            Mobile,
            City,
            OpeningBalance,
            LoyaltyPoints,
            IsActive
        FROM Customers
        WHERE IsDeleted = 0
        ORDER BY Name;
        """;

        return await connection.QueryAsync<CustomerListItemDto>(sql);
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT
            Id,
            CustomerCode,
            Name,
            Mobile,
            Email,
            Gender,
            DateOfBirth,
            Address,
            City,
            State,
            Pincode,
            GSTNumber,
            CreditLimit,
            OpeningBalance,
            LoyaltyPoints,
            IsActive,
            CreatedOn,
            UpdatedOn
        FROM Customers
        WHERE Id = @Id
          AND IsDeleted = 0;
        """;

        return await connection.QuerySingleOrDefaultAsync<Customer>(
            sql,
            new { Id = id });
    }

    public async Task<int> AddAsync(Customer customer)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        INSERT INTO Customers
        (
            CustomerCode,
            Name,
            Mobile,
            Email,
            Gender,
            DateOfBirth,
            Address,
            City,
            State,
            Pincode,
            GSTNumber,
            CreditLimit,
            OpeningBalance,
            LoyaltyPoints,
            IsActive
        )
        VALUES
        (
            @CustomerCode,
            @Name,
            @Mobile,
            @Email,
            @Gender,
            @DateOfBirth,
            @Address,
            @City,
            @State,
            @Pincode,
            @GSTNumber,
            @CreditLimit,
            @OpeningBalance,
            @LoyaltyPoints,
            @IsActive
        );

        SELECT CAST(SCOPE_IDENTITY() AS INT);
        """;

        return await connection.ExecuteScalarAsync<int>(sql, customer);
    }

    public async Task UpdateAsync(Customer customer)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        UPDATE Customers
        SET
            CustomerCode = @CustomerCode,
            Name = @Name,
            Mobile = @Mobile,
            Email = @Email,
            Gender = @Gender,
            DateOfBirth = @DateOfBirth,
            Address = @Address,
            City = @City,
            State = @State,
            Pincode = @Pincode,
            GSTNumber = @GSTNumber,
            CreditLimit = @CreditLimit,
            OpeningBalance = @OpeningBalance,
            LoyaltyPoints = @LoyaltyPoints,
            IsActive = @IsActive,
            UpdatedOn = GETDATE()
        WHERE Id = @Id;
        """;

        await connection.ExecuteAsync(sql, customer);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        UPDATE Customers
        SET
            IsDeleted = 1,
            UpdatedOn = GETDATE()
        WHERE Id = @Id;
        """;

        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<bool> ExistsByCustomerCodeAsync(string customerCode, int? excludeId = null)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT COUNT(*)
        FROM Customers
        WHERE CustomerCode = @CustomerCode
          AND IsDeleted = 0
          AND (@ExcludeId IS NULL OR Id <> @ExcludeId);
        """;

        int count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                CustomerCode = customerCode,
                ExcludeId = excludeId
            });

        return count > 0;
    }

    public async Task<bool> ExistsByMobileAsync(string mobile, int? excludeId = null)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT COUNT(*)
        FROM Customers
        WHERE Mobile = @Mobile
          AND IsDeleted = 0
          AND (@ExcludeId IS NULL OR Id <> @ExcludeId);
        """;

        int count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Mobile = mobile,
                ExcludeId = excludeId
            });

        return count > 0;
    }

    public async Task<bool> ExistsByGSTAsync(string? gstNumber, int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(gstNumber))
            return false;

        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT COUNT(*)
        FROM Customers
        WHERE GSTNumber = @GSTNumber
          AND IsDeleted = 0
          AND (@ExcludeId IS NULL OR Id <> @ExcludeId);
        """;

        int count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                GSTNumber = gstNumber,
                ExcludeId = excludeId
            });

        return count > 0;
    }

    public async Task<IEnumerable<Customer>> SearchAsync(string searchText)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT
            Id,
            CustomerCode,
            Name,
            Mobile,
            Email,
            Gender,
            DateOfBirth,
            Address,
            City,
            State,
            Pincode,
            GSTNumber,
            CreditLimit,
            OpeningBalance,
            LoyaltyPoints,
            IsActive,
            CreatedOn,
            UpdatedOn
        FROM Customers
        WHERE IsDeleted = 0
          AND
          (
                Name LIKE @Search
             OR CustomerCode LIKE @Search
             OR Mobile LIKE @Search
             OR City LIKE @Search
             OR GSTNumber LIKE @Search
          )
        ORDER BY Name;
        """;

        return await connection.QueryAsync<Customer>(
            sql,
            new
            {
                Search = $"%{searchText}%"
            });
    }

    public async Task<CustomerDetailsDto?> GetCustomerByIdAsync(int customerId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT
            Id,
            CustomerCode,
            Name,
            Mobile,
            Email,
            Gender,
            DateOfBirth,
            Address,
            City,
            State,
            Pincode,
            GSTNumber,
            CreditLimit,
            OpeningBalance,
            LoyaltyPoints,
            IsActive,
            CreatedOn,
            UpdatedOn
        FROM Customers
        WHERE Id = @CustomerId
          AND IsDeleted = 0;
        """;

        var customer = await connection.QuerySingleOrDefaultAsync<Customer>(
            sql,
            new { CustomerId = customerId });

        if (customer == null)
            return null;

        return new CustomerDetailsDto
        {
            Customer = customer
        };
    }
}