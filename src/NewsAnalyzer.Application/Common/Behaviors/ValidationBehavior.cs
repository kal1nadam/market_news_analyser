using FluentValidation;
using MediatR;

namespace NewsAnalyzer.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = results.SelectMany(r => r.Errors).Where(f => f is not null).ToArray();
            
            if (failures.Length != 0)
            {
                var message = "Validation failed: " + string.Join("; ", failures.Select(f => $"{f.PropertyName} {f.ErrorMessage}"));
                throw new ValidationException(message, failures);
            }
        }

        return await next(cancellationToken);
    }
}