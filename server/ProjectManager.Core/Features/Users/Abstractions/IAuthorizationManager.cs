using System.Threading.Tasks;
using ProjectManager.Features.Users.Domain.Commands;
using ProjectManager.Features.Users.Domain.Results;

namespace ProjectManager.Features.Users.Abstractions;

public interface IAuthorizationManager
{
    Task<AuthenticateUserResult> AuthenticateGoogleUserAsync(AuthenticateGoogleUserCommand command);
    Task<AuthenticateUserResult> RefreshAuthenticatedUserAsync(RefreshAuthenticatedUserCommand command);
    Task<SignOutUserResult> SignOutUserAsync(SignoutUserCommand command);
}