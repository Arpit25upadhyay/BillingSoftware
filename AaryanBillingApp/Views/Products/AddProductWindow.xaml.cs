using System.Windows;
using AaryanBillingApp.UI.ViewModels.Products;

namespace AaryanBillingApp.UI.Views.Products;

public partial class AddProductWindow : Window
{
    public AddProductWindow()
    {
        InitializeComponent();

        Loaded += AddProductWindow_Loaded;
    }

    private void AddProductWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is AddProductViewModel vm)
        {
            vm.CloseAction = result =>
            {
                DialogResult = result;
                Close();
            };
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }
}