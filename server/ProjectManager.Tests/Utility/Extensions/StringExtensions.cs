using System.Text.Json;
using Newtonsoft.Json;

namespace ProjectManager.Tests.Utility.Extensions;

internal static class StringExtensions
{
    public static string CamelCase(this string str)
        => JsonNamingPolicy.CamelCase.ConvertName(str);

    public static T DeserializedAs<T>(this string str, JsonSerializerSettings serializerSettings = null) 
        => JsonConvert.DeserializeObject<T>(str, serializerSettings);
}