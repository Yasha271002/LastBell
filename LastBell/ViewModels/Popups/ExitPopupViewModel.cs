using CommunityToolkit.Mvvm.Input;
using LastBell.ViewModels.Pages;
using MainComponents.Popups;
using MvvmNavigationLib.Services;

namespace LastBell.ViewModels.Popups;

public partial class ExitPopupViewModel(INavigationService closeModalNavigationService,
    NavigationService<MainPageViewModel> mainNavigationService)
    : BasePopupViewModel(closeModalNavigationService)
{
    [RelayCommand] private void MainNavigation() => mainNavigationService.Navigate();
    [RelayCommand] private void GoBackNavigation() => CloseContainerCommand.Execute(false);
}