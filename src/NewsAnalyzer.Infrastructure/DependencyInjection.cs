using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Common.Interfaces.Persistence;
using NewsAnalyzer.Infrastructure.Bus;
using NewsAnalyzer.Infrastructure.Common;
using NewsAnalyzer.Infrastructure.External.Fmp;
using NewsAnalyzer.Infrastructure.External.OpenAi;
using NewsAnalyzer.Infrastructure.Hangfire;
using NewsAnalyzer.Infrastructure.Persistence;
using NewsAnalyzer.Infrastructure.SignalR;
using NewsAnalyzer.Infrastructure.Workers;

namespace NewsAnalyzer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind options
        services.Configure<NewsApiOptions>(configuration.GetSection("NewsApi"));
        services.Configure<OpenAiApiOptions>(configuration.GetSection("OpenAiApi"));
        
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSingleton<IEventBus, ChannelEventBus>();
        
        // Must be call to register HubContext used in the notifier
        services.AddSignalR();
        services.AddScoped<INewsNotifier, NewsNotifier>();
        
        services.AddScoped<INewsProvider, FmpNewsProvider>();
        services.AddScoped<IAiAnalyzer, OpenAiAnalyzer>();
        services.AddScoped<IOutboxStore, OutboxStore>();

        services.AddScoped<IBackgroundJobScheduler, BackgroundJobScheduler>();
        
        // Background services
        services.AddHostedService<OutboxPublisherService>();
        services.AddHostedService<NewsAnalyzerHostedService>();
        
        var connectionString = configuration.GetConnectionString("AppDb") ?? "Data Source=App_Data/NewsAnalyzer.db;";
        
        // Configure DbContext with SQLite
        services.AddDbContext<AppDbContext>(opts => opts.UseSqlite(connectionString));
        
        // Add Hangfire
        services.AddHangfire(connectionString);
        
        // Repositories
        services.AddScoped<INewsRepository, NewsRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}