using System.Security.Claims;
using System.Threading.Tasks;
using ProjectManager.Features.Users.Domain.Common;

namespace ProjectManager.Features.Users.Abstractions;

public interface IGoogleAuthenticationEngine
{
    Task<GoogleLoginDto> AuthenticateGoogleSsoAsync(string idToken);
}