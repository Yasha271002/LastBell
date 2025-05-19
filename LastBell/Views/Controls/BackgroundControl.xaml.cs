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