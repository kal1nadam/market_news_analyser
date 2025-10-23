using NewsAnalyzer.Application.DTO;
using NewsAnalyzer.Application.DTO.External;

namespace NewsAnalyzer.Application.Common.Interfaces;

public interface INewsProvider
{
    Task<List<ImportNewsDto>> GetNewsAsync(CancellationToken ct = default);
}