using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.DTOs;

namespace AaryanBillingApp.Business.Services;

public class SettingsService : ISettingService
{
    private readonly ISettingRepository _settingRepository;

    public SettingsService(ISettingRepository settingRepository)
    {
        _settingRepository = settingRepository;
    }

    public async Task<IEnumerable<SettingDto>> GetAllAsync()
    {
        var settings = await _settingRepository.GetAllAsync();

        return settings.Select(x => new SettingDto
        {
            Id = x.Id,
            SettingKey = x.SettingKey,
            SettingValue = x.SettingValue,
            Description = x.Description
        });
    }

    public async Task<SettingDto?> GetByIdAsync(int id)
    {
        var setting = await _settingRepository.GetByIdAsync(id);

        if (setting == null)
            return null;

        return new SettingDto
        {
            Id = setting.Id,
            SettingKey = setting.SettingKey,
            SettingValue = setting.SettingValue,
            Description = setting.Description
        };
    }

    public async Task<SettingDto?> GetByKeyAsync(string settingKey)
    {
        if (string.IsNullOrWhiteSpace(settingKey))
            return null;

        var setting = await _settingRepository.GetByKeyAsync(settingKey);

        if (setting == null)
            return null;

        return new SettingDto
        {
            Id = setting.Id,
            SettingKey = setting.SettingKey,
            SettingValue = setting.SettingValue,
            Description = setting.Description
        };
    }

    public async Task<bool> UpdateAsync(int id, string settingValue)
    {
        return await _settingRepository.UpdateAsync(
            id,
            settingValue ?? string.Empty);
    }
}