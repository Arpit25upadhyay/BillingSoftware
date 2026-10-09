using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Interfaces;

public interface IColorService
{
    Task<IEnumerable<Color>> GetAllAsync();

    Task<int> AddAsync(Color color);

    Task<bool> ExistsAsync(string name);
}