namespace LastBell.Helpers.Messages;

public class VideoStateMessage
{
    public bool ShowLogo { get; }

    public VideoStateMessage(bool showLogo)
    {
        ShowLogo = showLogo;
    }
}