using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;
using Dapper;
using System.Data;


namespace AaryanBillingApp.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ProductRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT

            p.Id,

            p.CategoryId,
            c.Name AS CategoryName,

            p.BrandId,
            b.Name AS BrandName,

            p.UnitId,
            u.Name AS UnitName,

            p.Name,
            p.Description,
            p.HSNCode,
            p.GSTPercentage,

            p.IsActive,
            p.CreatedOn,
            p.UpdatedOn

        FROM Products p

        INNER JOIN Categories c
            ON p.CategoryId = c.Id

        LEFT JOIN Brands b
            ON p.BrandId = b.Id

        INNER JOIN Units u
            ON p.UnitId = u.Id

        WHERE p.IsDeleted = 0

        ORDER BY p.Name;
        """;

        return await connection.QueryAsync<Product>(sql);
    }

    public async Task<IEnumerable<ProductListItemDto>> GetProductListAsync()
    {
        const string sql = @"
    SELECT
        pv.Id AS VariantId,
        p.Id AS ProductId,
        p.Name AS ProductName,

        c.Name AS CategoryName,
        b.Name AS BrandName,
        co.Name AS ColorName,
        s.Name AS SizeName,
        u.Name AS UnitName,

        pv.Barcode,
        pv.PurchasePrice,
        pv.SellingPrice,
        pv.CurrentStock,
        pv.MinStock,
        pv.IsActive

    FROM Products p

    INNER JOIN ProductVariants pv
        ON p.Id = pv.ProductId

    LEFT JOIN Categories c
        ON p.CategoryId = c.Id

    LEFT JOIN Brands b
        ON p.BrandId = b.Id

    LEFT JOIN Colors co
        ON pv.ColorId = co.Id

    LEFT JOIN Sizes s
        ON pv.SizeId = s.Id

    LEFT JOIN Units u
        ON p.UnitId = u.Id

    WHERE p.IsDeleted = 0

    ORDER BY
        p.Name,
        co.Name,
        s.Name;";

        using var connection = _connectionFactory.CreateConnection();

        var result = await connection.QueryAsync<ProductListItemDto>(sql);

        return result.ToList();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT

            p.Id,

            p.CategoryId,
            c.Name AS CategoryName,

            p.BrandId,
            b.Name AS BrandName,

            p.UnitId,
            u.Name AS UnitName,

            p.Name,
            p.Description,
            p.HSNCode,
            p.GSTPercentage,

            p.IsActive,
            p.CreatedOn,
            p.UpdatedOn

        FROM Products p

        INNER JOIN Categories c
            ON p.CategoryId = c.Id

        LEFT JOIN Brands b
            ON p.BrandId = b.Id

        INNER JOIN Units u
            ON p.UnitId = u.Id

        WHERE p.Id = @Id;
        """;

        return await connection.QuerySingleOrDefaultAsync<Product>(
            sql,
            new { Id = id });
    }

    public async Task<int> AddAsync(Product product)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        INSERT INTO Products
        (
            CategoryId,
            BrandId,
            UnitId,
            Name,
            Description,
            HSNCode,
            GSTPercentage,
            IsActive
        )
        VALUES
        (
            @CategoryId,
            @BrandId,
            @UnitId,
            @Name,
            @Description,
            @HSNCode,
            @GSTPercentage,
            @IsActive
        );

        SELECT CAST(SCOPE_IDENTITY() AS INT);
        """;

        return await connection.ExecuteScalarAsync<int>(sql, product);
    }

    public async Task UpdateAsync(Product product)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        UPDATE Products
        SET
            CategoryId = @CategoryId,
            BrandId = @BrandId,
            UnitId = @UnitId,
            Name = @Name,
            Description = @Description,
            HSNCode = @HSNCode,
            GSTPercentage = @GSTPercentage,
            IsActive = @IsActive,
            UpdatedOn = GETDATE()
        WHERE Id = @Id;
        """;

        await connection.ExecuteAsync(sql, product);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        UPDATE Products
        SET
            IsDeleted = 1,
            UpdatedOn = GETDATE()
        WHERE Id = @Id;
        """;

        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT COUNT(*)
        FROM Products
        WHERE Name = @Name
          AND IsDeleted = 0
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

    public async Task<IEnumerable<Product>> SearchAsync(string searchText)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
    SELECT

        p.Id,

        p.CategoryId,
        c.Name CategoryName,

        p.BrandId,
        b.Name BrandName,

        p.UnitId,
        u.Name UnitName,

        p.Name,

        p.Description,

        p.HSNCode,

        p.GSTPercentage,

        p.IsActive,

        p.CreatedOn,

        p.UpdatedOn

    FROM Products p

    INNER JOIN Categories c
        ON p.CategoryId=c.Id

    LEFT JOIN Brands b
        ON p.BrandId=b.Id

    INNER JOIN Units u
        ON p.UnitId=u.Id

    WHERE
        p.IsDeleted=0
        AND
        (
            p.Name LIKE @Search
            OR c.Name LIKE @Search
            OR b.Name LIKE @Search
        )

    ORDER BY p.Name;
    """;

        return await connection.QueryAsync<Product>(
            sql,
            new
            {
                Search = $"%{searchText}%"
            });
    }

    public async Task<ProductDetailsDto?> GetProductByIdAsync(int productId)
    {
        const string productSql = @"
        SELECT
            Id AS ProductId,
            Name,
            CategoryId,
            BrandId,
            IsActive
        FROM Products
        WHERE Id = @ProductId;";

        const string variantSql = @"
        SELECT
            Id AS VariantId,
            Barcode,
            ColorId,
            SizeId,
            UnitId,
            PurchasePrice,
            SellingPrice,
            MRP,
            Stock,
            MinimumStock,
            IsActive
        FROM ProductVariants
        WHERE ProductId = @ProductId
        ORDER BY Id;";

        using var connection = _connectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
            connection.Open();

        using var multi = await connection.QueryMultipleAsync(
            $"{productSql} {variantSql}",
            new { ProductId = productId });

        var product = await multi.ReadSingleOrDefaultAsync<Product>();

        if (product == null)
            return null;

        var variants = (await multi.ReadAsync<ProductVariant>()).ToList();

        return new ProductDetailsDto
        {
            Product = product,
            Variants = variants
        };
    }
}

