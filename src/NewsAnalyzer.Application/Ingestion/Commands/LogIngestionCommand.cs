using MediatR;

namespace NewsAnalyzer.Application.Ingestion.Commands;

public sealed record LogIngestionCommand(string Source, string? Message) : IRequest<int>;