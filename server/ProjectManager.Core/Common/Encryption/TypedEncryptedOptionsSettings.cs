using System.Reflection;
using System.Text.RegularExpressions;

namespace ProjectManager.Common.Encryption;

public class TypedEncryptedOptionsSettings
{
    public string Name { get; init; }
    public PropertyInfo PropertyInfo { get; init; }
    public Regex Pattern { get; init; }
}
