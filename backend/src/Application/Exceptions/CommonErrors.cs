using Application.Common;

namespace Application.Exceptions;

public static class CommonErrors
{
    public static Error NotFound(string entity, Guid id) => new Error(
        $"{entity.ToUpperFirstLetter()}.NotFound", $"The follower with Id '{id}' was not found");
}