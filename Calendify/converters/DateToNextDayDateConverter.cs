using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Calendify.converters;

[ValueConversion(typeof(DateTime), typeof(DateTime))]
public class DateToNextDayDateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var date = (DateTime)value!;
        return date.AddDays(int.Parse(parameter!.ToString()!));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}