namespace AaryanBillingApp.Models.DTOs;

public class ProductListItemDto
{
    public int VariantId { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; } = "";

    public string CategoryName { get; set; } = "";

    public string BrandName { get; set; } = "";

    public string ColorName { get; set; } = "";

    public string SizeName { get; set; } = "";

    public string UnitName { get; set; } = "";

    public string Barcode { get; set; } = "";

    public decimal PurchasePrice { get; set; }

    public decimal SellingPrice { get; set; }

    public decimal CurrentStock { get; set; }
    public decimal MinStock { get; set; }

    public bool IsActive { get; set; }
}