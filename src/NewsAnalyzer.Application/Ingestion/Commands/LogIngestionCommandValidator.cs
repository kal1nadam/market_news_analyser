using FluentValidation;

namespace NewsAnalyzer.Application.Ingestion.Commands;

public sealed class LogIngestionCommandValidator : AbstractValidator<LogIngestionCommand>
{
    public LogIngestionCommandValidator()
    {
        RuleFor(x => x.Source)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Message)
            .MaximumLength(2000);
    }
}