using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LastBell.Models;

public partial class QuizModel : ObservableObject
{
    [ObservableProperty] private string _text;
    [ObservableProperty] private ObservableCollection<AnswerModel> _answers;
    [ObservableProperty] private string _imagePath;
    [ObservableProperty] private AnswerModel _selectedAnswer;
}

public partial class AnswerModel : ObservableObject
{
    [ObservableProperty] private string _text;
    [ObservableProperty] private string _category;
}