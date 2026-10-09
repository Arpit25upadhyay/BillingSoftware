using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Data.Interfaces;

public interface IColorRepository
{
    Task<IEnumerable<Color>> GetAllAsync();
    Task<int> AddAsync(Color color);
}