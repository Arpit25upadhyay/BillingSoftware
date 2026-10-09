public class PurchaseItemDto
{
    public int Id { get; set; }

    public int PurchaseId { get; set; }

    public int ProductVariantId { get; set; }

    public decimal Quantity { get; set; }

    public decimal PurchasePrice { get; set; }

    public decimal SellingPrice { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal GSTPercentage { get; set; }

    public decimal GSTAmount { get; set; }

    public decimal LineTotal { get; set; }
}