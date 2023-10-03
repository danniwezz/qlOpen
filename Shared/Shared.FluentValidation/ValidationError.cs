using FluentValidation.Results;
using Shared.Core;

namespace Shared.FluentValidation;
public record ValidationError : IError
{
    private readonly ValidationFailure _validationFailure;

    public ValidationError(ValidationFailure validationFailure)
    {
        _validationFailure = validationFailure;
    }
    public string Type => typeof(ValidationError).Name;

    public string Message => _validationFailure.ErrorMessage;


    public Dictionary<string, object?> Data =>
        new()
        {
            { "propertyName", _validationFailure.PropertyName },
            { "attemptedValue", _validationFailure.AttemptedValue },
        };
}
