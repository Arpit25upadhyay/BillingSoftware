namespace AaryanBillingApp.Models.DTOs;

public class SalesReportDto
{
    public int SaleId { get; set; }

    public string InvoiceNo { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public DateTime InvoiceDate { get; set; }

    public decimal SubTotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal RoundOff { get; set; }

    public decimal GrandTotal { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal DueAmount { get; set; }

    public string Status { get; set; } = string.Empty;
}