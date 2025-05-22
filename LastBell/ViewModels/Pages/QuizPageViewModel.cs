using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LastBell.Helpers;
using LastBell.Managers;
using LastBell.Models;
using LastBell.ViewModels.Popups;
using MvvmNavigationLib.Services;
using Serilog;

namespace LastBell.ViewModels.Pages;

public partial class QuizPageViewModel(
    JsonManager jsonManager,
    ILogger logger,
    NavigationService<MainPageViewModel> mainPageNavigationService,
    NavigationService<ExitPopupViewModel> exitNavigationService,
    ParameterNavigationService<ResultPageViewModel, ResultModel> resultNavigationService) : ObservableObject
{
    [ObservableProperty] private ObservableCollection<QuizModel> _quizModels;
    [ObservableProperty] private ObservableCollection<ResultModel> _resultModels;

    [ObservableProperty] private bool _switchQuiz;
    [ObservableProperty] private bool _hiddenImage;
    [ObservableProperty] private string _selectedImage = string.Empty;

    private int _currentIndex;
    [ObservableProperty] private bool _isAnimated;

    private readonly PathHelper pathHelper = new();

    [ObservableProperty] private int _currentQuestionNumber;
    [ObservableProperty] private int _totalQuestions;
    [ObservableProperty] private double _progressPercentage;
    [ObservableProperty] private bool _isAnswerSelected;

    private QuizModel _currentQuiz = new();

    [RelayCommand] private void GoMainPage() => exitNavigationService.Navigate();

    [RelayCommand]
    private void Loaded()
    {
        GetContent();
    }

    [RelayCommand]
    private void SelectAnswer(AnswerModel model)
    {
        foreach (var answer in _currentQuiz.Answers)
        {
            answer.IsChecked = (answer == model);
        }
        IsAnswerSelected = true;
    }

    [RelayCommand]
    private async Task NextQuestion()
    {
        IsAnimated = true;
        _currentIndex++;

        if (_currentIndex >= QuizModels.Count)
        {
            _currentIndex = 0;
        }

        await Switch();

        await Task.Delay(500);
        IsAnimated = false;
    }
    [RelayCommand]
    private async Task PreviewQuestion()
    {

        IsAnimated = true;
        _currentIndex--;
        if (_currentIndex < 0)
        {
            _currentIndex = QuizModels.Count - 1;
        }

        await Switch();
        await Task.Delay(500);
        IsAnimated = false;
    }
    private async Task Switch()
    {
        var goNext = await HandleQuizCompletion();
        if (goNext) return;
        _currentQuiz = QuizModels[_currentIndex];
        IsAnswerSelected = _currentQuiz.Answers.Any(a => a.IsChecked);
        UpdateProgress();
    }
    private void UpdateProgress()
    {
        CurrentQuestionNumber = _currentIndex + 1;
        ProgressPercentage = (double)CurrentQuestionNumber / TotalQuestions * 100;
        ProgressPercentage = Math.Max(0, Math.Min(100, ProgressPercentage));
    }
    private async Task<bool> HandleQuizCompletion()
    {
        try
        {
            var allAnswered = QuizModels.All(q => q.Answers.Any(a => a.IsChecked));
            if (!allAnswered)
            {
                return false;
            }

            var categoryCounts = QuizModels
                .SelectMany(q => q.Answers)
                .Where(a => a.IsChecked)
                .GroupBy(a => a.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToList();

            var maxCount = categoryCounts.Max(c => c.Count);
            var topCategories = categoryCounts
                .Where(c => c.Count == maxCount)
                .OrderBy(c => c.Category)
                .ToList();

            var selectedCategory = topCategories.First().Category;

            var result = ResultModels.FirstOrDefault(r => r.Category == selectedCategory);

            resultNavigationService.Navigate(result);
        }
        catch (Exception ex)
        {
            logger.Error($"Ошибка выбора Result: {ex.Message}");
        }

        return true;
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
                quizModel.ImagePath = pathHelper.ResolveImagePath(quizModel.ImagePath, "Resources\\QuizImages", logger);
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