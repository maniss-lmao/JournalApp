namespace JournalApp.Services;

/*
   Simple shared application state used to track
   whether the app is unlocked and which user is active.
*/
public class AppState
{
    public bool IsUnlocked { get; private set; } = false; // Indicates lock/unlock state
    public string Username { get; private set; } = string.Empty; // Active user name

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
