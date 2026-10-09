using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;

    public BrandService(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<IEnumerable<Brand>> GetAllAsync()
    {
        return await _brandRepository.GetAllAsync();
    }

    public async Task<int> AddAsync(Brand brand)
    {
        if (brand == null)
            throw new ArgumentNullException(nameof(brand));

        if (string.IsNullOrWhiteSpace(brand.Name))
            throw new ArgumentException("Brand name is required.");

        brand.Name = brand.Name.Trim();
        brand.Description = brand.Description?.Trim();

        return await _brandRepository.AddAsync(brand);
    }

    public async Task UpdateAsync(Brand brand)
    {
        if (brand == null)
            throw new ArgumentNullException(nameof(brand));

        if (brand.Id <= 0)
            throw new ArgumentException("Invalid brand id.");

        if (string.IsNullOrWhiteSpace(brand.Name))
            throw new ArgumentException("Brand name is required.");

        brand.Name = brand.Name.Trim();
        brand.Description = brand.Description?.Trim();

        await _brandRepository.UpdateAsync(brand);
    }

    public async Task<bool> BrandExistsAsync(
        string name,
        int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        return await _brandRepository.BrandExistsAsync(
            name.Trim(),
            excludeId);
    }
}