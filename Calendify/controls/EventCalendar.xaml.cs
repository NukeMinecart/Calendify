using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Google.Apis.Calendar.v3.Data;

namespace Calendify.controls;

public class EventCalendar : Control
{
    public static readonly DependencyProperty StartDateProperty = DependencyProperty.Register(nameof(StartDate), typeof(DateTime), typeof(EventCalendar));
    
    public DateTime StartDate
    {
        get => (DateTime)GetValue(StartDateProperty);
        set => SetValue(StartDateProperty, value);
    }
    
    public static readonly DependencyProperty EventsProperty = DependencyProperty.Register(nameof(Events), typeof(Dictionary<Event, string>), typeof(EventCalendar));
    
    public Dictionary<Event, string>? Events
    {
        get => (Dictionary<Event, string>?)GetValue(EventsProperty);
        set => SetValue(EventsProperty, value);
    }
}