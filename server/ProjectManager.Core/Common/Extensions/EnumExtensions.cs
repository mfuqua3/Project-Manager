using System;

namespace ProjectManager.Common.Extensions;

public static class EnumExtensions
{
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
