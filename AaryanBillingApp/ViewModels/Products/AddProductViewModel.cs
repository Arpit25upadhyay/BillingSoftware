using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.Entities;
using AaryanBillingApp.UI.Services;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using Size = AaryanBillingApp.Models.Entities.Size;

namespace AaryanBillingApp.UI.ViewModels.Products;

public partial class AddProductViewModel : ViewModelBase
{
    private readonly IProductService _productService;
    private readonly IMasterDataService _masterDataService;
    private readonly IDialogService _dialogService;

    public AddProductViewModel(IProductService productService, IMasterDataService masterDataService, IDialogService dialogService)
    {
        _productService = productService;
        _masterDataService = masterDataService;
        _dialogService = dialogService;

        Product = new Product
        {
            GSTPercentage = 5,
            IsActive = true
        };

        Product.PropertyChanged += Product_PropertyChanged;

        var variant = new ProductVariant
        {
            IsActive = true
        };

        variant.PropertyChanged += Variant_PropertyChanged;

        Variants.Add(variant);
    }

    private void Product_PropertyChanged(
    object? sender,
    System.ComponentModel.PropertyChangedEventArgs e)
    {
        SaveCommand.NotifyCanExecuteChanged();
    }

    private void Variant_PropertyChanged(
        object? sender,
        System.ComponentModel.PropertyChangedEventArgs e)
    {
        SaveCommand.NotifyCanExecuteChanged();
    }


    [ObservableProperty]
    private Product product;

    public ObservableCollection<ProductVariant> Variants { get; } = new();

    [ObservableProperty]
    private bool isEditMode;

    [ObservableProperty]
    private int editingProductId;

    public ObservableCollection<Category> Categories { get; } = new();
    public ObservableCollection<Brand> Brands { get; } = new();
    public ObservableCollection<Units> Units { get; } = new();
    public ObservableCollection<Color> Colors { get; } = new();
    public ObservableCollection<Size> Sizes { get; } = new();
    public Action<bool?>? CloseAction { get; set; }

    //Yahi add karna hai
    public async Task LoadMastersAsync()
    {
        Categories.Clear();
        foreach (var item in await _masterDataService.GetCategoriesAsync())
            Categories.Add(item);

        Brands.Clear();
        foreach (var item in await _masterDataService.GetBrandsAsync())
            Brands.Add(item);

        Units.Clear();
        foreach (var item in await _masterDataService.GetUnitsAsync())
            Units.Add(item);

        Colors.Clear();
        foreach (var item in await _masterDataService.GetColorsAsync())
            Colors.Add(item);

        Sizes.Clear();
        foreach (var item in await _masterDataService.GetSizesAsync())
            Sizes.Add(item);
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Product.Name))
            return;

        foreach (var variant in Variants)
        {
            if (string.IsNullOrWhiteSpace(variant.Barcode))
                return;

            if (string.IsNullOrWhiteSpace(variant.SKU))
                return;
        }

        if (IsEditMode)
        {
            await _productService.UpdateProductAsync(Product, Variants);
        }
        else
        {
            await _productService.AddProductAsync(Product, Variants);
        }

        CloseAction?.Invoke(true);
    }

    [RelayCommand]
    private async Task AddBrand()
    {
        var brandId = _dialogService.ShowAddBrandDialog();

        if (brandId == null)
            return;

        await LoadMastersAsync();

        Product.BrandId = brandId.Value;
    }

    [RelayCommand]
    private async Task AddColor()
    {
        var colorId = _dialogService.ShowAddColorDialog();

        if (colorId <= 0)
            return;

        await LoadMastersAsync();

        var variant = Variants.FirstOrDefault();

        if (variant != null)
        {
            variant.ColorId = colorId;
        }
    }

    partial void OnProductChanged(Product value)
    {
        RefreshSaveState();
    }

    private void RefreshSaveState()
    {
        SaveCommand.NotifyCanExecuteChanged();
    }

    public bool CanSave
    {
        get
        {
            var variant = Variants.FirstOrDefault();

            return Product.CategoryId > 0
                && (Product.BrandId.HasValue && Product.BrandId.Value > 0)
                && Product.UnitId > 0
                && !string.IsNullOrWhiteSpace(Product.Name)
                && variant != null
                && !string.IsNullOrWhiteSpace(variant.Barcode)
                && !string.IsNullOrWhiteSpace(variant.SKU)
                && variant.PurchasePrice > 0
                && variant.SellingPrice > 0;
        }
    }

    public async Task InitializeForEditAsync(int productId)
    {
        IsEditMode = true;
        EditingProductId = productId;

        await LoadMastersAsync();

        var product = await _productService.GetProductByIdAsync(productId);

        if (product == null)
            return;

        Product = new Product
        {
            Id = product.Product.Id,
            Name = product.Product.Name,
            CategoryId = product.Product.CategoryId,
            BrandId = product.Product.BrandId,
            IsActive = product.Product.IsActive
        };

        Product.PropertyChanged += Product_PropertyChanged;

        var firstVariant = product.Variants.FirstOrDefault();

        Variants.Clear();

        if (firstVariant != null)
        {
            var variant = new ProductVariant
            {
                Id = firstVariant.Id,
                ProductId = product.Product.Id,
                Barcode = firstVariant.Barcode,
                ColorId = firstVariant.ColorId,
                SizeId = firstVariant.SizeId,
                PurchasePrice = firstVariant.PurchasePrice,
                SellingPrice = firstVariant.SellingPrice,
                CurrentStock = firstVariant.CurrentStock,
                MinStock = firstVariant.MinStock,
                IsActive = firstVariant.IsActive
            };

            variant.PropertyChanged += Variant_PropertyChanged;

            Variants.Add(variant);
        }
    }

}