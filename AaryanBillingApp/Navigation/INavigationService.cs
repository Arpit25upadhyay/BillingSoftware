using AaryanBillingApp.UI.ViewModels.Common;

namespace AaryanBillingApp.UI.Navigation;

public interface INavigationService
{
    ViewModelBase? CurrentViewModel { get; }

    event Action? CurrentViewChanged;

    void NavigateTo<T>() where T : ViewModelBase;
}