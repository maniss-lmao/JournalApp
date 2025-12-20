using Microsoft.EntityFrameworkCore;
using JournalApp.Models;

namespace JournalApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Enforce one entry per day
        modelBuilder.Entity<JournalEntry>()
            .HasIndex(e => e.EntryDate)
            .IsUnique();
    }
}
