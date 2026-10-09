using System.Windows;
using AaryanBillingApp.UI.ViewModels.Categories;

namespace AaryanBillingApp.UI.Views.Categories;

public partial class AddCategoryWindow : Window
{
    public AddCategoryWindow(AddCategoryViewModel vm)
    {
        InitializeComponent();

        DataContext = vm;

        vm.CloseAction = () =>
        {
            DialogResult = true;
            Close();
        };
    }
}