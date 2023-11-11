using System.Net;
using System.Security.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ProjectManager.WebApi.ExceptionHandling;

public static class ProblemDetailsOptionsExtensions
{
    /// <summary>
    /// Adds or extends a delegate used to customize a <see cref="ProblemDetails"/> instance during middleware execution.
    /// </summary>
    /// <param name="options">The <see cref="ProblemDetailsOptions"/> instance to configure.</param>
    /// <param name="configureProblemDetails">A delegate that is called to customize a <see cref="ProblemDetails"/> instance during middleware execution.</param>
    /// <remarks>
    /// The <paramref name="configureProblemDetails"/> delegate is added to the <see cref="ProblemDetailsOptions.CustomizeProblemDetails"/>
    /// delegate chain. When the middleware handles an exception, it creates a new <see cref="ProblemDetails"/> instance and passes it
    /// to the <see cref="ProblemDetailsOptions.CustomizeProblemDetails"/> delegate chain to allow for customization of the instance.
    /// </remarks>
    public static void Configure(this ProblemDetailsOptions options,
        Action<ProblemDetailsContext> configureProblemDetails)
    {
        ArgumentNullException.ThrowIfNull(configureProblemDetails);
        if (options.CustomizeProblemDetails == null)
        {
            options.CustomizeProblemDetails = configureProblemDetails;
        }
        else
        {
            options.CustomizeProblemDetails += configureProblemDetails;
        }
    }
    /// <summary>
    /// Adds mappings from exception types to HTTP status codes and ProblemDetails properties using the provided configuration.
    /// </summary>
    /// <param name="options">The <see cref="ProblemDetailsOptions"/> instance to configure.</param>
    /// <param name="configureBuilder">A delegate that configures the <see cref="IProblemDetailsExceptionBuilder"/> instance used to map exception types to ProblemDetails properties.</param>
    public static void MapExceptions(this ProblemDetailsOptions options, Action<IProblemDetailsExceptionBuilder> configureBuilder = null)
    {
        var builder = new ProblemDetailsExceptionBuilder();
        builder.AddDefaultMappings();
        configureBuilder?.Invoke(builder);
        var mapper = builder.Build();
        options.Configure(context =>
        {
            var httpContext = context.HttpContext;
            var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (exception is null || !mapper.TryGetMapping(exception.GetType(), out var mapping))
            {
                return;
            }
            var apiOptions = httpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            context.ProblemDetails.Status = mapping.StatusCode;
            context.ProblemDetails.Detail = mapping.StatusDetail;
            if (apiOptions.Value.ClientErrorMapping.TryGetValue(mapping.StatusCode, out var clientErrorData))
            {
                context.ProblemDetails.Type = clientErrorData.Link;
                context.ProblemDetails.Title = clientErrorData.Title;
            }
            if (context.ProblemDetails.Status == 418)
            {
                context.ProblemDetails.Type = "https://tools.ietf.org/html/rfc2324#section-2.3.2";
            }

            if (httpContext.Features.Get<IHttpResponseFeature>() != null)
            {
                httpContext.Response.StatusCode = mapping.StatusCode;
            }
        });
    }

    /// <summary>
    /// Includes additional exception details in the <see cref="ProblemDetails"/> response.
    /// </summary>
    /// <param name="options">The <see cref="ProblemDetailsOptions"/> instance to configure.</param>
    /// <remarks>
    /// Note that including detailed exception information in a response can be useful for debugging purposes during development, 
    /// but this behavior is typically disabled in a production environment to avoid revealing sensitive information about the 
    /// system to potential attackers.
    /// </remarks>
    public static void IncludeExceptionDetails(this ProblemDetailsOptions options)
    {
        options.Configure(context =>
        {
            var httpContext = context.HttpContext;
            var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            context.ProblemDetails.Extensions.Add(ExceptionDetails.ExtensionName, new ExceptionDetails
            {
                Name = exception?.GetType().Name,
                Message = exception?.Message,
                StackTrace = exception?.StackTrace
            });
        });
    }

    private static void AddDefaultMappings(this IProblemDetailsExceptionBuilder builder)
    {
        builder
            .Map<NotImplementedException>(HttpStatusCode.NotImplemented)
            .Map<ServerIsTeapotException>(418, "ImATeapot");
    }
}