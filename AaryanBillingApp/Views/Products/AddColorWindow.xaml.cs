using AaryanBillingApp.UI.ViewModels.Products;
using System.Windows;

namespace AaryanBillingApp.UI.Views.Products;

public partial class AddColorWindow : Window
{
    public AddColorWindow(AddColorViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;

        viewModel.CloseAction = () =>
        {
            DialogResult = true;
        };
    }
}