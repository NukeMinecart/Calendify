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

        Application.Current.Dispatcher.Invoke(() => CalendarGrid.Events = eventColorMap);
    }


    private void DateSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var date = (sender as Calendar)!.SelectedDate!.Value;
        var dayOffset = new DateTime(date.Year, date.Month, 1);
        var dayOfWeek = (int)dayOffset.DayOfWeek;
        CalendarGrid.StartDate = date.AddDays(-date.Day + 1 - dayOfWeek);
    }
}