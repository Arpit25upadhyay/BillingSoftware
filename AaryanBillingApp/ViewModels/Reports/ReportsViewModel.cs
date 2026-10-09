using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AaryanBillingApp.UI.ViewModels.Reports;

public partial class ReportsViewModel : ViewModelBase
{
    private readonly IReportService _reportService;

    public ReportsViewModel(IReportService reportService)
    {
        _reportService = reportService;

        SalesReports = new ObservableCollection<SalesReportDto>();

        FromDate = DateTime.Today;
        ToDate = DateTime.Today;
    }

    public ObservableCollection<SalesReportDto> SalesReports { get; }

    [ObservableProperty]
    private DateTime fromDate;

    [ObservableProperty]
    private DateTime toDate;

    [ObservableProperty]
    private decimal totalSales;

    [ObservableProperty]
    private decimal totalDiscount;

    [ObservableProperty]
    private decimal totalTax;

    [ObservableProperty]
    private decimal totalPaid;

    [ObservableProperty]
    private decimal totalDue;

    [ObservableProperty]
    private bool isLoading;

    [RelayCommand]
    private async Task GenerateSalesReport()
    {
        if (FromDate > ToDate)
            throw new InvalidOperationException(
                "From date cannot be greater than To date.");

        try
        {
            IsLoading = true;

            SalesReports.Clear();

            var data = await _reportService.GetSalesReportAsync(
                FromDate,
                ToDate);

            foreach (var item in data)
            {
                SalesReports.Add(item);
            }

            CalculateSummary();
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task Refresh()
    {
        await GenerateSalesReport();
    }

    public async Task LoadAsync()
    {
        await GenerateSalesReport();
    }

    private void CalculateSummary()
    {
        TotalSales = SalesReports.Sum(x => x.GrandTotal);

        TotalDiscount = SalesReports.Sum(x => x.DiscountAmount);

        TotalTax = SalesReports.Sum(x => x.TaxAmount);

        TotalPaid = SalesReports.Sum(x => x.PaidAmount);

        TotalDue = SalesReports.Sum(x => x.DueAmount);
    }
}