namespace AaryanBillingApp.Models.DTOs;

public class CustomerListItemDto
{
    public int Id { get; set; }

    public string CustomerCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Mobile { get; set; } = string.Empty;

    public string? City { get; set; }

    public decimal OpeningBalance { get; set; }

    public int LoyaltyPoints { get; set; }

    public bool IsActive { get; set; }
}