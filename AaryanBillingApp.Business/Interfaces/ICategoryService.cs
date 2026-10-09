using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(int id);

    Task<int> AddAsync(Category category);

    Task UpdateAsync(Category category);

    Task DeleteAsync(int id);

    Task<bool> CategoryExistsAsync(string name, int? excludeId = null);
}