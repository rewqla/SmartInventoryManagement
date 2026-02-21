namespace SharedKernel.Exceptions;

public static class CommonErrors
{
    public static Error NotFound(string entity) => new (
        $"{entity.ToUpperFirstLetter()}.NotFound", $"The {entity.ToLower()} was not found");

    public static Error NotFoundById(string entity, Guid id) => new (
        $"{entity.ToUpperFirstLetter()}.NotFound", $"The {entity.ToLower()} with Id '{id}' was not found");

    public static Error ValidationError(string entity, List<ErrorDetail> errorDetails) => new (
        $"{entity.ToUpperFirstLetter()}.ValidationError",
        "Some validation problem occured",
        errorDetails);
}
