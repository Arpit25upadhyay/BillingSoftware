using AaryanBillingApp.Models.DTOs;

namespace AaryanBillingApp.Data.Interfaces;

public interface IReportRepository
{
    Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(
        DateTime fromDate,
        DateTime toDate);
}