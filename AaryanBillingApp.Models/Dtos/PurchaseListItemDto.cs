namespace AaryanBillingApp.Models.DTOs;

public class PurchaseListItemDto
{
    public int PurchaseId { get; set; }

    public string PurchaseNo { get; set; } = string.Empty;

    public string SupplierName { get; set; } = string.Empty;

    public string? InvoiceNumber { get; set; }

    public DateTime PurchaseDate { get; set; }

    public decimal SubTotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal GrandTotal { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal DueAmount { get; set; }

    public string Status { get; set; } = string.Empty;
}