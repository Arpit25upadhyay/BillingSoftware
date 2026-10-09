using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Dtos;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;
using Dapper;

namespace AaryanBillingApp.Data.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public PurchaseRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<PurchaseListItemDto>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                p.Id AS PurchaseId,
                p.PurchaseNo,
                ISNULL(s.CompanyName, '') AS SupplierName,
                p.InvoiceNumber,
                p.PurchaseDate,
                p.SubTotal,
                p.DiscountAmount,
                p.TaxAmount,
                p.GrandTotal,
                p.PaidAmount,
                p.DueAmount,
                p.Status
            FROM Purchases p
            INNER JOIN Suppliers s
                ON s.Id = p.SupplierId
            WHERE p.IsDeleted = 0
            ORDER BY p.Id DESC;
            """;

        return await connection.QueryAsync<PurchaseListItemDto>(sql);
    }

    public async Task<PurchaseDetailsDto?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string purchaseSql = """
            SELECT
                Id,
                PurchaseNo,
                SupplierId,
                UserId,
                InvoiceNumber,
                InvoiceDate,
                PurchaseDate,
                SubTotal,
                DiscountAmount,
                TaxAmount,
                RoundOff,
                GrandTotal,
                PaidAmount,
                DueAmount,
                Status,
                Remarks,
                CreatedOn,
                UpdatedOn
            FROM Purchases
            WHERE Id = @Id
              AND IsDeleted = 0;
            """;

        const string itemSql = """
            SELECT
                Id,
                PurchaseId,
                ProductVariantId,
                Quantity,
                PurchasePrice,
                SellingPrice,
                DiscountAmount,
                GSTPercentage,
                GSTAmount,
                LineTotal,
                CreatedOn
            FROM PurchaseItems
            WHERE PurchaseId = @Id
            ORDER BY Id;
            """;

        const string paymentSql = """
            SELECT
                Id,
                PurchaseId,
                PaymentModeId,
                Amount,
                TransactionNumber,
                PaymentDate,
                Remarks
            FROM PurchasePayments
            WHERE PurchaseId = @Id
            ORDER BY Id;
            """;

        var purchase = await connection.QuerySingleOrDefaultAsync<Purchase>(
            purchaseSql,
            new { Id = id });

        if (purchase == null)
            return null;

        var items = await connection.QueryAsync<PurchaseItem>(
            itemSql,
            new { Id = id });

        var payments = await connection.QueryAsync<PurchasePayment>(
            paymentSql,
            new { Id = id });

        return new PurchaseDetailsDto
        {
            Id = purchase.Id,
            PurchaseNo = purchase.PurchaseNo,
            SupplierId = purchase.SupplierId,
            UserId = purchase.UserId,
            InvoiceNumber = purchase.InvoiceNumber,
            InvoiceDate = purchase.InvoiceDate,
            PurchaseDate = purchase.PurchaseDate,

            SubTotal = purchase.SubTotal,
            DiscountAmount = purchase.DiscountAmount,
            TaxAmount = purchase.TaxAmount,
            RoundOff = purchase.RoundOff,
            GrandTotal = purchase.GrandTotal,
            PaidAmount = purchase.PaidAmount,
            DueAmount = purchase.DueAmount,

            Status = purchase.Status,
            Remarks = purchase.Remarks,

            Items = items.Select(x => new PurchaseItemDto
            {
                Id = x.Id,
                PurchaseId = x.PurchaseId,
                ProductVariantId = x.ProductVariantId,
                Quantity = x.Quantity,
                PurchasePrice = x.PurchasePrice,
                SellingPrice = x.SellingPrice,
                DiscountAmount = x.DiscountAmount,
                GSTPercentage = x.GSTPercentage,
                GSTAmount = x.GSTAmount,
                LineTotal = x.LineTotal
            }).ToList(),

            Payments = payments.Select(x => new PurchasePaymentDto
            {
                Id = x.Id,
                PurchaseId = x.PurchaseId,
                PaymentModeId = x.PaymentModeId,
                Amount = x.Amount,
                TransactionNumber = x.TransactionNumber,
                PaymentDate = x.PaymentDate,
                Remarks = x.Remarks
            }).ToList()
        };
    }

    public async Task<int> AddAsync(
        Purchase purchase,
        IEnumerable<PurchaseItem> items,
        IEnumerable<PurchasePayment> payments)
    {
        using var connection = _connectionFactory.CreateConnection();

        await connection.OpenAsync();

        using var transaction = connection.BeginTransaction();

        try
        {
            const string purchaseSql = """
                INSERT INTO Purchases
                (
                    PurchaseNo,
                    SupplierId,
                    UserId,
                    InvoiceNumber,
                    InvoiceDate,
                    PurchaseDate,
                    SubTotal,
                    DiscountAmount,
                    TaxAmount,
                    RoundOff,
                    GrandTotal,
                    PaidAmount,
                    DueAmount,
                    Status,
                    Remarks
                )
                VALUES
                (
                    @PurchaseNo,
                    @SupplierId,
                    @UserId,
                    @InvoiceNumber,
                    @InvoiceDate,
                    @PurchaseDate,
                    @SubTotal,
                    @DiscountAmount,
                    @TaxAmount,
                    @RoundOff,
                    @GrandTotal,
                    @PaidAmount,
                    @DueAmount,
                    @Status,
                    @Remarks
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);
                """;

            var purchaseId =
                await connection.ExecuteScalarAsync<int>(
                    purchaseSql,
                    purchase,
                    transaction);

            const string itemSql = """
                INSERT INTO PurchaseItems
                (
                    PurchaseId,
                    ProductVariantId,
                    Quantity,
                    PurchasePrice,
                    SellingPrice,
                    DiscountAmount,
                    GSTPercentage,
                    GSTAmount,
                    LineTotal
                )
                VALUES
                (
                    @PurchaseId,
                    @ProductVariantId,
                    @Quantity,
                    @PurchasePrice,
                    @SellingPrice,
                    @DiscountAmount,
                    @GSTPercentage,
                    @GSTAmount,
                    @LineTotal
                );
                """;

            foreach (var item in items)
            {
                item.PurchaseId = purchaseId;

                await connection.ExecuteAsync(
                    itemSql,
                    item,
                    transaction);

                const string stockSql = """
                    UPDATE ProductVariants
                    SET
                        CurrentStock = CurrentStock + @Quantity,
                        UpdatedOn = GETDATE()
                    WHERE Id = @ProductVariantId;
                    """;

                await connection.ExecuteAsync(
                    stockSql,
                    new
                    {
                        item.ProductVariantId,
                        item.Quantity
                    },
                    transaction);
            }

            const string paymentSql = """
                INSERT INTO PurchasePayments
                (
                    PurchaseId,
                    PaymentModeId,
                    Amount,
                    TransactionNumber,
                    PaymentDate,
                    Remarks
                )
                VALUES
                (
                    @PurchaseId,
                    @PaymentModeId,
                    @Amount,
                    @TransactionNumber,
                    @PaymentDate,
                    @Remarks
                );
                """;

            foreach (var payment in payments)
            {
                payment.PurchaseId = purchaseId;

                await connection.ExecuteAsync(
                    paymentSql,
                    payment,
                    transaction);
            }

            transaction.Commit();

            return purchaseId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        await connection.OpenAsync();

        using var transaction = connection.BeginTransaction();

        try
        {
            const string itemSql = """
                SELECT
                    ProductVariantId,
                    Quantity
                FROM PurchaseItems
                WHERE PurchaseId = @Id;
                """;

            var items = (
                await connection.QueryAsync<PurchaseItem>(
                    itemSql,
                    new { Id = id },
                    transaction)
            ).ToList();

            foreach (var item in items)
            {
                const string stockSql = """
                    UPDATE ProductVariants
                    SET
                        CurrentStock = CurrentStock - @Quantity,
                        UpdatedOn = GETDATE()
                    WHERE Id = @ProductVariantId;
                    """;

                await connection.ExecuteAsync(
                    stockSql,
                    new
                    {
                        item.ProductVariantId,
                        item.Quantity
                    },
                    transaction);
            }

            const string deletePaymentsSql = """
                DELETE FROM PurchasePayments
                WHERE PurchaseId = @Id;
                """;

            await connection.ExecuteAsync(
                deletePaymentsSql,
                new { Id = id },
                transaction);

            const string deleteItemsSql = """
                DELETE FROM PurchaseItems
                WHERE PurchaseId = @Id;
                """;

            await connection.ExecuteAsync(
                deleteItemsSql,
                new { Id = id },
                transaction);

            const string deletePurchaseSql = """
                UPDATE Purchases
                SET
                    IsDeleted = 1,
                    UpdatedOn = GETDATE()
                WHERE Id = @Id;
                """;

            await connection.ExecuteAsync(
                deletePurchaseSql,
                new { Id = id },
                transaction);

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}