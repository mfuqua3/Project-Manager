using System.Reflection;
using System.Xml.XPath;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectManager.WebApi.OpenApi;

public class XmlExceptionsOperationFilter : IOperationFilter
{
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly IHttpContextFactory _httpContextFactory;
    private readonly IServiceProvider _serviceProvider;
    private readonly XPathNavigator _xmlNavigator;

    public XmlExceptionsOperationFilter(
        XPathDocument xmlDoc, 
        ProblemDetailsFactory problemDetailsFactory, 
        IHttpContextFactory httpContextFactory,
        IServiceProvider serviceProvider)
    {
        _problemDetailsFactory = problemDetailsFactory;
        _httpContextFactory = httpContextFactory;
        _serviceProvider = serviceProvider;
        _xmlNavigator = xmlDoc.CreateNavigator();
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo == null) return;

        // If method is from a constructed generic type, look for comments from the generic type method
        var targetMethod = context.MethodInfo.DeclaringType.IsConstructedGenericType
            ? context.MethodInfo.GetUnderlyingGenericTypeMethod()
            : context.MethodInfo;

        if (targetMethod == null) return;

        var exceptions = GetExceptionTypes(operation, targetMethod);
        foreach (var (exceptionType, description) in exceptions)
        {
            if (exceptionType.GetConstructor(Array.Empty<Type>()) == null)
            {
                throw new InvalidOperationException(
                    $"{exceptionType.Name} may not be used for auto-generated OpenApi docs, it must have a parameterless constructor");
            }

            var featureCollection = new FeatureCollection();
            featureCollection.Set<IExceptionHandlerFeature>(new ExceptionHandlerFeature
            {
                Error = (Exception)Activator.CreateInstance(exceptionType)!
            });;
            var httpContext = _httpContextFactory.Create(featureCollection);
            httpContext.RequestServices = _serviceProvider;
            var problemDetail = _problemDetailsFactory.CreateProblemDetails(httpContext);
            if (!problemDetail.Status.HasValue || problemDetail.Status.Value == 500 ||
                operation.Responses.ContainsKey(problemDetail.Status.Value.ToString()))
            {
                continue;
            }
            operation.Responses.Add(problemDetail.Status.Value.ToString(), new OpenApiResponse
            {
                Description = description,
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new()
                    {
                        Example = new OpenApiString(JsonConvert.SerializeObject(problemDetail, Formatting.Indented)),
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails),
                            context.SchemaRepository)
                    }
                }
            });
        }
    }

    private IEnumerable<(Type, string)> GetExceptionTypes(OpenApiOperation operation, MethodInfo methodInfo)
    {
        var methodMemberName = XmlCommentsNodeNameHelper.GetMemberNameForMethod(methodInfo);
        var methodNode = _xmlNavigator.SelectSingleNode($"/doc/members/member[@name='{methodMemberName}']");

        if (methodNode == null) yield break;

        var exceptionNodes = methodNode.Select("exception");
        foreach (XPathNavigator exceptionNode in exceptionNodes)
        {
            var exceptionTypeString = exceptionNode.GetAttribute("cref", string.Empty);
            if (exceptionTypeString.StartsWith("T:"))
            {
                exceptionTypeString = exceptionTypeString[2..];
            }

            if (exceptionTypeString.StartsWith(ProjectManagerApplication.AssemblyName))
            {
                exceptionTypeString += $", {ProjectManagerApplication.AssemblyName}";
            }
            var exceptionType = Type.GetType(exceptionTypeString, throwOnError: true);
            if (!exceptionType.IsAssignableTo(typeof(Exception)))
            {
                throw new InvalidOperationException(
                    $"Type {exceptionType.Name} appended as an error to {methodInfo.Name} is not an exception type");
            }

            yield return (exceptionType, XmlCommentsTextHelper.Humanize(exceptionNode.InnerXml));
        }
    }
}