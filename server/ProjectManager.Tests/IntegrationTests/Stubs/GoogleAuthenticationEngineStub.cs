using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ProjectManager.Features.Users.Abstractions;
using ProjectManager.Features.Users.Domain.Common;

namespace ProjectManager.Tests.IntegrationTests.Stubs;

public class GoogleAuthenticationEngineStub : IGoogleAuthenticationEngine
{
    public Task<GoogleLoginDto> AuthenticateGoogleSsoAsync(string idToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        // Read the token without validating it
        var jwtToken = tokenHandler.ReadJwtToken(idToken);
        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim("google_id", jwtToken.Subject, "String", ClaimsIdentity.DefaultIssuer, jwtToken.Issuer),
            new Claim("name", jwtToken.Claims.First(x => x.Type == "name").Value, "String",
                ClaimsIdentity.DefaultIssuer, jwtToken.Issuer),
            new Claim("email", jwtToken.Claims.First(x => x.Type == "email").Value, "String",
                ClaimsIdentity.DefaultIssuer, jwtToken.Issuer)
        });
        return Task.FromResult(new GoogleLoginDto(jwtToken.Subject, claimsIdentity.Claims));
    }
}