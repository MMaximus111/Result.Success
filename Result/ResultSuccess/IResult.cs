using System.Collections.Generic;
using ResultSuccess.Errors;

namespace ResultSuccess;

public interface IResult
{
    public IReadOnlyCollection<string> Warnings { get; }

    public Error ErrorObj { get; }

    public bool IsSuccess { get; }
}

public interface IResult<out T> : IResult
{
    public T Data { get; }
}