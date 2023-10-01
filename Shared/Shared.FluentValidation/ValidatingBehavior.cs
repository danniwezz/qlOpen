using FluentValidation;
using MediatR;
using Shared.FluentValidation;

namespace CustomerModule.Application.Shared;
public class ValidatingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatingBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        foreach (var validator in _validators)
        {
            var validationResult = await validator.ValidateAsync(
                new ValidationContext<TRequest>(request)
            );

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult);
            }
        }
        return await next();
    }
}
