using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LastBell.Helpers.Messages;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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
        SelectVideo("All");
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

        SelectVideo("All");

    }

    private void SelectVideo(string category)
    {
        try
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Videos");
            var videos = Directory.GetFiles(path);
           
            SelectedVideo = videos.First(path => path.Contains(category)) ?? videos.FirstOrDefault();
        }
        catch (Exception e)
        {
            
        }
    }
}