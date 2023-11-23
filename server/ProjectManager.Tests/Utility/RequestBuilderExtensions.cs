using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Common.Extensions;
using ProjectManager.Data.Entities;
using ProjectManager.Features.Authorization.Abstractions;
using ProjectManager.Tests.IntegrationTests.Data;

namespace ProjectManager.Tests.Utility;

public static class RequestBuilderExtensions
{
    public static async Task<RequestBuilder> AddAuthorizationsAsync(this RequestBuilder builder)
    {
        var provider = builder.TestServer.Services;
        await using var scope = provider.GetRequiredService<IServiceScopeFactory>()
            .CreateAsyncScope();
        provider = scope.ServiceProvider;
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();
        var authorizedUser = await userManager.FindByEmailAsync(TestAdmin.Email);
        if (!authorizedUser.HasValidRefreshToken())
        {
            await userManager.GenerateRefreshTokenAsync(authorizedUser);
        }

        var claims = await userManager.GetClaimsAsync(authorizedUser!);
        return builder.AddAuthorizations(claims);
    }

    public static RequestBuilder AddAuthorizations(this RequestBuilder builder,
        IEnumerable<Claim> claims)
    {
        var provider = builder.TestServer.Services;
        var jwtEngine = provider.GetRequiredService<IJwtEngine>();
        var accessToken = jwtEngine.WriteToken(claims);
        builder.AddHeader("Authorization", $"Bearer {accessToken}");
        return builder;
    }
}