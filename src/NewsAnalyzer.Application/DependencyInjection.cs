using Microsoft.Extensions.DependencyInjection;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Common.Services;

namespace NewsAnalyzer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<INewsProcessor, NewsProcessor>();
        services.AddScoped<INewsImportService, NewsImportService>();

        return services;
    }
}