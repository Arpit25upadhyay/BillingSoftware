using AaryanBillingApp.UI.ViewModels.Suppliers;
using System.Windows;

namespace AaryanBillingApp.UI.Views.Suppliers;

public partial class AddSupplierWindow : Window
{
    public AddSupplierWindow(AddSupplierViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;

        viewModel.CloseAction = result =>
        {
            DialogResult = result;
            Close();
        };
    }
}