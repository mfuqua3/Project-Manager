using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.WebApi.ExceptionHandling;
using Shouldly;

namespace ProjectManager.Tests.Utility.Extensions;

internal static class HttpResponseExtensions
{
    public static async Task ShouldBeSuccessAsync(this HttpResponseMessage httpResponse)
    {
        if (httpResponse.IsSuccessStatusCode)
        {
            return;
        }

        var content = await httpResponse.Content.ReadAsStringAsync();
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(content);
        httpResponse.IsSuccessStatusCode.ShouldBeTrue(problemDetails.Print());
    }

    private static string Print(this ProblemDetails details)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Title: " + details.Title);
        sb.AppendLine("Type: " + details.Type);
        sb.AppendLine("Status: " + details.Status);
        sb.AppendLine("Detail: " + details.Detail);
        if (details.TryGetExceptionDetails(out var exceptionDetails))
        {
            sb.Append(exceptionDetails);
        }

        return sb.ToString();
    }
}