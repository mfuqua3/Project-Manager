using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace ProjectManager.WebApi.OpenApi;

public static class OpenApiDefaults
{
    public static OpenApiInfo Info => new()
    {
        Title = "Project Manager",
        Contact = new OpenApiContact
        {
            Email = @"matt.fuqua@fortyau.com",
            Name = @"Matt Fuqua",
            Url = new Uri(@"https://mattfuqua.dev")
        },
        Description = "Project onboarding and workflow management tool",
        License = new OpenApiLicense
        {
            Name = @"GNU Affero General Public License v3.0",
            Url = new Uri(@"https://github.com/mfuqua3/Project-Manager/blob/main/LICENSE")
        },
        Version = "v1"
    };
    public static OpenApiSecurityScheme SecurityScheme => new()
    {
        Description = "JWT Bearer Authorization Header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT"
    };
}