using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AaryanBillingApp.UI.ViewModels.Settings;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly ISettingService _settingService;

    private List<SettingDto> _allSettings = new();

    public SettingsViewModel(ISettingService settingService)
    {
        _settingService = settingService;

        Settings = new ObservableCollection<SettingDto>();
    }


    // ==========================================
    // PROPERTIES
    // ==========================================

    public ObservableCollection<SettingDto> Settings { get; }

    [ObservableProperty]
    private SettingDto? selectedSetting;

    [ObservableProperty]
    private string settingValue = string.Empty;

    [ObservableProperty]
    private bool isBusy;


    // ==========================================
    // LOAD SETTINGS
    // ==========================================

    [RelayCommand]
    private async Task LoadSettings()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var data = await _settingService.GetAllAsync();

            _allSettings = data.ToList();

            Settings.Clear();

            foreach (var setting in _allSettings)
            {
                Settings.Add(setting);
            }

            // Select first setting automatically
            if (Settings.Count > 0)
            {
                SelectedSetting = Settings[0];
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Failed to load settings: {ex}");
        }
        finally
        {
            IsBusy = false;
        }
    }


    // ==========================================
    // SELECTION CHANGED
    // ==========================================

    partial void OnSelectedSettingChanged(SettingDto? value)
    {
        SettingValue = value?.SettingValue ?? string.Empty;
    }


    // ==========================================
    // SAVE
    // ==========================================

    [RelayCommand]
    private async Task SaveSetting()
    {
        if (SelectedSetting == null)
            return;

        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var success =
                await _settingService.UpdateAsync(
                    SelectedSetting.Id,
                    SettingValue);

            if (!success)
                return;

            SelectedSetting.SettingValue = SettingValue;

            var index = Settings.IndexOf(SelectedSetting);

            if (index >= 0)
            {
                Settings[index] = SelectedSetting;
            }

            var existing = _allSettings.FirstOrDefault(
                x => x.Id == SelectedSetting.Id);

            if (existing != null)
            {
                existing.SettingValue = SettingValue;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Failed to save setting: {ex}");
        }
        finally
        {
            IsBusy = false;
        }
    }


    // ==========================================
    // REFRESH
    // ==========================================

    [RelayCommand]
    private async Task Refresh()
    {
        await LoadSettings();
    }
}