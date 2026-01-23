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
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "journal.db");
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        builder.Services.AddScoped<JournalService>();
        builder.Services.AddSingleton<AppState>();
        builder.Services.AddSingleton<LockService>();
        builder.Services.AddSingleton<ThemeService>();
        



#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;


        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        }

        return app;

    }
 
}

