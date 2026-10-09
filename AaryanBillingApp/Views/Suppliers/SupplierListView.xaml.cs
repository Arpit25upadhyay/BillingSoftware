using AaryanBillingApp.UI.ViewModels.Suppliers;
using System.Windows.Controls;

namespace AaryanBillingApp.UI.Views.Suppliers;

public partial class SupplierListView : UserControl
{
    public SupplierListView()
    {
        InitializeComponent();

        Loaded += async (_, _) =>
        {
            if (DataContext is SupplierListViewModel vm)
                await vm.LoadSuppliers();
        };
    }
}