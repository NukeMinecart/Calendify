using System.Windows.Controls;

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
    }


    private void DateSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var date = (sender as Calendar)!.SelectedDate!.Value;
        var dayOfWeek = (int)date.DayOfWeek;
        
        CalendarGrid.StartDate = date.AddDays(-date.Day);
    }
}