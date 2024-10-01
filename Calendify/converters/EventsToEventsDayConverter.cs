using System.Collections;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Google.Apis.Calendar.v3.Data;

namespace Calendify.converters;

[ValueConversion(typeof(List<Event>), typeof(List<Event>))]
public class EventsToEventsDayConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        Dictionary<Event, string> eventsMap;
        List<Event> events;
        DateTime date;
        try
        {
            eventsMap = (Dictionary<Event, string>)values[0];
            events = eventsMap.Keys.ToList();
            date = (DateTime)values[1];
        }
        catch (NullReferenceException)
        {
            return "";
        }

        var filteredEvents = events.Where(googleEvent =>
        {
            if (googleEvent.Start == null)
                return false;

            return DateTime.Parse(googleEvent.Start.Date ?? googleEvent.Start.DateTimeRaw).Date == date;
        }).Select(googleEvent => new Label{Content = googleEvent.Summary, Foreground = ColorToBrush(googleEvent.ColorId, eventsMap[googleEvent])}).ToList();
        return filteredEvents.Count > 0 ? filteredEvents : "";
    }

    private static SolidColorBrush ColorToBrush(string color, string calendarColor)
    {
        var hex = color switch
        {
            "1" => "#7986CB",
            "2" => "#33B679",
            "3" => "#8E24AA",
            "4" => "#E67C73",
            "5" => "#F6BF26",
            "6" => "#F4511E",
            "7" => "#039BE5",
            "8" => "#616161",
            "9" => "#3F51B5",
            "10" => "#0B8043",
            "11" => "#D50000",
            _ => calendarColor switch
            {
                "1" => "#795548",
                "2" => "#E67C73",
                "3" => "#D50000",
                "4" => "#F4511E",
                "5" => "#EF6C00",
                "6" => "#F09300",
                "7" => "#009688",
                "8" => "#0B8043",
                "9" => "#7CB342",
                "10" => "#C0CA33",
                "11" => "#E4C441",
                "12" => "#F6BF26",
                "13" => "#33B679",
                "14" => "#039BE5",
                "15" => "#4285F4",
                "16" => "#3F51B5",
                "17" => "#7986CB",
                "18" => "#B39DDB",
                "19" => "#616161",
                "20" => "#A79B8E",
                "21" => "#AD1457",
                "22" => "#D81B60",
                "23" => "#8E24AA",
                "24" => "#9E69AF",
                _ => "ERROR"
            }
        };

        if (hex.Equals("ERROR"))
            throw new ArgumentException("Invalid color");
        

        return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
    }

    public object[]? ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        return null;
    }
}