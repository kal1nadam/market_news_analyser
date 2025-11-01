using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NewsAnalyzer.Infrastructure.External.Fmp;

namespace NewsAnalyzer.Infrastructure.Common;

public static class HttpClientConfiguration
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<FmpNewsProvider>();

        return services;
    } 
}