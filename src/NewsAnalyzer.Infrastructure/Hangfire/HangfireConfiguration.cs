using Hangfire;
using Hangfire.SQLite;
using Microsoft.Extensions.DependencyInjection;

namespace NewsAnalyzer.Infrastructure.Hangfire;

public static class HangfireConfiguration
{
    public static IServiceCollection AddHangfire(this IServiceCollection services, string connectionString)
    {
        services.AddHangfire(config =>
            config.UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSQLiteStorage(connectionString));

        services.AddHangfireServer(options =>
        {
            options.WorkerCount = 1;
        });

        return services;
    }
}