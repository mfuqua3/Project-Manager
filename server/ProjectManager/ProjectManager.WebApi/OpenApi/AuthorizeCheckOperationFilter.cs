using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectManager.WebApi.OpenApi;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public AuthorizeCheckOperationFilter(ProblemDetailsFactory problemDetailsFactory)
    {
        _problemDetailsFactory = problemDetailsFactory;
    }
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var controllerAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>().Any();
        var controllerAnonymous = context.MethodInfo.DeclaringType.GetCustomAttributes(false)
            .OfType<AllowAnonymousAttribute>().Any();
        var methodAuthorize = context.MethodInfo.GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>().Any();
        var methodAnonymous = context.MethodInfo.GetCustomAttributes(true)
            .OfType<AllowAnonymousAttribute>().Any();
        var authorized = methodAuthorize || 
                         (controllerAuthorize && !methodAnonymous) ||
                         (controllerAuthorize && !controllerAnonymous);
        if (!authorized)
        {
            return;
        }

        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            httpContext: new DefaultHttpContext(),
            statusCode: 401);
        operation.Responses.Add("401", new OpenApiResponse
        {
            Description = problemDetails.Detail ?? "Unauthorized request.",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/problem+json"] = new()
                {
                    Example = new OpenApiString(JsonConvert.SerializeObject(problemDetails, Formatting.Indented)),
                    Schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository)
                }
            }
        });
    }
}