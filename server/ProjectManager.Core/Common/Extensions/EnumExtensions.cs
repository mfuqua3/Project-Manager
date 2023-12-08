using System;

namespace ProjectManager.Common.Extensions;

/// <summary>
/// Provides extension methods for Enum.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the attribute of a specified type that is applied on the Enum member.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attribute to retrieve.</typeparam>
    /// <param name="value">The Enum member to retrieve attribute.</param>
    /// <returns>The first found attribute of type TAttribute if available; otherwise, null.</returns>
    public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
    {
        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString());

        if (memberInfo.Length <= 0)
            return null;

        if (memberInfo[0].GetCustomAttributes(typeof(TAttribute), false) is TAttribute[] { Length: > 0 } attributes)
        {
            return attributes[0];
        }

        return null;
    }
}