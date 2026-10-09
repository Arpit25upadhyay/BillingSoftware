using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.Entities;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace AaryanBillingApp.UI.ViewModels.Products;

public partial class AddColorViewModel : ViewModelBase
{
    private readonly IColorService _colorService;

    public AddColorViewModel(IColorService colorService)
    {
        _colorService = colorService;

        Color = new Color
        {
            IsActive = true
        };
    }

    [ObservableProperty]
    private Color color;

    public Action? CloseAction { get; set; }

    public void Clear()
    {
        Color = new Color
        {
            IsActive = true
        };
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Color.Name))
        {
            MessageBox.Show(
                "Color name is required.",
                "Validation",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        try
        {
            var exists = await _colorService.ExistsAsync(Color.Name);

            if (exists)
            {
                MessageBox.Show(
                    "Color already exists.",
                    "Validation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            Color.Id = await _colorService.AddAsync(Color);

            MessageBox.Show(
                "Color added successfully.",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            CloseAction?.Invoke();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseAction?.Invoke();
    }
}