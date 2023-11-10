using ProjectManager.Core.Features.Authorization.Engines;
using ProjectManager.Core.Features.Authorization.Services;
using ProjectManager.Core.Utility.Configuration;
using ProjectManager.Core.Utility.DependencyInjection;

namespace ProjectManager.Core.Features.Authorization;

public class AuthorizationModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.BindOptions<AuthorizationOptions>(x =>
        {
            x.BindNestedOptions<GoogleOptions>();
            x.BindNestedOptions<JwtOptions>();
        });
        
        services.AddScoped<IAuthorizationService, AuthorizationService>();

        services.AddSingleton<IGoogleAuthenticationEngine, GoogleAuthenticationEngine>();
        services.AddSingleton<IJwtEngine, JwtEngine>();
    }
}