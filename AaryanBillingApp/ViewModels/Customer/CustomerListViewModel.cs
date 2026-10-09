using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.UI.Services;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AaryanBillingApp.UI.ViewModels.Customers;

public partial class CustomerListViewModel : ViewModelBase
{
    private readonly ICustomerService _customerService;
    private readonly IDialogService _dialogService;

    private List<CustomerListItemDto> _allCustomers = new();

    public CustomerListViewModel(
        ICustomerService customerService,
        IDialogService dialogService)
    {
        _customerService = customerService;
        _dialogService = dialogService;

        Customers = new ObservableCollection<CustomerListItemDto>();
    }

    [ObservableProperty]
    private ObservableCollection<CustomerListItemDto> customers;

    [ObservableProperty]
    private CustomerListItemDto? selectedCustomer;

    [ObservableProperty]
    private string searchText = string.Empty;

    [RelayCommand]
    public async Task LoadCustomers()
    {
        _allCustomers = (await _customerService.GetCustomerListAsync()).ToList();

        ApplyFilter();
    }

    [RelayCommand]
    public async Task Refresh()
    {
        await LoadCustomers();
    }

    [RelayCommand]
    public async Task AddCustomer()
    {
        bool result = _dialogService.ShowAddCustomerDialog();

        if (result)
            await LoadCustomers();
    }

    [RelayCommand]
    public async Task EditCustomer()
    {
        if (SelectedCustomer == null)
            return;

        bool result = _dialogService.ShowEditCustomerDialog(SelectedCustomer);

        if (result)
            await LoadCustomers();
    }

    [RelayCommand]
    public async Task DeleteCustomer()
    {
        if (SelectedCustomer == null)
            return;

        await _customerService.DeleteCustomerAsync(SelectedCustomer.Id);

        await LoadCustomers();
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        Customers.Clear();

        IEnumerable<CustomerListItemDto> filtered = _allCustomers;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(x =>
                x.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || x.CustomerCode.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || x.Mobile.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || (x.City ?? string.Empty).Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        foreach (var customer in filtered)
            Customers.Add(customer);
    }
}