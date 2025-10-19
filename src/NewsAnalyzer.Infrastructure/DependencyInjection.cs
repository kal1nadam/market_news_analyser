using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Infrastructure.Common;

namespace NewsAnalyzer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}