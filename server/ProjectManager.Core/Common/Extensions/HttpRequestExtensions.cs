using Microsoft.Extensions.Primitives;

namespace ProjectManager.Common.Extensions;

/// <summary>
/// A set of extension methods for the HttpRequest class.
/// </summary>
public static class HttpRequestExtensions
{
    /// <summary>
    /// Attempts to get the value of a parameter from either the query string or route values of the HttpRequest.
    /// </summary>
    /// <param name="request">The HttpRequest instance to get the parameter value from.</param>
    /// <param name="key">The name of the parameter to get.</param>
    /// <param name="value">The value of the parameter, if found.</param>
    /// <returns>True if the parameter was found, otherwise false.</returns>
    public static bool TryGetParameter(this HttpRequest request, string key, out string value)
    {
        if (request.Query.TryGetValue(key, out var valueAsStringValues))
        {
            value = valueAsStringValues.First();
            return true;
        }

        return request.TryGetRouteValue(key, out value);
    }

    /// <summary>
    /// Attempts to get the value of a query string parameter from the HttpRequest.
    /// </summary>
    /// <param name="request">The HttpRequest instance to get the parameter value from.</param>
    /// <param name="key">The name of the parameter to get.</param>
    /// <param name="value">The value of the parameter, if found.</param>
    /// <returns>True if the parameter was found, otherwise false.</returns>
    public static bool TryGetQueryValue(this HttpRequest request, string key, out StringValues value)
        => request.Query.TryGetValue(key, out value);

    /// <summary>
    /// Attempts to get the value of a route parameter from the HttpRequest.
    /// </summary>
    /// <typeparam name="T">The type of the parameter to get.</typeparam>
    /// <param name="request">The HttpRequest instance to get the parameter value from.</param>
    /// <param name="key">The name of the parameter to get.</param>
    /// <param name="value">The value of the parameter, if found.</param>
    /// <returns>True if the parameter was found, otherwise false.</returns>
    public static bool TryGetRouteValue<T>(this HttpRequest request, string key, out T value)
    {
        ValueConverter<T> valueConverter = obj => (T)obj;
        if (typeof(T).IsAssignableTo(typeof(IConvertible)))
        {
            valueConverter = obj => (T)((IConvertible)obj).ToType(typeof(T), null);
        }
        return request.TryGetRouteValue(key, valueConverter, out value);
    }

    /// <summary>
    /// Attempts to get the value of a route parameter from the HttpRequest and convert it to a string.
    /// </summary>
    /// <param name="request">The HttpRequest instance to get the parameter value from.</param>
    /// <param name="key">The name of the parameter to get.</param>
    /// <param name="value">The value of the parameter, if found.</param>
    /// <returns>True if the parameter was found, otherwise false.</returns>
    public static bool TryGetRouteValue(this HttpRequest request, string key, out string value)
        => request.TryGetRouteValue(key, obj => obj.ToString(), out value);

    /// <summary>
    /// Attempts to get the value of a route parameter from the HttpRequest and convert it to the specified type using the specified valueConverter function.
    /// </summary>
    /// <typeparam name="T">The type of the parameter to get.</typeparam>
    /// <param name="request">The HttpRequest instance to get the parameter value from.</param>
    /// <param name="key">The name of the parameter to get.</param>
    /// <param name="valueConverter">A function to convert the parameter value to the desired type.</param>
    /// <param name="value">The value of the parameter, if found.</param>
    /// <returns>True if the parameter was found, otherwise false.</returns>
    public static bool TryGetRouteValue<T>(this HttpRequest request, string key, ValueConverter<T> valueConverter,
        out T value)
    {
        value = default;
        if (!request.RouteValues.TryGetValue(key, out var valueAsObject))
        {
            return false;
        }

        try
        {
            value = valueConverter(valueAsObject);
        }
        catch (Exception e)
        {
            throw new ArgumentException(
                "The provided value converter threw an exception while attempting to parse the route value." +
                "See the inner exception for details", nameof(valueConverter), e);
        }
        return true;
    }

    public delegate T ValueConverter<out T>(object value);
}