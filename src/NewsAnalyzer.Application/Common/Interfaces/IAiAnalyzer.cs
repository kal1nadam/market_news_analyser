using NewsAnalyzer.Application.DTO;
using NewsAnalyzer.Application.DTO.External;

namespace NewsAnalyzer.Application.Common.Interfaces;

public interface IAiAnalyzer
{
    Task<NewsAnalysisDto?> AnalyzeNewsAsync(EnrichNewsPromptDto param, CancellationToken ct = default);
}