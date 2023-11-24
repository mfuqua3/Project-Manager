using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectManager.Features.Authorization.Abstractions;

public interface IGoogleAuthenticationEngine
{
    Task<ClaimsIdentity> AuthenticateGoogleSsoAsync(string idToken);
}