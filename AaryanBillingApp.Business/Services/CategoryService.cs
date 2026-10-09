using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<int> AddAsync(Category category)
    {
        return await _repository.AddAsync(category);
    }

    public async Task UpdateAsync(Category category)
    {
        await _repository.UpdateAsync(category);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<bool> CategoryExistsAsync(
        string name,
        int? excludeId = null)
    {
        return await _repository.CategoryExistsAsync(name, excludeId);
    }
}