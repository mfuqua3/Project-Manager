using System;
using System.Linq;
using System.Reflection;

namespace ProjectManager.Common.Extensions;

/// <summary>
/// Extension methods for <see cref="System.Reflection.MemberInfo"/>.
/// </summary>
public static class MemberInfoExtensions
{
    /// <summary>
    /// Tries to get the custom attribute of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of the custom attribute.</typeparam>
    /// <param name="type">The <see cref="System.Reflection.MemberInfo"/> to inspect.</param>
    /// <param name="attribute">The output parameter that receives the attribute. 
    /// Contains null if the attribute is not found. </param>
    /// <returns>True if the attribute was found, otherwise false.</returns>
    public static bool TryGetCustomAttribute<T>(this MemberInfo type, out T attribute) where T : Attribute
    {
        var customAttributes = type.GetCustomAttributes(true);
        attribute = customAttributes.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
        return attribute != null;
    }

    /// <summary>
    /// Checks if a specified attribute is present.
    /// </summary>
    /// <typeparam name="T">The type of the custom attribute.</typeparam>
    /// <param name="type">The <see cref="System.Reflection.MemberInfo"/> to inspect.</param>
    /// <returns>True if the attribute is present, otherwise false.</returns>
    public static bool HasCustomAttribute<T>(this MemberInfo type) where T : Attribute
        => type.TryGetCustomAttribute<T>(out _);
}