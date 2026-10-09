using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.Entities;
using AaryanBillingApp.UI.Services;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace AaryanBillingApp.UI.ViewModels.Categories;

public partial class CategoryListViewModel : ViewModelBase
{
    private readonly ICategoryService _categoryService;
    private readonly IDialogService _dialogService;

    private List<Category> _allCategories = new();

    public CategoryListViewModel(
        ICategoryService categoryService,
        IDialogService dialogService)
    {
        _categoryService = categoryService;
        _dialogService = dialogService;

        Categories = new ObservableCollection<Category>();

        RefreshCommand.Execute(null);
    }

    [ObservableProperty]
    private ObservableCollection<Category> categories;

    [ObservableProperty]
    private string searchText = string.Empty;

    partial void OnSearchTextChanged(string value)
    {
        FilterCategories();
    }

    [RelayCommand]
    private async Task Refresh()
    {
        var data = await _categoryService.GetAllAsync();

        _allCategories = data.ToList();

        Categories.Clear();

        foreach (var item in _allCategories)
            Categories.Add(item);
    }

    private void FilterCategories()
    {
        IEnumerable<Category> result = _allCategories;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            string search = SearchText.Trim().ToLower();

            result = result.Where(c =>
                (!string.IsNullOrWhiteSpace(c.Name) &&
                 c.Name.ToLower().Contains(search))

                ||

                (!string.IsNullOrWhiteSpace(c.Description) &&
                 c.Description.ToLower().Contains(search)));
        }

        Categories.Clear();

        foreach (var item in result)
            Categories.Add(item);
    }

    [RelayCommand]
    private async Task Delete(Category? category)
    {
        if (category == null)
            return;

        var result = MessageBox.Show(
            $"Delete '{category.Name}' ?",
            "Confirm",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        await _categoryService.DeleteAsync(category.Id);

        await Refresh();
    }

    [RelayCommand]
    private async Task AddCategory()
    {
        var result = _dialogService.ShowAddCategoryDialog();

        if (result != null)
            await Refresh();
    }

    [RelayCommand]
    private async Task Edit(Category? category)
    {
        if (category == null)
            return;

        var result = _dialogService.ShowEditCategoryDialog(category);

        if (result == true)
            await Refresh();
    }

    [RelayCommand]
    private void Export()
    {
        MessageBox.Show("Excel Export will be added later.");
    }
}