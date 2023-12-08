using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.Features.Users;
using ProjectManager.Tests.Utility;

namespace ProjectManager.Tests.IntegrationTests.Stubs;

public class GoogleIdTokenFactory
{
    private readonly string _audience;

    public GoogleIdTokenFactory(IOptions<GoogleOptions> options)
    {
        _audience = options.Value.ClientId;
    }

    public string GenerateFakeToken(ClaimsIdentity identity)
    {
        var key = SecretGenerator.GenerateSecret();
        var now = DateTime.UtcNow;
        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var jwt = new JwtSecurityToken(
            issuer: "https://accounts.google.com",
            audience: _audience,
            claims: identity.Claims,
            notBefore: now,
            expires: now.Add(TimeSpan.FromMinutes(60)),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}