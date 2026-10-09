using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Data.Interfaces;

public interface ISettingRepository
{
    Task<IEnumerable<Setting>> GetAllAsync();

    Task<Setting?> GetByIdAsync(int id);

    Task<Setting?> GetByKeyAsync(string settingKey);

    Task<bool> UpdateAsync(
        int id,
        string settingValue);
}