using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Interfaces;

public interface IProductVariantService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();

    Task<Product?> GetProductByIdAsync(int id);

    Task<int> AddProductAsync(Product product, ProductVariant variant);

    Task UpdateProductAsync(Product product, ProductVariant variant);

    Task DeleteProductAsync(int id);

    Task<IEnumerable<ProductVariant>> GetVariantsAsync(int productId);
}