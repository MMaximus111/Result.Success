using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Result.Errors;

namespace Result;

/// <summary>
/// Defines a result to incapsulate result logic of some action.
/// </summary>
[DataContract]
public class Result : IResult
{
    /// <summary>
    /// Collection of result warnings.
    /// </summary>
    [DataMember(Order = 1)]
    public IReadOnlyCollection<string> Warnings { get; protected set; }

    /// <summary>
    /// Error object.
    /// </summary>
    [DataMember(Order = 2)]
    public Error ErrorObj { get; protected set; }

    /// <summary>
    /// Indicator of success result.
    /// </summary>
    [DataMember(Order = 3)]
    public bool IsSuccess => ErrorObj is null;

    /// <summary>
    /// Success result with warning messages.
    /// </summary>
    /// <param name="warnings">Params array of warning messages.</param>
    /// <returns>Success result.</returns>
    public static Result Success(params string[] warnings)
    {
        return new Result { Warnings = warnings };
    }

    /// <summary>
    /// Success result with only one warning message.
    /// </summary>
    /// <param name="warning">Warning message.</param>
    /// <returns>Success result.</returns>
    public static Result Success(string warning)
    {
        return new Result { Warnings = string.IsNullOrWhiteSpace(warning) ? Array.Empty<string>() : new[] { warning } };
    }

    public static Result<T> Success<T>(T data, params string[] warnings)
    {
        return Result<T>.Success(data, warnings);
    }

    public static Result Error(Error error)
    {
        return new Result { ErrorObj = error };
    }

    public static Result<T> Error<T>(Error error)
    {
        return Result<T>.Error(error);
    }

    public static Result Error(string generalErrorMessage, string detailErrorMessage)
    {
        Error error = Errors.Error.CreateError(generalErrorMessage, detailErrorMessage);

        return new Result { ErrorObj = error };
    }

    public static Result Error(string generalErrorMessage, IReadOnlyCollection<string> detailErrorMessages)
    {
        Error error = Errors.Error.CreateError(generalErrorMessage, detailErrorMessages);

        return new Result { ErrorObj = error };
    }

    public void AddWarnings(IEnumerable<string> warnings)
    {
        Warnings ??= new List<string>();

        Warnings = Warnings.Union(warnings).ToList();
    }
}

/// <summary>
/// Defines an generic inheritor of the base result class with the ability to return a success object.
/// </summary>
/// <typeparam name="T">Data object type for success result.</typeparam>
[DataContract]
public sealed class Result<T> : Result, IResult<T>
{
    /// <summary>
    /// Data object. Filled only in case of successful result.
    /// </summary>
    [DataMember(Order = 4)]
    public T Data { get; set; }

    public static Result<T> Success(T data, params string[] warnings)
    {
        return new Result<T> { Data = data, Warnings = warnings };
    }

    public new static Result<T> Error(Error error)
    {
        return new Result<T> { ErrorObj = error };
    }

    public new static Result<T> Error(string generalErrorMessage, string detailErrorMessage)
    {
        Error error = Errors.Error.CreateError(generalErrorMessage, detailErrorMessage);

        return new Result<T> { ErrorObj = error };
    }

    public static Result<T> Error(string generalErrorMessage, string detailErrorMessage, int? errorId, IReadOnlyDictionary<string, object> messageParams = null)
    {
        Error error = Errors.Error.CreateError(generalErrorMessage, detailErrorMessage, errorId, messageParams);

        return new Result<T> { ErrorObj = error };
    }

    public static Result<T> Error(string generalErrorMessage, IEnumerable<string> detailErrorMessages)
    {
        Error error = Errors.Error.CreateError(generalErrorMessage, detailErrorMessages);

        return new Result<T> { ErrorObj = error };
    }
}