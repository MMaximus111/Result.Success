using System.Collections.Generic;

namespace ResultSuccess.Errors;

/// <summary>
/// Error contract.
/// </summary>
public interface IError
{
    /// <summary>
    /// Error identifier.
    /// </summary>
    int? ErrorId { get; }

    /// <summary>
    /// Error message.
    /// </summary>
    string ErrorMessage { get; }

    /// <summary>
    /// Message params for external error processing.
    /// </summary>
    IReadOnlyDictionary<string, object> MessageParams { get; }

    /// <summary>
    /// Detail errors list. Every error can contain internal errors.
    /// </summary>
    List<Error> Details { get; }

    /// <summary>
    /// Add error detail to current error.
    /// </summary>
    /// <param name="error">Error object.</param>
    void AddErrorDetail(Error error);
}