using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Calendify.calendar;
using Calendify.client;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Wpf.Ui.Controls;
using MenuItem = Wpf.Ui.Controls.MenuItem;
using TextBox = Wpf.Ui.Controls.TextBox;

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
        SetUsersMenu();
    }

    private void RetrieveAndSetEvents()
    {
        var eventColorMap = new Dictionary<Event, string>();
        foreach (var user in OAuthService.ActiveUsers)
        {
            
            var calendarListResult =
                CalendarRequestService.MakeRequest<CalendarListResource.ListRequest, CalendarList>(
                    CalendarRequestService.Service.CalendarList.List(), user);

            foreach (var calendar in calendarListResult.Items)
            {
                var events = new List<Event>();
                var getEventList =
                    CalendarRequestService.MakeRequest<EventsResource.ListRequest, Events>(
                        CalendarRequestService.Service.Events.List(calendar.Id), user);
                
                var repeatEvents = getEventList.Items.Where(@event => @event.Recurrence is { Count: > 0 }).ToList();
                repeatEvents.ForEach(repeatEvent => events.AddRange(CalendarRequestService
                    .MakeRequest<EventsResource.InstancesRequest, Events>(
                        CalendarRequestService.Service.Events.Instances(calendar.Id, repeatEvent.Id), user).Items));
                
                events.AddRange(getEventList.Items);
                events.ForEach(@event => eventColorMap.Add(@event, calendar.ColorId));
                
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var animation = new DoubleAnimation(FetchBar.Value + 100f / (OAuthService.ActiveUsers.Count * calendarListResult.Items.Count), TimeSpan.FromSeconds(1));
                    FetchBar.BeginAnimation(RangeBase.ValueProperty, animation);   
                });
            }
        }

        Application.Current.Dispatcher.Invoke(() =>
        {
            FetchBar.Visibility = Visibility.Collapsed;
            CalendarGrid.Events = eventColorMap;
        });
    }

    private void SetUsersMenu()
    {
        OAuthService.FetchActiveUsers().Wait();

        OAuthService.AuthenticateUsers().Wait();

        var add = new MenuItem
        {
            Header = "Add User",
            Icon = new SymbolIcon(SymbolRegular.Add24)
        };
        add.Click += AddOnClick;

        var headers = new List<MenuItem>{add};
        foreach (var user in OAuthService.ActiveUserNames!)
        {
            Console.WriteLine(user);
            var menu = new MenuItem
            {
                StaysOpenOnClick = true,
                Header = user,
                Icon = new SymbolIcon(SymbolRegular.Person16)
            };

            var logout = new MenuItem
            {
                Header = "Logout",
                Tag = user
            };
            logout.Click += LogoutOnClick;
            menu.ItemsSource = new List<MenuItem> { logout };
            headers.Add(menu);
        }
        PeopleItem.ItemsSource = headers;
        
        FetchBar.Visibility = Visibility.Visible;
        FetchBar.Value = 0;
        Task.Run(RetrieveAndSetEvents);
    }

    private async void AddOnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "Add User",
            PrimaryButtonText = "Add",
            CloseButtonText = "Cancel",
            DialogHost = RootContentDialog
        };
        
        var dialogContent = new StackPanel();
        var label = new Label
        {
            Content = "Enter the name for the user."
        };
        var box = new TextBox();
        dialogContent.Children.Add(label);
        dialogContent.Children.Add(box);
        dialog.Content = dialogContent;

        var result = await dialog.ShowAsync(CancellationToken.None);

        if (result == ContentDialogResult.Primary)
        {
            OAuthService.ActiveUserNames!.Add(box.Text);
            SetUsersMenu();
        }
    }

    private void LogoutOnClick(object sender, RoutedEventArgs e)
    {
        var menuItem = sender as MenuItem;
        var users = OAuthService.ActiveUsers.Where(user => user.Credential!.UserId == (string)menuItem!.Tag).ToList();
        Console.WriteLine("Begin logout");
        if(users.Count >= 1) 
            users[0].Credential!.RevokeTokenAsync(CancellationToken.None);
        Console.WriteLine("End logout");

        OAuthService.ActiveUsers = OAuthService.ActiveUsers.Except(users).ToList();
        OAuthService.ActiveUserNames.Remove((string)menuItem!.Tag);
        foreach (var activeUserName in OAuthService.ActiveUserNames)
        {
            Console.WriteLine(activeUserName);
        }
        SetUsersMenu();
    }
}