using System.Security.Authentication;
using System.Security.Claims;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace ProjectManager.Core.Features.Authorization.Engines;

public interface IGoogleAuthenticationEngine
{
    Task<ClaimsIdentity> AuthenticateGoogleSsoAsync(string idToken);
}

public class GoogleAuthenticationEngine : IGoogleAuthenticationEngine
{
    private readonly GoogleOptions _configuration;

    public GoogleAuthenticationEngine(IOptions<GoogleOptions> options)
    {
        _configuration = options.Value;
    }
    public async Task<ClaimsIdentity> AuthenticateGoogleSsoAsync(string idToken)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                // Replace with your app's client ID
                Audience = new List<string> { $"{_configuration.ClientId}.apps.googleusercontent.com" }
            };

            payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
        catch (Exception ex)
        {
            throw new AuthenticationException(
                "Google SSO credentials could not be validated. See inner exception for details", ex);
        }
        
        if (!_configuration.ValidEmails.Contains(payload.Email))
        {
            throw new UnauthorizedAccessException("User is not authorized.");
        }
        return new ClaimsIdentity(new []
        {
            new Claim("google_id", payload.Subject, "String", ClaimsIdentity.DefaultIssuer, payload.Issuer),
            new Claim("name", payload.Name, "String", ClaimsIdentity.DefaultIssuer, payload.Issuer),
            new Claim("email", payload.Email, "String", ClaimsIdentity.DefaultIssuer, payload.Issuer)
        });
    }
}