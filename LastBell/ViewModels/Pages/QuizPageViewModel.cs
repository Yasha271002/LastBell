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

    [ObservableProperty] private bool _switchQuiz;
    [ObservableProperty] private string _selectedImage = string.Empty;

    private int _currentIndex;
    
    

    [RelayCommand] private void GoMainPage() => mainPageNavigationService.Navigate();

    [RelayCommand]
    private void Loaded()
    {
        GetContent();
    }

    [RelayCommand]
    private void SelectAnswer(string category)
    {
        switch (category)
        {
            
        }
    }

    [RelayCommand]
    private async Task NextQuestion()
    {
        _currentIndex++;

        await Switch();
    }

    [RelayCommand]
    private async Task PreviewQuestion()
    {
    }

    private async Task Switch()
    {
        if (_currentIndex >= QuizModels.Count)
        {
            _currentIndex = 0;
        }

        SwitchQuiz = !SwitchQuiz;

        if (SwitchQuiz)
        {
            Quiz1 = QuizModels[_currentIndex];
            SelectedImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "QuizImages", Quiz1.ImagePath.TrimStart('\\'));
            await Task.Delay(1000); // Anim
            Quiz2 = null;
        }
        else
        {
            Quiz2 = QuizModels[_currentIndex];
            SelectedImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "QuizImages", Quiz2.ImagePath.TrimStart('\\'));
            await Task.Delay(1000); // Anim
            Quiz1 = null;
        }
    }

    private async void GetContent()
    {
        try
        {
            QuizModels = jsonManager.ReadJson<ObservableCollection<QuizModel>>("Content");
            foreach (var quizModel in QuizModels)
            {
                quizModel.Text = quizModel.Text.ToUpper();
            }

            await Switch();
        }
        catch (Exception e)
        {
            logger.Error("Ошибка в получении контента: " + e.Message);
        }
    }
}