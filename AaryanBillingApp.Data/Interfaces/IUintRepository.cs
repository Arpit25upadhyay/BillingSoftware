using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Data.Interfaces;

public interface IUnitRepository
{
    Task<IEnumerable<Units>> GetAllAsync();
}