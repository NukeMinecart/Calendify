using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Calendify.converters;

[ValueConversion(typeof(DateTime), typeof(string))]
public class DateToDayConverter : IValueConverter
{
    private enum Month
    {
        Jan = 1,
        Feb,
        Mar,
        Apr,
        May,
        Jun,
        Jul,
        Aug,
        Sep,
        Oct,
        Nov,
        Dec
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var date = (DateTime)value!;

        if (date.Day == 1)
            return (Month)date.Month + " " +date.Day; 
        
        return date.Day;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}