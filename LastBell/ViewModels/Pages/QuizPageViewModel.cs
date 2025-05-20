using System.Collections.ObjectModel;
using System.IO;
using System.Reflection.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LastBell.Helpers;
using LastBell.Managers;
using LastBell.Models;
using MvvmNavigationLib.Services;
using Serilog;

namespace LastBell.ViewModels.Pages;

public partial class QuizPageViewModel(
    JsonManager jsonManager,
    ILogger logger,
    NavigationService<MainPageViewModel> mainPageNavigationService,
    ParameterNavigationService<ResultPageViewModel, ResultModel> resultNavigationService) : ObservableObject
{
    [ObservableProperty] private ObservableCollection<QuizModel> _quizModels;
    [ObservableProperty] private ObservableCollection<ResultModel> _resultModels;

    [ObservableProperty] private QuizModel _quiz1;
    [ObservableProperty] private QuizModel _quiz2;

    [ObservableProperty] private bool _switchQuiz;
    [ObservableProperty] private bool _hiddenImage;
    [ObservableProperty] private string _selectedImage = string.Empty;

    private int _currentIndex;
    private string[] _images;
    private bool _isAnimated;

    private readonly PathHelper pathHelper = new();

    [ObservableProperty] private int _currentQuestionNumber;
    [ObservableProperty] private int _totalQuestions;
    [ObservableProperty] private double _progressPercentage;


    [RelayCommand] private void GoMainPage() => mainPageNavigationService.Navigate();

    [RelayCommand]
    private void Loaded()
    {
        GetContent();
    }

    [RelayCommand]
    private async Task NextQuestion()
    {
        if (_isAnimated)
            return;
        _isAnimated = true;
        _currentIndex++;
        await Switch();
        _isAnimated = false;
    }

    [RelayCommand]
    private async Task PreviewQuestion()
    {
        if (_isAnimated)
            return;
        _isAnimated = true;
        _currentIndex--;
        if (_currentIndex < 0)
        {
            _currentIndex = QuizModels.Count - 1;
        }

        await Switch();
        _isAnimated = false;
    }

    private async Task Switch()
    {
        if (_currentIndex >= QuizModels.Count)
        {
            _currentIndex = 0;
        }

        UpdateProgress();
        HiddenImage = true;

        SwitchQuiz = !SwitchQuiz;

        if (SwitchQuiz)
        {
            Quiz1 = QuizModels[_currentIndex];
            await Task.Delay(1000); // Anim
            SelectedImage = pathHelper.ResolveImagePath(Quiz1?.ImagePath, "Resources\\QuizImages", logger);
            Quiz2 = null;
        }
        else
        {
            Quiz2 = QuizModels[_currentIndex];
            await Task.Delay(1000); // Anim
            SelectedImage = pathHelper.ResolveImagePath(Quiz2?.ImagePath, "Resources\\QuizImages", logger);
            Quiz1 = null;
        }

        HiddenImage = false;
    }

    private void UpdateProgress()
    {
        CurrentQuestionNumber = _currentIndex + 1;
        ProgressPercentage = (double)CurrentQuestionNumber / TotalQuestions * 100;
        ProgressPercentage = Math.Max(0, Math.Min(100, ProgressPercentage));
    }

    private void ResultNavigation()
    {
    }

    private async void GetContent()
    {
        try
        {
            QuizModels = jsonManager.ReadJson<ObservableCollection<QuizModel>>("Content");
            ResultModels = jsonManager.ReadJson<ObservableCollection<ResultModel>>("ResultContent");

            foreach (var quizModel in QuizModels)
            {
                quizModel.Text = quizModel.Text.ToUpper();
            }

            TotalQuestions = QuizModels.Count;
            UpdateProgress();

            await Switch();
        }
        catch (Exception e)
        {
            logger.Error("Ошибка в получении контента: " + e.Message);
        }
    }
}