using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Infrastructure.Common;
using NewsAnalyzer.Infrastructure.Persistence;

namespace NewsAnalyzer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        
        // Configure DbContext with SQLite
        var connectionString = configuration.GetConnectionString("AppDb") ?? "Data Source=App_Data/NewsAnalyzer.db";
        services.AddDbContext<AppDbContext>(opts => opts.UseSqlite(connectionString));
        
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        return services;
    }
}