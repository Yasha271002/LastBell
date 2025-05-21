using CommunityToolkit.Mvvm.Input;
using LastBell.ViewModels.Pages;
using MainComponents.Popups;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;

namespace LastBell.ViewModels.Popups;

public partial class ExitPopupViewModel(
    CloseNavigationService<ModalNavigationStore> closeModalNavigationService,
    NavigationService<MainPageViewModel> mainNavigationService)
    : BasePopupViewModel(closeModalNavigationService)
{
    [RelayCommand] private void MainNavigation()
    {
        CloseContainerCommand.Execute(false);
        mainNavigationService.Navigate();
    }
    [RelayCommand] private void GoBackNavigation() => CloseContainerCommand.Execute(false);
}