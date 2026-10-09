using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.UI.Services;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AaryanBillingApp.UI.ViewModels.Products;

public partial class ProductListViewModel : ViewModelBase
{
    private readonly IProductService _productService;
    private readonly IDialogService _dialogService;

    private List<ProductListItemDto> _allProducts = new();

    public ProductListViewModel(IProductService productService, IDialogService dialogService)
    {
        _productService = productService;
        _dialogService = dialogService;

        Products = new ObservableCollection<ProductListItemDto>();
    }

    [ObservableProperty]
    private int totalProducts;

    [ObservableProperty]
    private decimal inventoryValue;

    [ObservableProperty]
    private int lowStock;

    [ObservableProperty]
    private int activeProducts;

    [ObservableProperty]
    private ObservableCollection<ProductListItemDto> products;

    [ObservableProperty]
    private ProductListItemDto? selectedProduct;

    [ObservableProperty]
    private string searchText = string.Empty;

    [RelayCommand]
    public async Task LoadProducts()
    {
        var data = (await _productService.GetProductListAsync()).ToList();

        _allProducts = data;

        Products.Clear();

        foreach (var item in data)
            Products.Add(item);

        // Dashboard statistics
        TotalProducts = data
            .Select(x => x.ProductId)
            .Distinct()
            .Count();

        InventoryValue = data.Sum(x => x.PurchasePrice * x.CurrentStock);

        LowStock = data.Count(x => x.CurrentStock <= x.MinStock);

        ActiveProducts = data.Count(x =>
            x.IsActive);
    }


    [RelayCommand]
    public async Task AddProduct()
    {
        bool result = _dialogService.ShowAddProductDialog();

        if (result)
            await LoadProducts();
    }

    [RelayCommand]
    private async Task EditProduct(ProductListItemDto? product)
    {
        if (product == null)
            return;

        bool result = _dialogService.ShowEditProductDialog(product.ProductId);

        if (result)
            await LoadProducts();
    }

    [RelayCommand]
    private async Task DeleteProduct(ProductListItemDto? product)
    {
        if (product == null)
            return;

        await _productService.DeleteProductAsync(product.ProductId);

        await LoadProducts();
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        Products.Clear();

        IEnumerable<ProductListItemDto> filtered = _allProducts;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(x =>
                x.ProductName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || (x.Barcode ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || x.CategoryName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || x.BrandName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || x.ColorName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || x.SizeName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        foreach (var item in filtered)
            Products.Add(item);
    }

    [RelayCommand]
    public async Task Refresh()
    {
        await LoadProducts();
    }
}