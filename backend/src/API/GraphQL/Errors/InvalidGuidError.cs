namespace API.GraphQL.Errors;

public class InvalidGuidError(string message) : Exception(message);