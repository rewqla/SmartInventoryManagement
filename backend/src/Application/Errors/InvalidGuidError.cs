namespace Application.Errors;

public class InvalidGuidError(string message) : Exception(message);