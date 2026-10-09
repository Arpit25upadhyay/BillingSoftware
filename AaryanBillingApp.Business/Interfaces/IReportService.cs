using AaryanBillingApp.Models.DTOs;

namespace AaryanBillingApp.Business.Interfaces;

public interface IReportService
{
    Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(
        DateTime fromDate,
        DateTime toDate);
}