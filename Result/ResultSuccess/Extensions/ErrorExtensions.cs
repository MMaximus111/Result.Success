using System;
using System.Collections.Generic;
using System.Linq;
using ResultSuccess.Errors;

namespace ResultSuccess.Extensions;

public static class ErrorExtensions
{
    /// <summary>
    /// Create new error from two errors.
    /// </summary>
    /// <param name="first">First error.</param>
    /// <param name="second">Second error.</param>
    /// <param name="generalErrorMessage">General error message for two errors.</param>
    /// <returns>New error.</returns>
    public static Error Concat(this Error first, Error second, string generalErrorMessage)
    {
        if (first is null)
        {
            throw new ArgumentNullException(nameof(first));
        }
        
        if (second is null)
        {
            throw new ArgumentNullException(nameof(second));
        }
        
        Error error = Error.CreateError(generalErrorMessage, first.ErrorId);

        first.Details?.ForEach(error.AddErrorDetail);
        second.Details?.ForEach(error.AddErrorDetail);

        return error;
    }

    /// <summary>
    /// Returns detail messages from error. 
    /// </summary>
    /// <param name="error">Error object.</param>
    /// <returns>IEnumerable of messages.</returns>
    public static IEnumerable<string> GetDetailMessages(this Error error)
    {
        return error.Details?.Select(errorDetail => errorDetail.ErrorMessage);
    }
}