using System.Text;
using System.Text.Json.Serialization;

namespace ProjectManager.WebApi.ExceptionHandling;

public class ExceptionDetails
{
    [JsonIgnore]
    public const string ExtensionName = "error";
    public string Name { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine("Name: " + Name)
            .AppendLine("Message: " + Message)
            .Append(StackTrace)
            .ToString();
    }
}