using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Result.Errors
{
    [DataContract]
    public class Error
    {
        public Error()
        {
        }

        public Error(int? errorId, string message)
        {
            ErrorId = errorId;
            ErrorMessage = message;
        }

        public Error(int? errorId, string message, IReadOnlyDictionary<string, object> messageParams)
        {
            ErrorId = errorId;
            ErrorMessage = message;
            MessageParams = messageParams;
        }

        [DataMember(Order = 1)]
        public int? ErrorId { get; protected set; }

        [DataMember(Order = 2)]
        public string ErrorMessage { get; protected set; }

        [DataMember(Order = 3)]
        public IReadOnlyDictionary<string, object> MessageParams { get; protected set; }

        [DataMember(Order = 4)]
        public List<Error> Details { get; protected set; }

        public static Error CreateError(string generalErrorMessage, string detailErrorMessage)
        {
            Error error = new (null, generalErrorMessage);

            Error detailError = new (null, detailErrorMessage);

            error.AddErrorDetail(detailError);

            return error;
        }

        public static Error CreateError(string generalErrorMessage, string detailErrorMessage, int? errorId, IReadOnlyDictionary<string, object> messageParams)
        {
            Error error = new(errorId, generalErrorMessage);

            Error detailError = new(errorId, detailErrorMessage, messageParams);

            error.AddErrorDetail(detailError);

            return error;
        }

        public static Error CreateError(string generalErrorMessage, IEnumerable<string> detailErrorMessages)
        {
            Error error = new (null, generalErrorMessage);

            foreach (string detailErrorMessage in detailErrorMessages)
            {
                Error detailError = new (null, detailErrorMessage);

                error.AddErrorDetail(detailError);
            }

            return error;
        }

        private void AddErrorDetail(Error error)
        {
            Details ??= new List<Error>();

            Details.Add(error);
        }
    }
}