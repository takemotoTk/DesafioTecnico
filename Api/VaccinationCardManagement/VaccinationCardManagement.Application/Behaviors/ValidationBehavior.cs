using FluentValidation;
using MediatR;

namespace VaccinationCardManagement.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
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
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context)));

        var validationFailures = validationResults.SelectMany(validationResult => validationResult.Errors);

        if (validationFailures.Any())
        {
            throw new ValidationException(validationFailures);
        }

        var response = await next();

        return response;
    }
}
