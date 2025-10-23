using NewsAnalyzer.Application.DTO;

namespace NewsAnalyzer.Application.Common.Interfaces;

public interface INewsProvider
{
    Task<List<NewsDto>> GetNewsAsync(CancellationToken ct = default);
}