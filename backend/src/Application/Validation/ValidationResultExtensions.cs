using Application.Common;

namespace Application.Validation;

public static class ValidationResultExtensions
{
    public static List<ErrorDetail> ToErrorDetails(this FluentValidation.Results.ValidationResult validationResult)
    {
        return validationResult.Errors.Select(error => new ErrorDetail
        {
            PropertyName = error.PropertyName,
            ErrorMessage = error.ErrorMessage
        }).ToList();
    }
}