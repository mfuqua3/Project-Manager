using ProjectManager.Common.DependencyInjection;

namespace ProjectManager.WebApi.OpenApi;

public class OpenApiModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerGen(cfg =>
        {
            cfg.SwaggerDoc("v1", OpenApiDefaults.Info);
        });
        services
            .AddModule<OpenApiXmlDocumentationModule>()
            .AddModule<OpenApiAuthorizationModule>();
    }
}