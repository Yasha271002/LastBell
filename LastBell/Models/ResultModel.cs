using CommunityToolkit.Mvvm.ComponentModel;

namespace LastBell.Models;

public partial class ResultModel : ObservableObject
{
    [ObservableProperty] private string _imagePath;
    [ObservableProperty] private string _profession;
    [ObservableProperty] private string _description;
    [ObservableProperty] private string _category;
}