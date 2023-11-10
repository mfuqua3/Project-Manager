using System.Reflection;
using System.Text.RegularExpressions;

namespace ProjectManager.Core.Utility.Encryption;

public class TypedEncryptedOptionsSettings
{
    public string Name { get; init; }
    public PropertyInfo PropertyInfo { get; init; }
    public Regex Pattern { get; init; }
}
