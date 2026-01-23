namespace JournalApp.Services;

public class ThemeService
{
    private const string ThemeKey = "app_theme";

    public string CurrentTheme { get; private set; } = "dark";

    public string ThemeClass => CurrentTheme == "light" ? "theme-light" : "theme-dark";

    public ThemeService()
    {
        CurrentTheme = Preferences.Get(ThemeKey, "dark");
    }

    public Task ToggleAsync()
    {
        CurrentTheme = CurrentTheme == "dark" ? "light" : "dark";
        Preferences.Set(ThemeKey, CurrentTheme);
        return Task.CompletedTask;
    }
}
