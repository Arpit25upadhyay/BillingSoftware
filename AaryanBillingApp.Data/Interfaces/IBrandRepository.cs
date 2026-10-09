using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Data.Interfaces;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> GetAllAsync();

    Task<int> AddAsync(Brand brand);

    Task UpdateAsync(Brand brand);

    Task<bool> BrandExistsAsync(
        string name,
        int? excludeId = null);
}