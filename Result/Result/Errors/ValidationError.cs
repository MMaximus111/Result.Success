using System.Runtime.Serialization;

namespace Result.Errors;

[DataContract]
public class ValidationError
{
    public ValidationError(string propertyName, string severity)
    {
        PropertyName = propertyName;
        Severity = severity;
    }

    [DataMember(Order = 1)]
    public string PropertyName { get; protected set; }

    [DataMember(Order = 2)]
    public string Severity { get; protected set; }
}