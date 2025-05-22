using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LastBell.Helpers.Messages;

namespace LastBell.ViewModels.Windows;

public partial class VideoWindowViewModel : ObservableObject, IRecipient<VideoSelectionMessage>
{
    [ObservableProperty] private string _selectedVideo;

    public VideoWindowViewModel(IMessenger messenger)
    {
        messenger.Register(this);
    }

    public void Receive(VideoSelectionMessage message)
    {
        SelectVideo(message.Value);
    }

    private void SelectVideo(string category)
    {
        try
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Videos");
            var videos = Directory.GetFiles(path);
            SelectedVideo = videos.First(path => path.Contains(category)) ?? videos.FirstOrDefault();
        }
        catch (Exception e)
        {
            
        }
    }
}