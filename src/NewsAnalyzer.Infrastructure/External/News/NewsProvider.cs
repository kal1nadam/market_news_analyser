using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.DTO;
using NewsAnalyzer.Application.DTO.External;

namespace NewsAnalyzer.Infrastructure.External.News;

public sealed class NewsProvider : INewsProvider
{
    private readonly HttpClient _httpClient;
    private readonly NewsApiOptions _options;
    
    public NewsProvider(IHttpClientFactory httpClientFactory, IOptions<NewsApiOptions> options)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(NewsProvider));
        _options = options.Value;
    }
    
    public async Task<List<ImportNewsDto>> GetNewsAsync(CancellationToken ct = default)
    {
        // TODO fetch real data
        ImportNewsDto mockDto = new()
        {
            Image = "image test",
            PublishedDate = DateTime.UtcNow,
            Publisher = "publisher test",
            Site = "site test",
            Symbol = "TEST",
            Text = "This is a test news article for unit testing purposes.",
            Title = "Test News Article",
            Url = "url test"
        };

        return [mockDto];
    }
    
    private async Task<List<ImportNewsDto>> FetchNewsFromFmpAsync(int page = 0, int limit = 50)
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