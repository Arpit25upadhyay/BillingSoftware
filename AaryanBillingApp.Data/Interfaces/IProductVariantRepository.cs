using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Data.Interfaces;

public interface IProductVariantRepository
{
    Task<IEnumerable<ProductVariant>> GetByProductIdAsync(int productId);

    Task<ProductVariant?> GetByIdAsync(int id);

    Task<int> AddAsync(ProductVariant variant);

    Task UpdateAsync(ProductVariant variant);

    Task DeleteAsync(int id);

    Task<bool> BarcodeExistsAsync(string barcode, int? excludeId = null);

    Task<bool> SKUExistsAsync(string sku, int? excludeId = null);
}