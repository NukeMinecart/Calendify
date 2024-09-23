using System.IO;
using System.Windows.Controls;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Newtonsoft.Json;
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
        var calendarServiceInitializer = new BaseClientService.Initializer()
        {
            ApplicationName = "Calendify",
        };
        JsonReader reader = new JsonTextReader(new StreamReader(new FileStream("client/client_secrets.json", FileMode.Open)));
        reader.Read();
        reader.Skip();
        Console.Write(reader.ReadAsString());
        
        var calendarService = new CalendarService(calendarServiceInitializer);
    }


    private void DateSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //TODO change this to a value converter
        var date = (sender as Calendar)!.SelectedDate!.Value;
        var dayOfWeek = (int)date.DayOfWeek;
        
        CalendarGrid.StartDate = date.AddDays(-date.Day);
    }
}