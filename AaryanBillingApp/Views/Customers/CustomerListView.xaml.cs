using AaryanBillingApp.UI.ViewModels.Customers;
using System.Windows.Controls;

namespace AaryanBillingApp.UI.Views.Customers;

public partial class CustomerListView : UserControl
{
    public CustomerListView()
    {
        InitializeComponent();

        Loaded += async (_, _) =>
        {
            if (DataContext is CustomerListViewModel vm)
            {
                await vm.LoadCustomers();
            }
        };
    }
}