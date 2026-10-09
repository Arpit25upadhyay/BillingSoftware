using AaryanBillingApp.UI.ViewModels.Products;
using System.Windows.Controls;

namespace AaryanBillingApp.UI.Views.Products;

public partial class ProductListView : UserControl
{
    public ProductListView()
    {
        InitializeComponent();

        Loaded += async (_, _) =>
        {
            if (DataContext is ProductListViewModel vm)
            {
                await vm.LoadProducts();
            }
        };
    }
}