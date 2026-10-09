using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.Entities;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Xml.Linq;

namespace AaryanBillingApp.UI.ViewModels.Brands;

public partial class AddBrandViewModel : ViewModelBase
{
    private readonly IBrandService _brandService;

    private int _editingBrandId;

    public Action? CloseAction { get; set; }

    public bool IsEditMode => _editingBrandId > 0;

    public string WindowTitle =>
        IsEditMode ? "Edit Brand" : "Add Brand";

    public int CreatedBrandId { get; private set; }

    public AddBrandViewModel(IBrandService brandService)
    {
        _brandService = brandService;

        IsActive = true;
    }

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private bool isActive = true;

    public void LoadBrand(Brand brand)
    {
        _editingBrandId = brand.Id;

        Name = brand.Name;
        Description = brand.Description;
        IsActive = brand.IsActive;

        OnPropertyChanged(nameof(IsEditMode));
        OnPropertyChanged(nameof(WindowTitle));
    }

    public void Clear()
    {
        _editingBrandId = 0;

        Name = string.Empty;
        Description = string.Empty;
        IsActive = true;

        OnPropertyChanged(nameof(IsEditMode));
        OnPropertyChanged(nameof(WindowTitle));
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            MessageBox.Show(
                "Brand name is required.",
                "Validation",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        bool exists = await _brandService.BrandExistsAsync(
            Name.Trim(),
            IsEditMode ? _editingBrandId : null);

        if (exists)
        {
            MessageBox.Show(
                "Brand already exists.",
                "Validation",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        Brand brand = new()
        {
            Id = _editingBrandId,
            Name = Name.Trim(),
            Description = Description.Trim(),
            IsActive = IsActive
        };

        try
        {
            if (IsEditMode)
            {
                await _brandService.UpdateAsync(brand);
            }
            else
            {
                CreatedBrandId = await _brandService.AddAsync(brand);
            }

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

    partial void OnNameChanged(string value)
    {
        Name = value?.Trim() ?? string.Empty;
    }

    partial void OnDescriptionChanged(string value)
    {
        Description = value?.Trim() ?? string.Empty;
    }
}