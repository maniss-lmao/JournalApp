using System.Security.Cryptography;
using System.Text;

namespace JournalApp.Services;

public class LockService
{
    // PIN hash is stored in SecureStorage (more appropriate for secrets)
    private const string PinKey = "journal_pin_hash";

    // Username is stored in Preferences (not secret)
    private const string UsernameKey = "journal_username";

    // Security questions (text) + answers (hashed)
    private const string SecQ1Key = "journal_sec_q1";
    private const string SecA1Key = "journal_sec_a1_hash";
    private const string SecQ2Key = "journal_sec_q2";
    private const string SecA2Key = "journal_sec_a2_hash";

    // ---------- PIN / Username ----------
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
        await SecureStorage.SetAsync(PinKey, Hash(pin));
    }

    public async Task<bool> VerifyPinAsync(string pin)
    {
        var savedHash = await SecureStorage.GetAsync(PinKey);
        if (string.IsNullOrWhiteSpace(savedHash))
            return false;

        return string.Equals(savedHash, Hash(pin), StringComparison.Ordinal);
    }

    // ---------- Security Questions ----------
    public async Task<bool> HasSecurityQuestionsAsync()
    {
        var q1 = await SecureStorage.GetAsync(SecQ1Key);
        var a1 = await SecureStorage.GetAsync(SecA1Key);
        var q2 = await SecureStorage.GetAsync(SecQ2Key);
        var a2 = await SecureStorage.GetAsync(SecA2Key);

        return !string.IsNullOrWhiteSpace(q1)
            && !string.IsNullOrWhiteSpace(a1)
            && !string.IsNullOrWhiteSpace(q2)
            && !string.IsNullOrWhiteSpace(a2);
    }

    public async Task<(string Q1, string Q2)> GetSecurityQuestionsAsync()
    {
        var q1 = await SecureStorage.GetAsync(SecQ1Key) ?? string.Empty;
        var q2 = await SecureStorage.GetAsync(SecQ2Key) ?? string.Empty;
        return (q1, q2);
    }

    public async Task SaveSecurityQuestionsAsync(string q1, string a1, string q2, string a2)
    {
        // Questions are stored as plain text for display
        await SecureStorage.SetAsync(SecQ1Key, q1.Trim());
        await SecureStorage.SetAsync(SecQ2Key, q2.Trim());

        // Answers are normalized and hashed before storage
        await SecureStorage.SetAsync(SecA1Key, Hash(NormalizeAnswer(a1)));
        await SecureStorage.SetAsync(SecA2Key, Hash(NormalizeAnswer(a2)));
    }

    public async Task<bool> VerifySecurityAnswersAsync(string a1, string a2)
    {
        var savedA1 = await SecureStorage.GetAsync(SecA1Key);
        var savedA2 = await SecureStorage.GetAsync(SecA2Key);

        if (string.IsNullOrWhiteSpace(savedA1) || string.IsNullOrWhiteSpace(savedA2))
            return false;

        var inputA1 = Hash(NormalizeAnswer(a1));
        var inputA2 = Hash(NormalizeAnswer(a2));

        return string.Equals(savedA1, inputA1, StringComparison.Ordinal)
            && string.Equals(savedA2, inputA2, StringComparison.Ordinal);
    }

    // ---------- Account reset ----------
    // Used when the user cannot answer security questions or wants to start fresh.
    public async Task ResetAccountAsync()
    {
        SecureStorage.Remove(PinKey);

        SecureStorage.Remove(SecQ1Key);
        SecureStorage.Remove(SecA1Key);
        SecureStorage.Remove(SecQ2Key);
        SecureStorage.Remove(SecA2Key);

        Preferences.Remove(UsernameKey);

        await Task.CompletedTask;
    }

    private static string NormalizeAnswer(string input)
        => (input ?? string.Empty).Trim().ToLowerInvariant();

    private static string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input ?? string.Empty));
        return Convert.ToBase64String(bytes);
    }
}
