namespace JournalApp.Services;

public class ThemeService
{
    private const string ThemeKey = "app_theme"; // Persistent key for user theme choice

    public string CurrentTheme { get; private set; } = "dark"; // Default theme
    public string ThemeClass => CurrentTheme == "light" ? "theme-light" : "theme-dark"; // CSS mapping

    public event Action? OnChange; // Notifies UI components of theme updates

    public ThemeService()
    {
        CurrentTheme = Preferences.Get(ThemeKey, "dark"); // Restore last selected theme
    }

    public Task ToggleAsync()
    {
        CurrentTheme = CurrentTheme == "dark" ? "light" : "dark"; // Toggle state
        Preferences.Set(ThemeKey, CurrentTheme); // Persist user preference

        OnChange?.Invoke(); // Trigger re-render for subscribed components
        return Task.CompletedTask;
    }
}
