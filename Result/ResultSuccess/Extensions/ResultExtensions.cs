namespace ResultSuccess.Extensions;

public static class ResultExtensions
{
    public static Result<T> CastError<T>(this Result result)
    { 
        return Result<T>.Error(result.ErrorObj);
    }
}