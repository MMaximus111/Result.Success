using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ResultSuccess.Errors
{
    /// <summary>
    /// Defines an error to encapsulate error logic of some action.
    /// </summary>
    [DataContract]
    public class Error : IError
    {
        private Error(string message, int? errorId = null)
        {
            ErrorId = errorId;
            ErrorMessage = message;
        }

        private Error(int? errorId, string message, IReadOnlyDictionary<string, object> messageParams)
        {
            ErrorId = errorId;
            ErrorMessage = message;
            MessageParams = messageParams;
        }

        /// <summary>
        /// Error identifier.
        /// </summary>
        [DataMember(Order = 1)]
        public int? ErrorId { get; protected set; }

        /// <summary>
        /// Error message.
        /// </summary>
        [DataMember(Order = 2)]
        public string ErrorMessage { get; protected set; }

        /// <summary>
        /// Message params.
        /// </summary>
        [DataMember(Order = 3)]
        public IReadOnlyDictionary<string, object> MessageParams { get; protected set; }

        /// <summary>
        /// Detail error
        /// </summary>
        [DataMember(Order = 4)]
        public List<Error> Details { get; protected set; }

        /// <summary>
        /// Create error with error messages and message params.
        /// </summary>
        /// <param name="generalErrorMessage">General message about error.</param>
        /// <param name="detailErrorMessage">Detailed message about error.</param>
        /// <param name="errorId">Error identifier.</param>
        /// <param name="messageParams">Message parameters.</param>
        /// <returns>Error object.</returns>
        public static Error CreateError(string generalErrorMessage, string detailErrorMessage, int? errorId, IReadOnlyDictionary<string, object> messageParams = null)
        {
            Error error = new(generalErrorMessage, errorId);

            Error detailError = new(errorId, detailErrorMessage, messageParams);

            error.AddErrorDetail(detailError);

            return error;
        }

        public static Error CreateError(string generalErrorMessage, int? errorId)
        {
            return new Error(generalErrorMessage, errorId);
        }

        /// <summary>
        /// Create error with error messages.
        /// </summary>
        /// <param name="generalErrorMessage">General error message.</param>
        /// <param name="detailErrorMessages">Detail error messages params array.</param>
        /// <returns>Error object.</returns>
        public static Error CreateError(string generalErrorMessage, params string[] detailErrorMessages)
        {
            Error error = new (generalErrorMessage);

            if (detailErrorMessages?.Any() == true)
            {
                foreach (string detailErrorMessage in detailErrorMessages)
                {
                    Error detailError = new (detailErrorMessage);

                    error.AddErrorDetail(detailError);
                }
            }

            return error;
        }

        /// <summary>
        /// Add error detail to current error.
        /// </summary>
        /// <param name="error">Error object.</param>
        public void AddErrorDetail(Error error)
        {
            if (error is null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            Details ??= new List<Error>();

            Details.Add(error);
        }
    }
}