using System.Windows;
using System.Windows.Controls;


namespace AaryanBillingApp.UI.Views.Purchases;

public partial class PurchaseListView : UserControl
{
    public PurchaseListView()
    {
        InitializeComponent();

        Loaded += PurchaseListView_Loaded;
    }

    private async void PurchaseListView_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        if (DataContext is ViewModels.Purchases.PurchaseListViewModel viewModel)
        {
            await viewModel.LoadPurchasesCommand.ExecuteAsync(null);
        }
    }
}