namespace JournalApp.Services;

public class AppState
{
    public bool IsUnlocked { get; private set; } = false;
    public string Username { get; private set; } = string.Empty;

    public void SetUnlocked(string username)
    {
        IsUnlocked = true;
        Username = username;
    }

    public void Lock()
    {
        IsUnlocked = false;
    }
}
