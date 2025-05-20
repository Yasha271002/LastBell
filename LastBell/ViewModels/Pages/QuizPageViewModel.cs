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
    [ObservableProperty] private bool _hiddenImage;
    [ObservableProperty] private string _selectedImage = string.Empty;
    [ObservableProperty] private string _navigationDirection = "Forward";

    private int _currentIndex;
    private string[] _images;


    [RelayCommand]
    private void GoMainPage() => mainPageNavigationService.Navigate();

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
        NavigationDirection = "Forward";
        await Switch();
    }

    [RelayCommand]
    private async Task PreviewQuestion()
    {
        _currentIndex--;
        if (_currentIndex < 0)
        {
            _currentIndex = QuizModels.Count - 1;
        }
        NavigationDirection = "Backward";
        await Switch();
    }

    private async Task Switch()
    {
        if (_currentIndex >= QuizModels.Count)
        {
            _currentIndex = 0;
        }

        HiddenImage = true;

        SwitchQuiz = !SwitchQuiz;

        if (SwitchQuiz)
        {
            Quiz1 = QuizModels[_currentIndex];
            await Task.Delay(1000); // Anim
            SelectedImage = ResolveImagePath(Quiz1?.ImagePath);
            Quiz2 = null;
        }
        else
        {
            Quiz2 = QuizModels[_currentIndex];
            await Task.Delay(1000); // Anim
            SelectedImage = ResolveImagePath(Quiz2?.ImagePath);
            Quiz1 = null;
        }

        HiddenImage = false;
    }

    private string ResolveImagePath(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            logger.Warning("Путь до изображения пустой или null");
            return string.Empty;
        }

        var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\QuizImages");
        var fullPath = Path.Combine(basePath, relativePath);

        if (File.Exists(fullPath))
        {
            return fullPath;
        }

        logger.Warning($"Изображение не найдено: {fullPath}");
        return string.Empty;
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