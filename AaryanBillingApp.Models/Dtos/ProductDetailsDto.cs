using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Models.DTOs;

public class ProductDetailsDto
{
    public Product Product { get; set; } = new();

    public List<ProductVariant> Variants { get; set; } = new();
}