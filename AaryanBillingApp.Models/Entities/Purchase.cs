namespace AaryanBillingApp.Models.Entities;

public class Purchase
{
    public int Id { get; set; }

    public string PurchaseNo { get; set; } = string.Empty;

    public int SupplierId { get; set; }

    public int UserId { get; set; }

    public string? InvoiceNumber { get; set; }

    public DateTime InvoiceDate { get; set; }

    public DateTime PurchaseDate { get; set; }

    public decimal SubTotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal RoundOff { get; set; }

    public decimal GrandTotal { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal DueAmount { get; set; }

    public string Status { get; set; } = "Pending";

    public string? Remarks { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int isDeleted { get; set; }

    // Navigation / UI collections
    public List<PurchaseItem> Items { get; set; } = new();

    public List<PurchasePayment> Payments { get; set; } = new();
}