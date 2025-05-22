using CommunityToolkit.Mvvm.Messaging.Messages;

namespace LastBell.Helpers.Messages;

public class VideoSelectionMessage : ValueChangedMessage<string>
{
    public VideoSelectionMessage(string value) : base(value) { }
}