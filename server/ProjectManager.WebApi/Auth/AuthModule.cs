using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.Common.DependencyInjection;
using ProjectManager.Features.Users;

namespace ProjectManager.WebApi.Auth;

public class AuthModule : Module<AuthorizationOptions>
{
    public override void ConfigureServices(IServiceCollection services, AuthorizationOptions options)
    {
        var jwtOptions = options.Jwt;
        services
            .AddAuthentication(ProjectManagerAuthenticationDefaults.DefaultScheme)
            .AddJwtBearer(ProjectManagerAuthenticationDefaults.DefaultScheme, opt =>
            {
                opt.TokenValidationParameters.ValidIssuer = jwtOptions.Issuer;
                opt.TokenValidationParameters.ValidAudience = jwtOptions.Audience;
                opt.TokenValidationParameters.IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
            })
            .AddJwtBearer(ProjectManagerAuthenticationDefaults.RefreshScheme, opt =>
            {
                opt.TokenValidationParameters.ValidIssuer = jwtOptions.Issuer;
                opt.TokenValidationParameters.ValidAudience = jwtOptions.Audience;
                opt.TokenValidationParameters.IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
                opt.TokenValidationParameters.ValidateLifetime = false;
            });
        services.AddAuthorization();
    }
}