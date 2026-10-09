using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.DTOs;
using Dapper;

namespace AaryanBillingApp.Data.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ReportRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(
        DateTime fromDate,
        DateTime toDate)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                s.Id AS SaleId,
                s.InvoiceNo,
                s.CustomerId,
                ISNULL(c.Name, 'Walk In Customer') AS CustomerName,
                s.InvoiceDate,
                s.SubTotal,
                s.DiscountAmount,
                s.TaxAmount,
                s.RoundOff,
                s.GrandTotal,
                s.PaidAmount,
                s.DueAmount,
                s.Status
            FROM Sales s
            LEFT JOIN Customers c
                ON c.Id = s.CustomerId
            WHERE
                s.InvoiceDate >= @FromDate
                AND s.InvoiceDate < DATEADD(DAY, 1, @ToDate)
                AND ISNULL(s.IsCancelled, 0) = 0
            ORDER BY
                s.InvoiceDate DESC,
                s.Id DESC;
            """;

        return await connection.QueryAsync<SalesReportDto>(
            sql,
            new
            {
                FromDate = fromDate.Date,
                ToDate = toDate.Date
            });
    }
}