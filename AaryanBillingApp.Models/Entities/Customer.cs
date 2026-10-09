namespace AaryanBillingApp.Models.Entities;

public class Customer
{
    public int Id { get; set; }

    public string CustomerCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Mobile { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Pincode { get; set; }

    public string? GSTNumber { get; set; }

    public decimal CreditLimit { get; set; }

    public decimal OpeningBalance { get; set; }

    public int LoyaltyPoints { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}