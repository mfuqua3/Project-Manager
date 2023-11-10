using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.WebApi.ExceptionHandling;

public static class ProblemDetailsExtensions
{
    public static bool TryGetExceptionDetails(this ProblemDetails details, out ExceptionDetails exceptionDetails)
    {
        exceptionDetails = null;
        if (!details.Extensions.ContainsKey(ExceptionDetails.ExtensionName))
        {
            return false;
        }

        var errorJson = details.Extensions[ExceptionDetails.ExtensionName].ToString();
        if (string.IsNullOrWhiteSpace(errorJson))
        {
            return false;
        }
        exceptionDetails = JsonSerializer.Deserialize<ExceptionDetails>(errorJson, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return exceptionDetails != null;
    }
}