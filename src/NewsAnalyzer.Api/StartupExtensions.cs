using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Infrastructure.Persistence;

namespace NewsAnalyzer.Api;

public static class StartupExtensions
{
    public static async Task EnsureDbCreatedAndMigratedAsync(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Create directory for SQLite database file if it doesn't exist
        var connectionString = db.Database.GetConnectionString();
        if (!string.IsNullOrWhiteSpace(connectionString) &&
            connectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase))
        {
            var path = connectionString.Split("Data Source=")[1].Split(';')[0].Trim();
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        
        await db.Database.MigrateAsync();
    }
}