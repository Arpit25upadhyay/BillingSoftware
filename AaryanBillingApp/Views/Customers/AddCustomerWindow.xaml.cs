using AaryanBillingApp.UI.ViewModels.Customers;
using System.Windows;

namespace AaryanBillingApp.UI.Views.Customers;

public partial class AddCustomerWindow : Window
{
    public AddCustomerWindow(AddCustomerViewModel vm)
    {
        InitializeComponent();

        DataContext = vm;

        vm.CloseAction = result =>
        {
            DialogResult = result;
            Close();
        };
    }
}