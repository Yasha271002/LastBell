using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows;
using LastBell.ViewModels.Pages;

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
        var isAnswerSelected = false;
        if (AssociatedObject.DataContext is QuizPageViewModel viewModel)
        {
            isAnswerSelected = viewModel.IsAnswerSelected;
        }

        if (AssociatedObject.HorizontalOffset <= 0)
        {
            LeftButton.IsEnabled = false;
            LeftButton.Opacity = 0.2;
        }
        else
        {
            LeftButton.IsEnabled = true;
            LeftButton.Opacity = 1;
        }

        if (!isAnswerSelected)
        {
            RightButton.IsEnabled = false;
            RightButton.Opacity = 0.2;
        }
        else
        {
            RightButton.IsEnabled = true;
            RightButton.Opacity = 1;
        }
    }
}