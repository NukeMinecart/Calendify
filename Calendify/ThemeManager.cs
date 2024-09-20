using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace Calendify;

public static class ThemeManager
{
    private static readonly ThemeService ThemeService = new();
    public static void ChangeTheme(ApplicationTheme theme)
    { 
        ThemeService.SetTheme(theme);   
    }

    public static ApplicationTheme GetSystemTheme()
    {
        return ThemeService.GetSystemTheme() switch
        {
            ApplicationTheme.Unknown => ApplicationTheme.Dark,
            ApplicationTheme.Dark => ApplicationTheme.Dark,
            ApplicationTheme.Light => ApplicationTheme.Light,
            ApplicationTheme.HighContrast => ApplicationTheme.HighContrast,

            _ => throw new ArgumentOutOfRangeException()
        }

        ;
    }

    public static ApplicationTheme GetCurrentTheme()
    {
        return ThemeService.GetTheme();
    }
}