using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LastBell.Helpers.Messages;
using MvvmNavigationLib.Services;

namespace LastBell.ViewModels.Pages;

public partial class MainPageViewModel(NavigationService<QuizPageViewModel> quizPageNavigationService,NavigationService<StartPageViewModel> startPageNavigationService, IMessenger messenger) : ObservableObject
{
    [RelayCommand] private void GoQuizPage() => quizPageNavigationService.Navigate();
    [RelayCommand] private void GoStartPage() => startPageNavigationService.Navigate();

    [RelayCommand]
    private void Loaded()
    {
        messenger.Send(new VideoSelectionMessage("All"));
    }
}