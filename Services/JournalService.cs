using JournalApp.Data;
using JournalApp.Models;
using Microsoft.EntityFrameworkCore;

namespace JournalApp.Services;

public class JournalService
{
    private readonly AppDbContext _db;

    public JournalService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<JournalEntry?> GetByDateAsync(DateOnly date)
    {
        return await _db.JournalEntries
            .FirstOrDefaultAsync(e => e.EntryDate == date);
    }

    public async Task SaveAsync(JournalEntry entry)
    {
        var existing = await GetByDateAsync(entry.EntryDate);

        if (existing == null)
        {
            entry.CreatedAt = DateTime.Now;
            entry.UpdatedAt = DateTime.Now;

            _db.JournalEntries.Add(entry);
        }
        else
        {
            existing.Title = entry.Title;
            existing.Content = entry.Content;

            existing.PrimaryMood = entry.PrimaryMood;
            existing.SecondaryMood1 = entry.SecondaryMood1;
            existing.SecondaryMood2 = entry.SecondaryMood2;

            existing.Tags = entry.Tags;

            existing.UpdatedAt = DateTime.Now;
        }

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(DateOnly date)
    {
        var entry = await GetByDateAsync(date);
        if (entry != null)
        {
            _db.JournalEntries.Remove(entry);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<JournalEntry>> GetAllAsync()
    {
        return await _db.JournalEntries
            .OrderByDescending(e => e.EntryDate)
            .ToListAsync();
    }

    public async Task<List<JournalEntry>> SearchAsync(string? query, DateOnly? fromDate, DateOnly? toDate)
    {
        var q = _db.JournalEntries.AsQueryable();

        if (fromDate.HasValue)
            q = q.Where(e => e.EntryDate >= fromDate.Value);

        if (toDate.HasValue)
            q = q.Where(e => e.EntryDate <= toDate.Value);

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = query.Trim();

            q = q.Where(e =>
                EF.Functions.Like(e.Title, $"%{term}%") ||
                EF.Functions.Like(e.Content, $"%{term}%"));
        }

        return await q
            .OrderByDescending(e => e.EntryDate)
            .ToListAsync();
    }

    public async Task<int> GetCountAsync()
    {
        return await _db.JournalEntries.CountAsync();
    }

    public async Task<List<JournalEntry>> GetPageAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        return await _db.JournalEntries
            .OrderByDescending(e => e.EntryDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<JournalEntry>> GetRangeAsync(DateOnly fromDate, DateOnly toDate)
    {
        return await _db.JournalEntries
            .Where(e => e.EntryDate >= fromDate && e.EntryDate <= toDate)
            .OrderByDescending(e => e.EntryDate)
            .ToListAsync();
    }
}
