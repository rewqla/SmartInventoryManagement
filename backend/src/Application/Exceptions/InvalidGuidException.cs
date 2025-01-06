namespace Application.Exceptions;

// #todo: rename to not found entity
public class InvalidGuidException(string message) : Exception(message);