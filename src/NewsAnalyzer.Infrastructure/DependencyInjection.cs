using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Common.Interfaces.Persistence;
using NewsAnalyzer.Infrastructure.Bus;
using NewsAnalyzer.Infrastructure.Common;
using NewsAnalyzer.Infrastructure.External.News;
using NewsAnalyzer.Infrastructure.Persistence;
using NewsAnalyzer.Infrastructure.Workers;

namespace NewsAnalyzer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind options
        services.Configure<NewsApiOptions>(configuration.GetSection("NewsApi"));
        
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSingleton<IEventBus, ChannelEventBus>();
        
        services.AddScoped<INewsProvider, NewsProvider>();
        services.AddScoped<IOutboxStore, OutboxStore>();

        // Background services
        services.AddHostedService<OutboxPublisherService>();
        
        
        // Configure DbContext with SQLite
        var connectionString = configuration.GetConnectionString("AppDb") ?? "Data Source=App_Data/NewsAnalyzer.db";
        services.AddDbContext<AppDbContext>(opts => opts.UseSqlite(connectionString));
        
        // Repositories
        services.AddScoped<INewsRepository, NewsRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Http Clients
        services.AddHttpClient<NewsProvider>((sp, client) =>
        {
            // Get base url from config
            var options = sp.GetRequiredService<IOptions<NewsApiOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        return services;
    }
}