using CommunityToolkit.Mvvm.ComponentModel;

namespace LastBell.ViewModels.Windows;

public partial class VideoWindowViewModel : ObservableObject
{
    [ObservableProperty] private string _selectedVideo;
}