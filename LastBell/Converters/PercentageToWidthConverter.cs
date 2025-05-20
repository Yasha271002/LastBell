using System.Globalization;
using System.Windows.Data;

namespace LastBell.Converters;

public class PercentageToWidthConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is not double percentage || values[1] is not double actualWidth) return 0.0;
        return (percentage / 100.0) * actualWidth;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}