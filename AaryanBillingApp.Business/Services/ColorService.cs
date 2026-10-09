using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Services;

public class ColorService : IColorService
{
    private readonly IColorRepository _colorRepository;

    public ColorService(IColorRepository colorRepository)
    {
        _colorRepository = colorRepository;
    }

    public async Task<IEnumerable<Color>> GetAllAsync()
    {
        return await _colorRepository.GetAllAsync();
    }

    public async Task<int> AddAsync(Color color)
    {
        if (string.IsNullOrWhiteSpace(color.Name))
            throw new ArgumentException("Color name is required.");

        color.Name = color.Name.Trim();

        if (await ExistsAsync(color.Name))
            throw new InvalidOperationException(
                "Color already exists.");

        return await _colorRepository.AddAsync(color);
    }

    public async Task<bool> ExistsAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var colors = await _colorRepository.GetAllAsync();

        return colors.Any(x =>
            x.Name.Equals(
                name.Trim(),
                StringComparison.OrdinalIgnoreCase));
    }
}