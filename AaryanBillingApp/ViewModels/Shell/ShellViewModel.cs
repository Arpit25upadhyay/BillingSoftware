using AaryanBillingApp.UI.Navigation;
using AaryanBillingApp.UI.ViewModels.Categories;
using AaryanBillingApp.UI.ViewModels.Common;
using AaryanBillingApp.UI.ViewModels.Customers;
using AaryanBillingApp.UI.ViewModels.Dashboard;
using AaryanBillingApp.UI.ViewModels.Products;
using AaryanBillingApp.UI.ViewModels.Purchases;
using AaryanBillingApp.UI.ViewModels.Reports;
using AaryanBillingApp.UI.ViewModels.Settings;
using AaryanBillingApp.UI.ViewModels.Suppliers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AaryanBillingApp.UI.ViewModels.Shell;

public partial class ShellViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ViewModelBase? currentView;

    public ShellViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        _navigationService.CurrentViewChanged += NavigationService_CurrentViewChanged;

        CurrentView = _navigationService.CurrentViewModel;

        _navigationService.NavigateTo<DashboardViewModel>();
    }

    private void NavigationService_CurrentViewChanged()
    {
        CurrentView = _navigationService.CurrentViewModel;
    }

    [RelayCommand]
    private void Dashboard()
    {
        _navigationService.NavigateTo<DashboardViewModel>();
    }

    [RelayCommand]
    private void Products()
    {
        _navigationService.NavigateTo<ProductListViewModel>();
    }

    [RelayCommand]
    private void Categories()
    {
        _navigationService.NavigateTo<CategoryListViewModel>();
    }

    [RelayCommand]
    private void Customers()
    {
        _navigationService.NavigateTo<CustomerListViewModel>();
    }

    [RelayCommand]
    private void Billing()
    {
        _navigationService.NavigateTo<CustomerListViewModel>();
    }
    [RelayCommand]
    private void Purchase()
    {
        _navigationService.NavigateTo<PurchaseListViewModel>();
    }
    [RelayCommand]
    private void Suppliers()
    {
        _navigationService.NavigateTo<SupplierListViewModel>();
    }
    [RelayCommand]
    private void Reports()
    {
        _navigationService.NavigateTo<ReportsViewModel>();
    }
    [RelayCommand]
    private void Settings()
    {
        _navigationService.NavigateTo<SettingsViewModel>();
    }
}