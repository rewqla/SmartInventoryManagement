namespace API.GraphQL.Errors;

internal class InvalidGuidError(string message) : Exception(message);