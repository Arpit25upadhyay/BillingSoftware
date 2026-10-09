using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Data.Interfaces;

public interface ISizeRepository
{
    Task<IEnumerable<Size>> GetAllAsync();
}