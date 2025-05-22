using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;

namespace LastBell.Helpers.Behavior;

public enum ScrollDirection
{
    Left,
    Right
}

public class ScrollViewerBehavior : Behavior<Button>
{
    public static readonly DependencyProperty ScrollViewerProperty =
        DependencyProperty.Register(nameof(ScrollViewer), typeof(ScrollViewer), typeof(ScrollViewerBehavior));

    public ScrollViewer ScrollViewer
    {
        get => (ScrollViewer)GetValue(ScrollViewerProperty);
        set => SetValue(ScrollViewerProperty, value);
    }

    public static readonly DependencyProperty DirectionProperty =
        DependencyProperty.Register(nameof(Direction), typeof(ScrollDirection), typeof(ScrollViewerBehavior));

    public ScrollDirection Direction
    {
        get => (ScrollDirection)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    public static readonly DependencyProperty AnimatedOffsetProperty =
        DependencyProperty.Register(nameof(AnimatedOffset), typeof(double), typeof(ScrollViewerBehavior),
            new PropertyMetadata(0.0, OnAnimatedOffsetChanged));

    public double AnimatedOffset
    {
        get => (double)GetValue(AnimatedOffsetProperty);
        set => SetValue(AnimatedOffsetProperty, value);
    }

    private static void OnAnimatedOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = d as ScrollViewerBehavior;
        if (behavior?.ScrollViewer != null)
        {
            behavior.ScrollViewer.ScrollToHorizontalOffset((double)e.NewValue);
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Click += OnButtonClick;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Click -= OnButtonClick;
    }

    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        if (ScrollViewer == null)
        {
            MessageBox.Show("ScrollViewer не установлен.");
            return;
        }

        var currentOffset = ScrollViewer.HorizontalOffset;
        double targetOffset;

        var scrollAmount = ScrollViewer.ViewportWidth;

        switch (Direction)
        {
            case ScrollDirection.Left:
                targetOffset = Math.Max(currentOffset - scrollAmount, 0);
                break;

            case ScrollDirection.Right:
                var maxOffset = ScrollViewer.ExtentWidth - ScrollViewer.ViewportWidth;
                targetOffset = Math.Min(currentOffset + scrollAmount, maxOffset);
                break;

            default:
                targetOffset = currentOffset;
                break;
        }

        var animation = new DoubleAnimation
        {
            From = currentOffset,
            To = targetOffset,
            Duration = TimeSpan.FromSeconds(0.5),
            EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut }
        };

        this.BeginAnimation(AnimatedOffsetProperty, animation);
    }
}