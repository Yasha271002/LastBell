using Microsoft.Xaml.Behaviors;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;

namespace LastBell.Helpers.Behavior;

public class GridAnimationBehavior : Behavior<FrameworkElement>
{
    public double SlideInFrom { get; set; } = 500;
    public double SlideOutTo { get; set; } = -500;
    public double DurationSeconds { get; set; } = 0.5;

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Loaded += OnLoaded;
        AssociatedObject.Unloaded += OnUnloaded;
        var transform = new TranslateTransform(SlideInFrom, 0);
        AssociatedObject.RenderTransform = transform;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (AssociatedObject.RenderTransform is not TranslateTransform transform) return;
        var slideIn = new DoubleAnimation
        {
            From = SlideInFrom,
            To = 0,
            Duration = TimeSpan.FromSeconds(DurationSeconds),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        transform.BeginAnimation(TranslateTransform.XProperty, slideIn);
    }

    private async void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (AssociatedObject.RenderTransform is not TranslateTransform transform) return;
        var slideOut = new DoubleAnimation
        {
            From = 0,
            To = SlideOutTo,
            Duration = TimeSpan.FromSeconds(DurationSeconds),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
        };

        AssociatedObject.Unloaded -= OnUnloaded;

        var tcs = new TaskCompletionSource<bool>();
        slideOut.Completed += (s, _) => tcs.SetResult(true);

        transform.BeginAnimation(TranslateTransform.XProperty, slideOut);
        await tcs.Task;
    }
}