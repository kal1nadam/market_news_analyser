using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Infrastructure.Bus;
using NewsAnalyzer.Infrastructure.Common;
using NewsAnalyzer.Infrastructure.External.News;
using NewsAnalyzer.Infrastructure.Outbox;
using NewsAnalyzer.Infrastructure.Persistence;
using NewsAnalyzer.Infrastructure.Workers;

namespace NewsAnalyzer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSingleton<IEventBus, ChannelEventBus>();
        
        services.AddScoped<INewsProvider, NewsProvider>();
        services.AddScoped<IOutboxStore, OutboxStore>();

        // Background services
        services.AddHostedService<OutboxPublisherService>();
        
        
        // Configure DbContext with SQLite
        var connectionString = configuration.GetConnectionString("AppDb") ?? "Data Source=App_Data/NewsAnalyzer.db";
        services.AddDbContext<AppDbContext>(opts => opts.UseSqlite(connectionString));
        
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        
        
        // Http Clients
        services.AddHttpClient<NewsProvider>(client =>
        {
            client.BaseAddress = new Uri("https://financialmodelingprep.com/stable/");
        });

        return services;
    }
}