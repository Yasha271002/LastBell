using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows;
using LastBell.ViewModels.Pages;
using Button = System.Windows.Controls.Button;

namespace LastBell.Helpers.Behavior;

public class ScrollViewerButtonsStateBehavior : Behavior<ScrollViewer>
{
    public static readonly DependencyProperty LeftButtonProperty =
        DependencyProperty.Register(nameof(LeftButton), typeof(Button), typeof(ScrollViewerButtonsStateBehavior));

    public Button LeftButton
    {
        get => (Button)GetValue(LeftButtonProperty);
        set => SetValue(LeftButtonProperty, value);
    }

    public static readonly DependencyProperty RightButtonProperty =
        DependencyProperty.Register(nameof(RightButton), typeof(Button), typeof(ScrollViewerButtonsStateBehavior));

    public Button RightButton
    {
        get => (Button)GetValue(RightButtonProperty);
        set => SetValue(RightButtonProperty, value);
    }

    public static readonly DependencyProperty IsAnswerSelectedProperty = DependencyProperty.Register(
        nameof(IsAnswerSelected), typeof(bool), typeof(ScrollViewerButtonsStateBehavior), new PropertyMetadata(default(bool), PropertyChangedCallback));

    private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (ScrollViewerButtonsStateBehavior)d;
        behavior.UpdateButtonStates();
    }

    public bool IsAnswerSelected
    {
        get { return (bool)GetValue(IsAnswerSelectedProperty); }
        set { SetValue(IsAnswerSelectedProperty, value); }
    }

    public static readonly DependencyProperty IsAnimatedProperty = DependencyProperty.Register(
        nameof(IsAnimated), typeof(bool), typeof(ScrollViewerButtonsStateBehavior), new PropertyMetadata(default(bool), PropertyChangedCallback));

    public bool IsAnimated
    {
        get { return (bool)GetValue(IsAnimatedProperty); }
        set { SetValue(IsAnimatedProperty, value); }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.ScrollChanged += OnScrollChanged;
        UpdateButtonStates();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.ScrollChanged -= OnScrollChanged;
    }

    private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        if (AssociatedObject.HorizontalOffset <= 0 || IsAnimated)
        {
            LeftButton.IsEnabled = false;
            LeftButton.Opacity = 0.2;
        }
        else
        {
            LeftButton.IsEnabled = true;
            LeftButton.Opacity = 1;
        }

        if (IsAnswerSelected && !IsAnimated)
        {
            RightButton.IsEnabled = true;
            RightButton.Opacity = 1;
        }
        else
        {
            RightButton.IsEnabled = false;
            RightButton.Opacity = 0.2;
        }
    }
}