using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProjectManager.Features.Users.Abstractions;

namespace ProjectManager.Tests.Utility;

public class AuthorizationsDelegatingHandler : DelegatingHandler
{
    private readonly IJwtEngine _jwtEngine;
    private readonly IEnumerable<Claim> _claims;

    public AuthorizationsDelegatingHandler(IEnumerable<Claim> claims, IJwtEngine jwtEngine,  HttpMessageHandler inner) :
        base(inner)
    {
        _jwtEngine = jwtEngine;
        _claims = claims;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = _jwtEngine.WriteToken(_claims);
        request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
        return base.SendAsync(request, cancellationToken);
    }
}