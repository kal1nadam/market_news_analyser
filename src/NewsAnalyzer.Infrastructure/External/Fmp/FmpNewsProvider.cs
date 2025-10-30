using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.DTO.External;

namespace NewsAnalyzer.Infrastructure.External.Fmp;

public sealed class FmpNewsProvider : INewsProvider
{
    private readonly HttpClient _httpClient;
    private readonly NewsApiOptions _options;
    
    public FmpNewsProvider(IHttpClientFactory httpClientFactory, IOptions<NewsApiOptions> options)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(FmpNewsProvider));
        _options = options.Value;
    }
    
    public async Task<List<ImportNewsDto>> GetNewsAsync(CancellationToken ct = default)
    {
        var news = await FetchNewsFromFmpAsync();
        return news;
    }
    
    private async Task<List<ImportNewsDto>> FetchNewsFromFmpAsync(int page = 0, int limit = 10)
    {
        var url = "news/stock-latest" + 
                  $"?page={Uri.EscapeDataString(page.ToString())}" + 
                  $"&limit={Uri.EscapeDataString(limit.ToString())}" + 
                  $"&apikey={Uri.EscapeDataString(_options.ApiKey)}";
        
        var resp = await _httpClient.GetAsync(url);
        resp.EnsureSuccessStatusCode();
        
        // deserialize into DTO
        var news = await resp.Content.ReadFromJsonAsync<List<ImportNewsDto>>()
                        ?? [];

        return news;
    }
}