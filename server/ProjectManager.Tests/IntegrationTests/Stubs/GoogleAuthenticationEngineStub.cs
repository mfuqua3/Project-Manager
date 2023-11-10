using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ProjectManager.Core.Features.Authorization.Engines;

namespace ProjectManager.Tests.IntegrationTests.Stubs;

public class GoogleAuthenticationEngineStub : IGoogleAuthenticationEngine
{
    public Task<ClaimsIdentity> AuthenticateGoogleSsoAsync(string idToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        // Read the token without validating it
        var jwtToken = tokenHandler.ReadJwtToken(idToken);

        return Task.FromResult(new ClaimsIdentity(new[]
        {
            new Claim("google_id", jwtToken.Subject, "String", ClaimsIdentity.DefaultIssuer, jwtToken.Issuer),
            new Claim("name", jwtToken.Claims.First(x => x.Type == "name").Value, "String",
                ClaimsIdentity.DefaultIssuer, jwtToken.Issuer),
            new Claim("email", jwtToken.Claims.First(x => x.Type == "email").Value, "String",
                ClaimsIdentity.DefaultIssuer, jwtToken.Issuer)
        }));
    }
}