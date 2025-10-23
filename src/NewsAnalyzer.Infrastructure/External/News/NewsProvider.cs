using System.Net.Http.Json;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.DTO;

namespace NewsAnalyzer.Infrastructure.External.News;

public sealed class NewsProvider : INewsProvider
{
    private readonly HttpClient _httpClient;
    
    public NewsProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(NewsProvider));
    }
    
    public async Task<List<NewsDto>> GetNewsAsync(CancellationToken ct = default)
    {
        // TODO mock data
        throw new NotImplementedException();
    }
    
    private async Task<List<NewsDto>> FetchNewsFromFmpAsync(int page = 0, int limit = 50)
    {
        // var url = "news/stock-latest" + 
        //           $"?page={Uri.EscapeDataString(page.ToString())}" + 
        //           $"&limit={Uri.EscapeDataString(limit.ToString())}" + 
        //           $"&apikey={Uri.EscapeDataString(_fmpSettings.Key)}"; // TODO get api key from config
        //
        // var resp = await _httpClient.GetAsync(url);
        // resp.EnsureSuccessStatusCode();
        //
        // // deserialize into DTO
        // var news = await resp.Content.ReadFromJsonAsync<List<NewsDto>>()
        //                 ?? [];

        // return news;
        throw new NotImplementedException();
    }
}