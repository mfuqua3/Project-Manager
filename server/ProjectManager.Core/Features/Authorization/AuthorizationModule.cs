using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Common.Configuration;
using ProjectManager.Common.DependencyInjection;
using ProjectManager.Features.Authorization.Abstractions;
using ProjectManager.Features.Authorization.Engines;
using ProjectManager.Features.Authorization.Managers;

namespace ProjectManager.Features.Authorization;

public class AuthorizationModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.BindOptions<AuthorizationOptions>(x =>
        {
            x.BindNestedOptions<GoogleOptions>();
            x.BindNestedOptions<JwtOptions>();
        });
        
        services.AddScoped<IAuthorizationManager, AuthorizationManager>();

        services.AddSingleton<IGoogleAuthenticationEngine, GoogleAuthenticationEngine>();
        services.AddSingleton<IJwtEngine, JwtEngine>();
    }
}