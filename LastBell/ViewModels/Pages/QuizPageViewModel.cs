using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LastBell.Managers;
using LastBell.Models;
using MvvmNavigationLib.Services;
using Serilog;

namespace LastBell.ViewModels.Pages;

public partial class QuizPageViewModel(
    JsonManager jsonManager, 
    ILogger logger,
    NavigationService<MainPageViewModel> mainPageNavigationService) : ObservableObject
{
    [ObservableProperty] private ObservableCollection<QuizModel> _quizModels;

    [ObservableProperty] private QuizModel _quiz1;
    [ObservableProperty] private QuizModel _quiz2;
    [RelayCommand] private void GoMainPage() => mainPageNavigationService.Navigate();

    [RelayCommand]
    private void Loaded()
    {
        GetContent();
    }

    [RelayCommand]
    private void NextQuestion()
    {

    }

    private void GetContent()
    {
        try
        {
            QuizModels = jsonManager.ReadJson<ObservableCollection<QuizModel>>("Content");
            foreach (var quizModel in QuizModels)
            {
                quizModel.Text = quizModel.Text.ToUpper();
                quizModel.ImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, quizModel.ImagePath);
            }
        }
        catch (Exception e)
        {
            logger.Error("Ошибка в получении контента: " + e.Message);
        }
    }
}