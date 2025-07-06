using FluentValidation;
using FluentValidation.Results;

namespace Igloo.Presentation.Controllers.Dtos;

public record ValidationErrorResponse(string Message, IEnumerable<ValidationError> Errors)
{
    public ValidationErrorResponse(ValidationException ex) : this(
        "Invalid input data.",
        ex.Errors.Select(failure => new ValidationError(failure)).ToList()
    ) { }
}

public record ValidationError(string PropertyName, string ErrorMessage)
{
    public ValidationError(ValidationFailure failure)
        : this(failure.PropertyName, failure.ErrorMessage) { }
}
