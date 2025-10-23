using MediatR;

namespace NewsAnalyzer.Application.News.Commands;

public sealed record ImportNewsCommand : IRequest<int>;