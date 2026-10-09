using System.Windows;
using System.Windows.Controls;
using AaryanBillingApp.UI.ViewModels.Settings;

namespace AaryanBillingApp.UI.Views.Settings;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private async void UserControl_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel viewModel)
        {
            await viewModel.LoadSettingsCommand.ExecuteAsync(null);
        }
    }
}