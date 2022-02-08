using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Result.Errors;

namespace Result;

/// <summary>
/// Defines a result to encapsulate result logic of some action.
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
    /// Error result with error object.
    /// </summary>
    /// <param name="error">Error object</param>
    /// <returns>Error result.</returns>
    public static Result Error(Error error)
    {
        return new Result { ErrorObj = error };
    }

    /// <summary>
    /// Error result with error messages.
    /// </summary>
    /// <param name="generalErrorMessage">General error message.</param>
    /// <param name="detailErrorMessages">Detail error messages.</param>
    /// <returns>Error result.</returns>
    public static Result Error(string generalErrorMessage, params string[] detailErrorMessages)
    {
        Error error = Errors.Error.CreateError(generalErrorMessage, detailErrorMessages);

        return new Result { ErrorObj = error };
    }

    /// <summary>
    /// Add warnings to result.
    /// </summary>
    /// <param name="warnings">Collection of warnings.</param>
    public void AddWarnings(params string[] warnings)
    {
        if (warnings?.Any() == true)
        {
            Warnings ??= new List<string>();

            Warnings = Warnings.Union(warnings).ToList();   
        }
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

    /// <summary>
    /// Success result with data and params warning messages.
    /// </summary>
    /// <param name="warnings">Params array of warning messages.</param>
    /// <returns>Success result with data.</returns>
    public static Result<T> Success(T data, params string[] warnings)
    {
        return new Result<T> { Data = data, Warnings = warnings };
    }

    /// <summary>
    /// Error result with error object.
    /// </summary>
    /// <param name="error">Error object</param>
    /// <returns>Error result.</returns>
    public new static Result<T> Error(Error error)
    {
        return new Result<T> { ErrorObj = error };
    }

    /// <summary>
    /// Error result with error messages.
    /// </summary>
    /// <param name="generalErrorMessage">General error message.</param>
    /// <param name="detailErrorMessages">Detail error messages.</param>
    /// <returns>Error result.</returns>
    public new static Result<T> Error(string generalErrorMessage, params string[] detailErrorMessages)
    {
        Error error = Errors.Error.CreateError(generalErrorMessage, detailErrorMessages);

        return new Result<T> { ErrorObj = error };
    }

    /// <summary>
    /// Error result with error messages, error identifier and message params.
    /// </summary>
    /// <param name="generalErrorMessage">General error message.</param>
    /// <param name="detailErrorMessage">Detail error message.</param>
    /// <param name="errorId">Error identifier.</param>
    /// <param name="messageParams">Message params.</param>
    /// <returns>Error result.</returns>
    public static Result<T> Error(string generalErrorMessage, string detailErrorMessage, int? errorId, IReadOnlyDictionary<string, object> messageParams = null)
    {
        Error error = Errors.Error.CreateError(generalErrorMessage, detailErrorMessage, errorId, messageParams);

        return new Result<T> { ErrorObj = error };
    }
}