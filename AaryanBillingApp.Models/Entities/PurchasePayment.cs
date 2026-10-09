namespace AaryanBillingApp.Models.Entities;

public class PurchasePayment
{
    public int Id { get; set; }

    public int PurchaseId { get; set; }

    public int PaymentModeId { get; set; }

    public decimal Amount { get; set; }

    public string? TransactionNumber { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? Remarks { get; set; }
}