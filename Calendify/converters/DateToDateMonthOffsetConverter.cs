using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Calendify.converters;

public class DateToDateMonthOffsetConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var date = (DateTime) (value ?? DateTime.Today);
        var dayOffset = new DateTime(date.Year, date.Month, 1);
        var dayOfWeek = (int)dayOffset.DayOfWeek;
        return date.AddDays(-date.Day + 1 - dayOfWeek);    
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}