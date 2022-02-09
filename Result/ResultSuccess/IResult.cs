using System.Collections.Generic;
using ResultSuccess.Errors;

namespace ResultSuccess;

/// <summary>
/// Result contract. You can create your own result.
/// </summary>
public interface IResult
{
    /// <summary>
    /// List of result warnings.
    /// </summary>
    public IReadOnlyCollection<string> Warnings { get; }

    /// <summary>
    /// Result error object.
    /// </summary>
    public Error ErrorObj { get; }

    /// <summary>
    /// Result success indicator.
    /// </summary>
    public bool IsSuccess { get; }
}

/// <summary>
/// Result generic contract.
/// </summary>
/// <typeparam name="T">Success data type.</typeparam>
public interface IResult<out T> : IResult
{
    /// <summary>
    /// Success result data.
    /// </summary>
    public T Data { get; }
}