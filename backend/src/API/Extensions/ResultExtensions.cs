using Error = SharedKernel.Error;

namespace API.Extensions;

public static class ResultExtensions
{
    public static IResult Match<T>(
        this SharedKernel.ResultPattern.Result<T> result,
        Func<T, IResult> onSuccess,
        Func<Error, IResult> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
    }
}