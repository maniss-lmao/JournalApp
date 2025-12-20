namespace JournalApp.Models;

public class JournalEntry
{
    public int Id { get; set; }

    // One journal entry per day
    public DateOnly EntryDate { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty; // Markdown text

    // Mood tracking
    public string PrimaryMood { get; set; } = string.Empty;
    public string? SecondaryMood1 { get; set; }
    public string? SecondaryMood2 { get; set; }

    // Tags (comma-separated)
    public string Tags { get; set; } = string.Empty;

    // System timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
