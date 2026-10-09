using System.Windows;
using AaryanBillingApp.UI.ViewModels.Brands;

namespace AaryanBillingApp.UI.Views.Brands;

public partial class AddBrandWindow : Window
{
    public AddBrandWindow(AddBrandViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;

        viewModel.CloseAction = () =>
        {
            DialogResult = true;
        };
    }
}