using System.Windows;
using System.Windows.Controls;
using Google.Apis.Calendar.v3.Data;

namespace Calendify.controls;

public class EventDay : Control
{
    public static readonly DependencyProperty DateProperty = DependencyProperty.Register(nameof(Date), typeof(DateTime), typeof(EventDay));
    
    public DateTime? Date
    {
        get => (DateTime?)GetValue(DateProperty);
        set => SetValue(DateProperty, value);
    }
    public static readonly DependencyProperty EventsProperty = DependencyProperty.Register(nameof(Events), typeof(List<Event>), typeof(EventDay));
    
    public List<Event>? Events
    {
        get => (List<Event>?)GetValue(EventsProperty);
        set => SetValue(EventsProperty, value);
    }
}