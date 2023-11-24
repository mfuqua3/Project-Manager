using System.Xml.XPath;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectManager.WebApi.OpenApi;

public class OpenApiXmlDocumentationModule : Common.DependencyInjection.Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<SwaggerGenOptions>(cfg =>
        {
            var xmlFileName = $"{WebApi.AssemblyName}.xml";
            var filePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
            cfg.IncludeXmlComments(filePath);
            var xmlDoc = new XPathDocument(filePath);
            cfg.OperationFilter<XmlExceptionsOperationFilter>(xmlDoc);
        });
    }
}