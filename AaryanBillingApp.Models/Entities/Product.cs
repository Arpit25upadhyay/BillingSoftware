using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;


namespace AaryanBillingApp.Models.Entities;

public partial class Product : ObservableObject
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private int categoryId;

    [ObservableProperty]
    private int? brandId;

    [ObservableProperty]
    private int unitId;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string? description;

    [ObservableProperty]
    private string? hSNCode;

    [ObservableProperty]
    private decimal gSTPercentage = 5;

    [ObservableProperty]
    private bool isActive = true;

    public string CategoryName { get; set; } = string.Empty;
    public string? BrandName { get; set; }
    public string UnitName { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    public List<ProductVariant> Variants { get; set; } = new();
}