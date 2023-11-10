using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;

namespace ProjectManager.Tests.Utility.Extensions;

internal static class ShouldlyExtensions
{
    public static void ShouldBeInRange(this decimal? actual, decimal from, decimal to, string customMessage = null)
    {
        actual.GetValueOrDefault().ShouldBeInRange(from, to, customMessage);
    }

    public static void ShouldBeEquivalentJson(this string candidate, string against, string customMessage = null)
    {
        var candidateObject = candidate.ShouldBeValidJson(customMessage);
        var againstObject = against.ShouldBeValidJson(customMessage);
        candidateObject.ShouldBe(againstObject);
    }

    public static T ShouldDeserializeAs<T>(this string candidate, string customMessage = null)
    {
        try
        {
            var deserialized = JsonConvert.DeserializeObject<T>(candidate);
            if (deserialized != null) return deserialized;
            var error = BuildErrorMessage($"Provided value could not be deserialized as type {typeof(T).Name}.\n" +
                                          $"Provided value => {candidate}");
            throw new ShouldAssertException(error);

        }
        catch (JsonReaderException ex)
        {
            var error = BuildErrorMessage($"Provided value should be valid json string but was not.", customMessage);
            throw new ShouldAssertException(error, ex);
        }
    }

    private static JObject ShouldBeValidJson(this string jsonString, string customMessage = null)
    {
        try
        {
            return JObject.Parse(jsonString);
        }
        catch (JsonReaderException ex)
        {
            var error = BuildErrorMessage($"Provided value should be valid json string but was not. {ex.Message}", customMessage);
            throw new ShouldAssertException(error, ex);
        }
    }

    private static string BuildErrorMessage(string defaultMessage, string customMessage = null)
    {
        var error = defaultMessage;
        if (customMessage != null)
        {
            error = string.Join(". ", customMessage, error);
        }

        return error;
    }
}