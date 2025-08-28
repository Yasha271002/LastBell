using CommunityToolkit.Mvvm.Input;
using LastBell.ViewModels.Pages;
using MainComponents.Popups;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;

namespace LastBell.ViewModels.Popups;

public partial class ExitPopupViewModel(
    CloseNavigationService<ModalNavigationStore> closeModalNavigationService,
    NavigationService<MainPageViewModel> mainNavigationService,
    NavigationService<StartPageViewModel> startNavigationService,
    bool InStartPage)
    : BasePopupViewModel(closeModalNavigationService)
{
    [RelayCommand] private void MainNavigation()
    {
        CloseContainerCommand.Execute(false);
        if(InStartPage)  startNavigationService.Navigate();
        else mainNavigationService.Navigate();
    }
    [RelayCommand] private void GoBackNavigation() => CloseContainerCommand.Execute(false);
}