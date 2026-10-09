using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;
using AaryanBillingApp.Models.DTOs;


namespace AaryanBillingApp.Business.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<IEnumerable<ProductListItemDto>> GetProductListAsync();
    Task<int> AddProductAsync(Product product, IEnumerable<ProductVariant> variants);

    Task UpdateProductAsync(Product product, IEnumerable<ProductVariant> variants);
    Task DeleteProductAsync(int id);
    Task<IEnumerable<ProductVariant>> GetVariantsAsync(int productId);
    
    Task<IEnumerable<Product>> SearchProductsAsync(string searchText);
    Task<ProductDetailsDto?> GetProductByIdAsync(int productId);
}