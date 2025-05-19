using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmNavigationLib.Services;

namespace LastBell.ViewModels.Pages;

public partial class MainPageViewModel(NavigationService<QuizPageViewModel> quizPageNavigationService) : ObservableObject
{
    [RelayCommand] private void GoQuizPage() => quizPageNavigationService.Navigate();
}