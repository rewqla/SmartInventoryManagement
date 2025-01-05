﻿using Application.Common;
using FluentValidation.Results;

namespace Application.Mapping.Errors;

public static class ValidationErrorMapper
{
    public static List<ValidationFailure> MapToValidationFailures(List<ErrorDetail> errorDetails)
    {
        return errorDetails.Select(e => new ValidationFailure(e.PropertyName, e.ErrorMessage)).ToList();
    }
}