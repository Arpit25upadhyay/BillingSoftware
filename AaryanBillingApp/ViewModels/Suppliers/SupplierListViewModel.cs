using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.Dtos;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.UI.Services;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AaryanBillingApp.UI.ViewModels.Suppliers;

public partial class SupplierListViewModel : ViewModelBase
{
    private readonly ISupplierService _supplierService;
    private readonly IDialogService _dialogService;

    private List<SupplierListItemDto> _allSuppliers = new();

    public SupplierListViewModel(
        ISupplierService supplierService,
        IDialogService dialogService)
    {
        _supplierService = supplierService;
        _dialogService = dialogService;

        Suppliers = new ObservableCollection<SupplierListItemDto>();
    }

    [ObservableProperty]
    private ObservableCollection<SupplierListItemDto> suppliers;

    [ObservableProperty]
    private SupplierListItemDto? selectedSupplier;

    [ObservableProperty]
    private string searchText = string.Empty;

    [RelayCommand]
    public async Task LoadSuppliers()
    {
        var data = await _supplierService.GetSupplierListAsync();

        _allSuppliers = data.ToList();

        Suppliers.Clear();

        foreach (var item in data)
            Suppliers.Add(item);
    }

    [RelayCommand]
    public async Task AddSupplier()
    {
        bool result = _dialogService.ShowAddSupplierDialog();

        if (result)
            await LoadSuppliers();
    }

    [RelayCommand]
    public async Task EditSupplier()
    {
        if (SelectedSupplier == null)
            return;

        bool result = _dialogService.ShowEditSupplierDialog(SelectedSupplier.Id);

        if (result)
            await LoadSuppliers();
    }

    [RelayCommand]
    public async Task DeleteSupplier()
    {
        if (SelectedSupplier == null)
            return;

        await _supplierService.DeleteSupplierAsync(SelectedSupplier.Id);

        await LoadSuppliers();
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        Suppliers.Clear();

        IEnumerable<SupplierListItemDto> filtered = _allSuppliers;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(x =>
                x.CompanyName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || x.ContactPerson.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || x.Mobile.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || x.SupplierCode.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        foreach (var item in filtered)
            Suppliers.Add(item);
    }

    [RelayCommand]
    public async Task Refresh()
    {
        await LoadSuppliers();
    }
}