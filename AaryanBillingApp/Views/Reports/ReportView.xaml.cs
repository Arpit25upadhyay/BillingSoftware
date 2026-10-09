using System.Windows;
using System.Windows.Controls;
using AaryanBillingApp.UI.ViewModels.Reports;

namespace AaryanBillingApp.UI.Views.Reports;

public partial class ReportsView : UserControl
{
    public ReportsView()
    {
        InitializeComponent();

        Loaded += ReportsView_Loaded;
    }

    private async void ReportsView_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        if (DataContext is ReportsViewModel viewModel)
        {
            await viewModel.LoadAsync();
        }
    }
}