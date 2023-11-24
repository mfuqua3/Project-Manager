using System.Threading.Tasks;
using ProjectManager.Features.Authorization.Domain.Commands;
using ProjectManager.Features.Authorization.Domain.Results;

namespace ProjectManager.Features.Authorization.Abstractions;

public interface IAuthorizationManager
{
    Task<AuthenticateUserResult> AuthenticateGoogleUserAsync(AuthenticateGoogleUserCommand command);
    Task<AuthenticateUserResult> RefreshAuthenticatedUserAsync(RefreshAuthenticatedUserCommand command);
    Task<SignOutUserResult> SignOutUserAsync(SignoutUserCommand command);
}