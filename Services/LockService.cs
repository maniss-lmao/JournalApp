using System.Security.Cryptography;
using System.Text;

namespace JournalApp.Services;

public class LockService
{
    private const string PinKey = "journal_pin_hash";
    private const string UsernameKey = "journal_username";

    public async Task<bool> HasPinAsync()
    {
        var saved = await SecureStorage.GetAsync(PinKey);
        return !string.IsNullOrWhiteSpace(saved);
    }

    public string GetUsername()
    {
        return Preferences.Get(UsernameKey, string.Empty);
    }

    public void SaveUsername(string username)
    {
        Preferences.Set(UsernameKey, username);
    }

    public async Task SavePinAsync(string pin)
    {
        var hash = Hash(pin);
        await SecureStorage.SetAsync(PinKey, hash);
    }

    public async Task<bool> VerifyPinAsync(string pin)
    {
        var savedHash = await SecureStorage.GetAsync(PinKey);
        if (string.IsNullOrWhiteSpace(savedHash)) return false;

        var inputHash = Hash(pin);
        return string.Equals(savedHash, inputHash, StringComparison.Ordinal);
    }

    public async Task ResetAsync()
    {
        SecureStorage.Remove(PinKey);
        Preferences.Remove(UsernameKey);
        await Task.CompletedTask;
    }

    private static string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}
