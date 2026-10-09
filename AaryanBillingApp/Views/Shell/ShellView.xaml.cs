using AaryanBillingApp.UI.ViewModels.Shell;
using System.Windows;

namespace AaryanBillingApp.UI.Views.Shell;

public partial class ShellView : Window
{
    public ShellView(ShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}