using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Common.Configuration;
using ProjectManager.Common.DependencyInjection;
using ProjectManager.Features.Users.Abstractions;
using ProjectManager.Features.Users.Engines;
using ProjectManager.Features.Users.Managers;

namespace ProjectManager.Features.Users;

public class AuthorizationModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.BindOptions<AuthorizationOptions>(x =>
        {
            x.BindNestedOptions<GoogleOptions>();
            x.BindNestedOptions<JwtOptions>();
        });
        services.AddScoped<IUserCreationEngine, UserCreationEngine>();
        services.AddScoped<IAuthorizationManager, AuthorizationManager>();

        services.AddSingleton<IGoogleAuthenticationEngine, GoogleAuthenticationEngine>();
        services.AddSingleton<IJwtEngine, JwtEngine>();
    }
}