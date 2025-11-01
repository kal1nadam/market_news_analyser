using MediatR;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.News.Commands;

namespace NewsAnalyzer.Application.Common.Services;

public sealed class NewsImportService : INewsImportService
{
    private readonly IMediator _mediator;

    public NewsImportService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task RunImportAsync()
    {
        await _mediator.Send(new ImportNewsCommand());
    }
}