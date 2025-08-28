using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LastBell.Helpers.Messages;

namespace LastBell.ViewModels.Windows;

public partial class VideoWindowViewModel : ObservableObject, IRecipient<VideoSelectionMessage>, IRecipient<VideoStateMessage>
{
    [ObservableProperty] private string _selectedVideo;
    [ObservableProperty] private bool _showLogo;

    public VideoWindowViewModel(IMessenger messenger)
    {
        //messenger.Register(this);

        messenger.Register<VideoWindowViewModel, VideoSelectionMessage>(this, (recipient, message) =>
        {
            recipient.Receive(message);
        });

        messenger.Register<VideoWindowViewModel, VideoStateMessage>(this, (recipient, message) =>
        {
            recipient.Receive(message);
        });

        ShowLogo = true;
    }

    public void Receive(VideoSelectionMessage message)
    {
        //SelectVideo(message.Value);

        if (!ShowLogo)
        {
            SelectVideo(message.Value);
        }
    }

    public void Receive(VideoStateMessage message)
    {
        ShowLogo = message.ShowLogo;

        if (ShowLogo)
        {
            SelectedVideo = null;
        }
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