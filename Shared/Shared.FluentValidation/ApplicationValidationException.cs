using FluentValidation.Results;
using System.Runtime.Serialization;

namespace Shared.FluentValidation;
[Serializable]
public class ApplicationValidationException : Exception
{
    public ApplicationValidationException()
    {
    }

    public ApplicationValidationException(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }

    public ApplicationValidationException(string? message) : base(message)
    {
    }

    public ApplicationValidationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ApplicationValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ValidationResult ValidationResult { get; set; } = new ValidationResult();
}
