using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.UI.Services;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AaryanBillingApp.UI.ViewModels.Purchases;

public partial class PurchaseListViewModel : ViewModelBase
{
    private readonly IPurchaseService _purchaseService;
    private readonly IDialogService _dialogService;

    private List<PurchaseListItemDto> _allPurchases = new();

    public PurchaseListViewModel(IPurchaseService purchaseService, IDialogService dialogService)
    {
        _purchaseService = purchaseService;
        _dialogService = dialogService;

        Purchases = new ObservableCollection<PurchaseListItemDto>();
    }

    [ObservableProperty]
    private ObservableCollection<PurchaseListItemDto> purchases;

    [ObservableProperty]
    private PurchaseListItemDto? selectedPurchase;

    [ObservableProperty]
    private string searchText = string.Empty;

    [RelayCommand]
    public async Task LoadPurchases()
    {
        var data = await _purchaseService.GetPurchaseListAsync();

        _allPurchases = data.ToList();

        ApplyFilter();
    }

    [RelayCommand]
    public async Task AddPurchase()
    {
        bool result = _dialogService.ShowAddPurchaseDialog();

        if (result)
            await LoadPurchases();
    }

    [RelayCommand]
    public async Task DeletePurchase()
    {
        if (SelectedPurchase == null)
            return;

        await _purchaseService.DeletePurchaseAsync(
            SelectedPurchase.PurchaseId);

        SelectedPurchase = null;

        await LoadPurchases();
    }

    [RelayCommand]
    public async Task Refresh()
    {
        await LoadPurchases();
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        Purchases.Clear();

        IEnumerable<PurchaseListItemDto> filtered = _allPurchases;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(x =>
                x.PurchaseNo.Contains(
                    SearchText,
                    StringComparison.OrdinalIgnoreCase)

                || x.SupplierName.Contains(
                    SearchText,
                    StringComparison.OrdinalIgnoreCase)

                || (x.InvoiceNumber ?? string.Empty).Contains(
                    SearchText,
                    StringComparison.OrdinalIgnoreCase));
        }

        foreach (var item in filtered)
            Purchases.Add(item);
    }
}