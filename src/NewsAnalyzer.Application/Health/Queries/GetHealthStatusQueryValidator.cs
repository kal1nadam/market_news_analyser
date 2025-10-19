using FluentValidation;

namespace NewsAnalyzer.Application.Health.Queries;

public sealed class GetHealthStatusQueryValidator : AbstractValidator<GetHealthStatusQuery>
{
    public GetHealthStatusQueryValidator()
    {
        RuleFor(_ => _).NotNull();
    }
}