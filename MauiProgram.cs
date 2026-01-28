using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using JournalApp.Data;
using JournalApp.Services;
using QuestPDF.Infrastructure;

namespace JournalApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>() // App bootstrap
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); // Global font registration
            });

        builder.Services.AddMauiBlazorWebView(); // Enables Blazor UI inside MAUI

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "journal.db"); // Local app data DB path
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}")); // SQLite EF Core configuration

        builder.Services.AddScoped<JournalService>(); // Scoped to request/render cycle
        builder.Services.AddSingleton<AppState>();    // Shared session state
        builder.Services.AddSingleton<LockService>(); // Shared lock/auth service
        builder.Services.AddSingleton<ThemeService>(); // Shared theme state + event

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools(); // Dev tools for debugging UI
        builder.Logging.AddDebug(); // Debug logging in dev builds
#endif
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community; // QuestPDF license setup

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated(); // Creates DB + tables if missing
        }

        return app;
    }
}
