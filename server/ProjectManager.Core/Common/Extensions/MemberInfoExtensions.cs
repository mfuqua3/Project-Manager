using System.Reflection;

namespace ProjectManager.Common.Extensions;

public static class MemberInfoExtensions
{
    public static bool TryGetCustomAttribute<T>(this MemberInfo type, out T attribute) where T : Attribute
    {
        var customAttributes = type.GetCustomAttributes(true);
        attribute = customAttributes.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
        return attribute != null;
    }

    public static bool HasCustomAttribute<T>(this MemberInfo type) where T : Attribute
        => type.TryGetCustomAttribute<T>(out _);
}