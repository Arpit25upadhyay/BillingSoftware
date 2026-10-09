using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Interfaces;

public interface IBrandService
{
    Task<IEnumerable<Brand>> GetAllAsync();

    Task<int> AddAsync(Brand brand);

    Task UpdateAsync(Brand brand);

    Task<bool> BrandExistsAsync( string name, int? excludeId = null);
}