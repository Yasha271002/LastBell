using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LastBell.Views.Controls;

public partial class BackgroundControl : UserControl
{
    public static readonly DependencyProperty BackgroundBrushProperty = 
        DependencyProperty.Register(
            nameof(BackgroundBrush), 
            typeof(Brush), 
            typeof(BackgroundControl),
            new PropertyMetadata(default(Brush)));

    public static readonly DependencyProperty ForegroundBrushProperty =
        DependencyProperty.Register(
            nameof(ForegroundBrush), 
            typeof(Brush), 
            typeof(BackgroundControl),
            new PropertyMetadata(default(Brush)));

    public static readonly DependencyProperty OpacityIconProperty = 
        DependencyProperty.Register(
            nameof(OpacityIcon),
            typeof(double),
            typeof(BackgroundControl)
            , new PropertyMetadata(default(double)));

    public double OpacityIcon
    {
        get { return (double)GetValue(OpacityIconProperty); }
        set { SetValue(OpacityIconProperty, value); }
    }
    public Brush ForegroundBrush
    {
        get { return (Brush)GetValue(ForegroundBrushProperty); }
        set { SetValue(ForegroundBrushProperty, value); }
    }

    public Brush BackgroundBrush
    {
        get { return (Brush)GetValue(BackgroundBrushProperty); }
        set { SetValue(BackgroundBrushProperty, value); }
    }

    public BackgroundControl()
    {
        InitializeComponent();
    }
}