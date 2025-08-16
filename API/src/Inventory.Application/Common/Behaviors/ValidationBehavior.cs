using FluentValidation;
using MediatR;

namespace Inventory.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any()) 
            return await next();

        // Create validation context
        var context = new ValidationContext<TRequest>(request);

        // Execute all validators asynchronously
        var validationTasks = _validators
            .Select(v => v.ValidateAsync(context, cancellationToken));

        // Wait for all validations to complete
        var validationResults = await Task.WhenAll(validationTasks);

        // Collect all errors
        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next();
    }
}