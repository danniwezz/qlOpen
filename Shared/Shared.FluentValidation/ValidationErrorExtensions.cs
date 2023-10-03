using FluentValidation.Results;
using Shared.Core;

namespace Shared.FluentValidation;

public static class ValidationErrorExtensions
{
    public static IEnumerable<IError> ToFailureResult(this ValidationResult validationResult)
    {
        return validationResult.Errors.Select(x => new ValidationError(x));
    }
}
