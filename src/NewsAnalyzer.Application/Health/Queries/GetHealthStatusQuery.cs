using MediatR;

namespace NewsAnalyzer.Application.Health.Queries;

public sealed record GetHealthStatusQuery : IRequest<HealthStatusDto>;

public sealed record HealthStatusDto(string Status, DateTime ServerUtc, string ServiceName);