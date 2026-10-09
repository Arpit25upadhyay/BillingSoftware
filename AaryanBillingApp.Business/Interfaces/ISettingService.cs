using AaryanBillingApp.Models.DTOs;

namespace AaryanBillingApp.Business.Interfaces;

public interface ISettingService
{
    Task<IEnumerable<SettingDto>> GetAllAsync();

    Task<SettingDto?> GetByIdAsync(int id);

    Task<SettingDto?> GetByKeyAsync(string settingKey);

    Task<bool> UpdateAsync(
        int id,
        string settingValue);
}