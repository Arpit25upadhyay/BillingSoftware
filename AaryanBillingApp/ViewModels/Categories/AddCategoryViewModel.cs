using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.Entities;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Xml.Linq;

namespace AaryanBillingApp.UI.ViewModels.Categories;

public partial class AddCategoryViewModel : ViewModelBase
{
    private readonly ICategoryService _categoryService;

    private int _editingCategoryId;

    public Action? CloseAction { get; set; }

    public int CreatedCategoryId { get; private set; }

    public bool IsEditMode => _editingCategoryId > 0;

    public string WindowTitle => IsEditMode ? "Edit Category" : "Add Category";

    public AddCategoryViewModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;

        IsActive = true;
    }

    #region Properties

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private bool isActive = true;

    #endregion

    public void LoadCategory(Category category)
    {
        _editingCategoryId = category.Id;

        Name = category.Name;
        Description = category.Description;
        IsActive = category.IsActive;

        OnPropertyChanged(nameof(IsEditMode));
        OnPropertyChanged(nameof(WindowTitle));
    }

    public void Clear()
    {
        _editingCategoryId = 0;

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
                "Category name is required.",
                "Validation",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        bool exists = await _categoryService.CategoryExistsAsync(
            Name.Trim(),
            IsEditMode ? _editingCategoryId : null);

        if (exists)
        {
            MessageBox.Show(
                "Category already exists.",
                "Validation",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        Category category = new()
        {
            Id = _editingCategoryId,
            Name = Name.Trim(),
            Description = Description.Trim(),
            IsActive = IsActive
        };

        try
        {
            if (IsEditMode)
            {
                await _categoryService.UpdateAsync(category);

                MessageBox.Show(
                    "Category updated successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                var categoryId = await _categoryService.AddAsync(category);

                CreatedCategoryId = categoryId;

                MessageBox.Show(
                    "Category added successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
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
        Name = value ?? string.Empty;
    }

    partial void OnDescriptionChanged(string value)
    {
        Description = value ?? string.Empty;
    }
}