using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Google.Apis.Calendar.v3.Data;

namespace Calendify.converters;

[ValueConversion(typeof(List<Event>), typeof(List<Event>))]
public class EventsToEventsDayConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is null) return DependencyProperty.UnsetValue;
        if(parameter is null) return DependencyProperty.UnsetValue;

        var events = (List<Event>)value;
        var date = (DateTime)parameter;
        
        return events.Select(googleEvent => DateTime.Parse(googleEvent.Start.Date) == date);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}