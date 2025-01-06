namespace Application.Common;

public sealed record Error(string Code, string Description, List<ErrorDetail> Errors = null)
{
    // #todo: add class with all possible error codes
    public static readonly Error None = new(string.Empty, string.Empty, new List<ErrorDetail>());
}