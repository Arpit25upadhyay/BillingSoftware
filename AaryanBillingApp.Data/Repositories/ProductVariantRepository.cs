using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Entities;
using Dapper;

namespace AaryanBillingApp.Data.Repositories;

public class ProductVariantRepository : IProductVariantRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ProductVariantRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ProductVariant>> GetByProductIdAsync(int productId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT

            pv.Id,

            pv.ProductId,

            p.Name AS ProductName,

            pv.ColorId,
            c.Name AS ColorName,

            pv.SizeId,
            s.Name AS SizeName,

            pv.Barcode,
            pv.SKU,

            pv.PurchasePrice,
            pv.SellingPrice,

            pv.CurrentStock,
            pv.MinStock,

            pv.IsActive,
            pv.CreatedOn,
            pv.UpdatedOn

        FROM ProductVariants pv

        INNER JOIN Products p
            ON pv.ProductId = p.Id

        LEFT JOIN Colors c
            ON pv.ColorId = c.Id

        LEFT JOIN Sizes s
            ON pv.SizeId = s.Id

        WHERE pv.ProductId = @ProductId
            AND pv.IsDeleted = 0

        ORDER BY c.Name, s.DisplayOrder;
        """;

        return await connection.QueryAsync<ProductVariant>(
            sql,
            new { ProductId = productId });
    }

    public async Task<ProductVariant?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT

            pv.Id,

            pv.ProductId,

            p.Name AS ProductName,

            pv.ColorId,
            c.Name AS ColorName,

            pv.SizeId,
            s.Name AS SizeName,

            pv.Barcode,
            pv.SKU,

            pv.PurchasePrice,
            pv.SellingPrice,

            pv.CurrentStock,
            pv.MinStock,

            pv.IsActive,
            pv.CreatedOn,
            pv.UpdatedOn

        FROM ProductVariants pv

        INNER JOIN Products p
            ON pv.ProductId = p.Id

        LEFT JOIN Colors c
            ON pv.ColorId = c.Id

        LEFT JOIN Sizes s
            ON pv.SizeId = s.Id

        WHERE pv.Id = @Id;
        """;

        return await connection.QuerySingleOrDefaultAsync<ProductVariant>(
            sql,
            new { Id = id });
    }

    public async Task<int> AddAsync(ProductVariant variant)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        INSERT INTO ProductVariants
        (
            ProductId,
            ColorId,
            SizeId,
            Barcode,
            SKU,
            PurchasePrice,
            SellingPrice,
            CurrentStock,
            MinStock,
            IsActive
        )
        VALUES
        (
            @ProductId,
            @ColorId,
            @SizeId,
            @Barcode,
            @SKU,
            @PurchasePrice,
            @SellingPrice,
            @CurrentStock,
            @MinStock,
            @IsActive
        );

        SELECT CAST(SCOPE_IDENTITY() AS INT);
        """;

        return await connection.ExecuteScalarAsync<int>(sql, variant);
    }

    public async Task UpdateAsync(ProductVariant variant)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        UPDATE ProductVariants
        SET
            ColorId = @ColorId,
            SizeId = @SizeId,
            Barcode = @Barcode,
            SKU = @SKU,
            PurchasePrice = @PurchasePrice,
            SellingPrice = @SellingPrice,
            CurrentStock = @CurrentStock,
            MinStock = @MinStock,
            IsActive = @IsActive,
            UpdatedOn = GETDATE()
        WHERE Id = @Id;
        """;

        await connection.ExecuteAsync(sql, variant);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        UPDATE ProductVariants
        SET
            IsDeleted = 1,
            UpdatedOn = GETDATE()
        WHERE Id = @Id;
        """;

        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<bool> BarcodeExistsAsync(string barcode, int? excludeId = null)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT COUNT(*)
        FROM ProductVariants
        WHERE Barcode = @Barcode
          AND IsDeleted = 0
          AND (@ExcludeId IS NULL OR Id <> @ExcludeId);
        """;

        int count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Barcode = barcode,
                ExcludeId = excludeId
            });

        return count > 0;
    }

    public async Task<bool> SKUExistsAsync(string sku, int? excludeId = null)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
        SELECT COUNT(*)
        FROM ProductVariants
        WHERE SKU = @SKU
          AND IsDeleted = 0
          AND (@ExcludeId IS NULL OR Id <> @ExcludeId);
        """;

        int count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                SKU = sku,
                ExcludeId = excludeId
            });

        return count > 0;
    }
}