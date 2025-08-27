using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmNavigationLib.Services;

namespace LastBell.ViewModels.Pages;

public partial class StartPageViewModel(NavigationService<MainPageViewModel> mainPageNavigationService): ObservableObject
{
    [RelayCommand]
    private void GoMainView() => mainPageNavigationService.Navigate();
}