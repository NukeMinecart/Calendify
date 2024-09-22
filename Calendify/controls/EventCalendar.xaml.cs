using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Calendify.controls;

public class EventCalendar : Control
{
    public static readonly DependencyProperty StartDateProperty = DependencyProperty.Register(nameof(StartDate), typeof(DateTime), typeof(EventCalendar));
    
    public DateTime StartDate
    {
        get => (DateTime)GetValue(StartDateProperty);
        set => SetValue(StartDateProperty, value);
    }
}