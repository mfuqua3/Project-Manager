using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProjectManager.Core.Utility.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectManager.WebApi.OpenApi;

public class OpenApiAuthorizationModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<SwaggerGenOptions>(cfg =>
        {
            cfg.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, OpenApiDefaults.SecurityScheme);
            cfg.OperationFilter<AuthorizeCheckOperationFilter>();
        });
    }
}