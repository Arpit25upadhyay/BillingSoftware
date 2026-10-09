using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.DTOs;

namespace AaryanBillingApp.Business.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;

    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(
        DateTime fromDate,
        DateTime toDate)
    {
        if (fromDate > toDate)
            throw new ArgumentException(
                "From date cannot be greater than To date.");

        return await _reportRepository.GetSalesReportAsync(
            fromDate.Date,
            toDate.Date);
    }
}