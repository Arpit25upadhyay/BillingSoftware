using AaryanBillingApp.UI.ViewModels.Purchases;
using System.Windows;

namespace AaryanBillingApp.UI.Views.Purchases;

public partial class AddPurchaseWindow : Window
{
    private readonly AddPurchaseViewModel _viewModel;

    public AddPurchaseWindow(AddPurchaseViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        DataContext = _viewModel;

        _viewModel.CloseAction = result =>
        {
            DialogResult = result;
        };

        Loaded += async (_, _) =>
        {
            await _viewModel.LoadMastersAsync();
        };
    }
}