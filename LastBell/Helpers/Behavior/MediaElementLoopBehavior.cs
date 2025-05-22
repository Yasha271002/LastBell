using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;

namespace LastBell.Helpers.Behavior;

public class MediaElementLoopBehavior : Behavior<MediaElement>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.MediaEnded += MediaElement_MediaEnded;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.MediaEnded -= MediaElement_MediaEnded;
    }

    private void MediaElement_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.Position = TimeSpan.Zero; 
            AssociatedObject.Play(); 
        }
    }
}