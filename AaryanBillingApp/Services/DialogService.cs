using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;
using AaryanBillingApp.UI.ViewModels.Brands;
using AaryanBillingApp.UI.ViewModels.Categories;
using AaryanBillingApp.UI.ViewModels.Customers;
using AaryanBillingApp.UI.ViewModels.Products;
using AaryanBillingApp.UI.ViewModels.Purchases;
using AaryanBillingApp.UI.ViewModels.Suppliers;
using AaryanBillingApp.UI.Views.Brands;
using AaryanBillingApp.UI.Views.Categories;
using AaryanBillingApp.UI.Views.Customers;
using AaryanBillingApp.UI.Views.Products;
using AaryanBillingApp.UI.Views.Purchases;
using AaryanBillingApp.UI.Views.Suppliers;
using Microsoft.Extensions.DependencyInjection;

namespace AaryanBillingApp.UI.Services;

public class DialogService : IDialogService
{
    private readonly IServiceProvider _serviceProvider;

    public DialogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    #region Product

    public bool ShowAddProductDialog()
    {
        var viewModel = _serviceProvider.GetRequiredService<AddProductViewModel>();

        var window = new AddProductWindow
        {
            DataContext = viewModel
        };

        window.Loaded += async (_, _) =>
        {
            await viewModel.LoadMastersAsync();
        };

        return window.ShowDialog() == true;
    }

    public bool ShowEditProductDialog(int productId)
    {
        var viewModel = _serviceProvider.GetRequiredService<AddProductViewModel>();

        viewModel.InitializeForEditAsync(productId).GetAwaiter().GetResult();

        var window = new AddProductWindow
        {
            DataContext = viewModel
        };

        return window.ShowDialog() == true;
    }

    #endregion

    #region Category

    //public bool ShowAddCategoryDialog()
    //{
    //    var window = _serviceProvider.GetRequiredService<AddCategoryWindow>();

    //    if (window.DataContext is AddCategoryViewModel vm)
    //    {
    //        vm.Clear();
    //    }

    //    return window.ShowDialog() == true;
    //}

    public int? ShowAddCategoryDialog()
    {
        var window = _serviceProvider
            .GetRequiredService<AddCategoryWindow>();

        if (window.DataContext is not AddCategoryViewModel vm)
            return null;

        vm.Clear();

        var result = window.ShowDialog();

        if (result == true)
            return vm.CreatedCategoryId;

        return null;
    }


    public bool ShowEditCategoryDialog(Category category)
    {
        var window = _serviceProvider.GetRequiredService<AddCategoryWindow>();

        if (window.DataContext is AddCategoryViewModel vm)
        {
            vm.LoadCategory(category);
        }

        return window.ShowDialog() == true;
    }

    public bool ShowAddCustomerDialog()
    {
        var window = _serviceProvider.GetRequiredService<AddCustomerWindow>();

        return window.ShowDialog() == true;
    }

    public bool ShowEditCustomerDialog(CustomerListItemDto customer)
    {
        var window = _serviceProvider.GetRequiredService<AddCustomerWindow>();

        var vm = _serviceProvider.GetRequiredService<AddCustomerViewModel>();

        vm.InitializeForEditAsync(customer.Id).GetAwaiter().GetResult();

        window.DataContext = vm;

        return window.ShowDialog() == true;
    }

    public bool ShowAddSupplierDialog()
    {
        var viewModel = _serviceProvider.GetRequiredService<AddSupplierViewModel>();

        var window = new AddSupplierWindow(viewModel);

        return window.ShowDialog() == true;
    }

    public bool ShowEditSupplierDialog(int supplierId)
    {
        var viewModel = _serviceProvider.GetRequiredService<AddSupplierViewModel>();

        viewModel.InitializeForEditAsync(supplierId)
                 .GetAwaiter()
                 .GetResult();

        var window = new AddSupplierWindow(viewModel);

        return window.ShowDialog() == true;
    }

    public bool ShowAddPurchaseDialog()
    {
        var viewModel =
            _serviceProvider.GetRequiredService<AddPurchaseViewModel>();

        var window = new AddPurchaseWindow(viewModel);

        return window.ShowDialog() == true;
    }

    public bool ShowEditPurchaseDialog(int purchaseId)
    {
        var viewModel =
            _serviceProvider.GetRequiredService<AddPurchaseViewModel>();

        viewModel.InitializeForEditAsync(purchaseId).GetAwaiter().GetResult();

        var window = new AddPurchaseWindow(viewModel);

        return window.ShowDialog() == true;
    }

    public int? ShowAddBrandDialog()
    {
        var window = _serviceProvider
            .GetRequiredService<AddBrandWindow>();

        if (window.DataContext is not AddBrandViewModel vm)
            return null;

        vm.Clear();

        var result = window.ShowDialog();

        if (result == true)
            return vm.CreatedBrandId;

        return null;
    }

    public int ShowAddColorDialog()
    {
        var window = _serviceProvider
            .GetRequiredService<AddColorWindow>();

        if (window.DataContext is not AddColorViewModel vm)
            return -1;

        vm.Clear();

        var result = window.ShowDialog();

        if (result == true)
            return vm.Color.Id;

        return -1;
    }

    #endregion
}