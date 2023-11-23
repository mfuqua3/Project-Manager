using System.Security.Claims;

namespace ProjectManager.Features.Authorization.Abstractions;

public interface IGoogleAuthenticationEngine
{
    Task<ClaimsIdentity> AuthenticateGoogleSsoAsync(string idToken);
}