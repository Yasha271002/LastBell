using System.Collections.ObjectModel;
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
    [ObservableProperty] private bool _isGoingBack;

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
        IsGoingBack = false;
        _currentIndex++;

        if (_currentIndex >= QuizModels.Count)
        {
            _currentIndex = 0;
        }

        await Switch();
        _isAnimated = false;
    }

    [RelayCommand]
    private async Task PreviewQuestion()
    {
        if (_isAnimated)
            return;
        _isAnimated = true;
        IsGoingBack = true;
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
        var goNext = await HandleQuizCompletion();

        if (goNext)
            return;

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
        UpdateProgress();
        HiddenImage = false;
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