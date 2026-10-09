using AaryanBillingApp.Models.Entities;
using AaryanBillingApp.Models.DTOs;


public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<ProductListItemDto>> GetProductListAsync();
    Task<Product?> GetByIdAsync(int id);

    Task<int> AddAsync(Product product);

    Task UpdateAsync(Product product);

    Task DeleteAsync(int id);

    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
    Task<IEnumerable<Product>> SearchAsync(string searchText);

    Task<ProductDetailsDto?> GetProductByIdAsync(int productId);
}