using System.Windows;
using System.Windows.Controls;
using Calendify.calendar;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Calendar = System.Windows.Controls.Calendar;

namespace Calendify;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        ThemeManager.ChangeTheme(ThemeManager.GetSystemTheme());
        SelectionCalendar.SelectedDate = DateTime.Today;
        var workerThread = new Thread(RetrieveAndSetEvents)
        {
            Priority = ThreadPriority.AboveNormal
        };
        workerThread.Start();
        Closed += (_, _) => workerThread.Interrupt();
        
    }

    private void RetrieveAndSetEvents()
    {
        var calendarListResult = CalendarRequestService.MakeRequest<CalendarListResource.ListRequest, CalendarList>(CalendarRequestService.Service.CalendarList.List());
        var eventColorMap = new Dictionary<Event, string>();
        
        foreach (var calendar in calendarListResult.Items)
        {
            var events = new List<Event>();
            var getEventList = CalendarRequestService.MakeRequest<EventsResource.ListRequest, Events>(CalendarRequestService.Service.Events.List(calendar.Id));
            var repeatEvents = getEventList.Items.Where(@event => @event.Recurrence is { Count: > 0 }).ToList();
            repeatEvents.ForEach(repeatEvent => events.AddRange(CalendarRequestService
                .MakeRequest<EventsResource.InstancesRequest, Events>(
                    CalendarRequestService.Service.Events.Instances(calendar.Id, repeatEvent.Id)).Items));
            events.AddRange(getEventList.Items);
            events.ForEach(@event => eventColorMap.Add(@event, calendar.ColorId));
        }

        Application.Current.Dispatcher.Invoke(() =>
        {
            FetchRing.Visibility = Visibility.Collapsed;
            CalendarGrid.Events = eventColorMap;
        });
    }
}