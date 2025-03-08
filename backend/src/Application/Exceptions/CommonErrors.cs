using Application.Common;

namespace Application.Exceptions;

public static class CommonErrors
{
    public static Error NotFound(string entity) => new Error(
        $"{entity.ToUpperFirstLetter()}.NotFound", $"The {entity.ToLower()} was not found");
    
    public static Error NotFoundById(string entity, Guid id) => new Error(
        $"{entity.ToUpperFirstLetter()}.NotFound", $"The {entity.ToLower()} with Id '{id}' was not found");

    public static Error ValidationError(string entity, List<ErrorDetail> errorDetails) => new Error(
        $"{entity.ToUpperFirstLetter()}.ValidationError", 
        "Some validation problem occured",
        errorDetails);
}