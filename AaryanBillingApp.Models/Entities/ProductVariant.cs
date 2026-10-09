using CommunityToolkit.Mvvm.ComponentModel;

namespace AaryanBillingApp.Models.Entities;

public partial class ProductVariant : ObservableObject
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private int productId;

    [ObservableProperty]
    private int? colorId;

    [ObservableProperty]
    private int? sizeId;

    [ObservableProperty]
    private string barcode = string.Empty;

    [ObservableProperty]
    private string sKU = string.Empty;

    [ObservableProperty]
    private decimal purchasePrice;

    [ObservableProperty]
    private decimal sellingPrice;

    [ObservableProperty]
    private decimal currentStock;

    [ObservableProperty]
    private decimal minStock;

    [ObservableProperty]
    private bool isActive = true;

    public string ProductName { get; set; } = string.Empty;
    public string? ColorName { get; set; }
    public string? SizeName { get; set; }
    public string? CategoryName { get; set; }
    public string? BrandName { get; set; }

    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public Product? Product { get; set; }

    public string DisplayName
    {
        get
        {
            var color = string.IsNullOrWhiteSpace(ColorName)
                ? string.Empty
                : ColorName;

            var size = string.IsNullOrWhiteSpace(SizeName)
                ? string.Empty
                : SizeName;

            if (string.IsNullOrEmpty(color) &&
                string.IsNullOrEmpty(size))
                return ProductName;

            return $"{ProductName} ({color} / {size})";
        }
    }
}