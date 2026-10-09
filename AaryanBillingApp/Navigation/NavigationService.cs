using AaryanBillingApp.UI.ViewModels.Common;
using Microsoft.Extensions.DependencyInjection;

namespace AaryanBillingApp.UI.Navigation;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;

    public ViewModelBase? CurrentViewModel { get; private set; }

    public event Action? CurrentViewChanged;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void NavigateTo<T>() where T : ViewModelBase
    {
        var viewModel = _serviceProvider.GetRequiredService<T>();

        CurrentViewModel = viewModel;

        CurrentViewChanged?.Invoke();
    }
}