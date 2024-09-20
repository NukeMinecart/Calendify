using System.Windows;
using Calendify.client;
using Wpf.Ui;

namespace Calendify;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public App()
    {
        OAuthService.AuthenticateUsers();
        Exit += OnExit;
    }

    private static void OnExit(object sender, ExitEventArgs e)
    {
        OAuthService.SaveUsers();
    }
}