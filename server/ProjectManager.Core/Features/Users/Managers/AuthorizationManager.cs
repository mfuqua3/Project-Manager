using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectManager.Common.Contracts;
using ProjectManager.Common.Exceptions;
using ProjectManager.Common.Extensions;
using ProjectManager.Data.Entities;
using ProjectManager.Features.Users.Abstractions;
using ProjectManager.Features.Users.Domain.Commands;
using ProjectManager.Features.Users.Domain.Common;
using ProjectManager.Features.Users.Domain.Results;
using ProjectManager.Features.Users.Utility;

namespace ProjectManager.Features.Users.Managers;

public class AuthorizationManager : IAuthorizationManager
{
    private readonly IGoogleAuthenticationEngine _googleAuthenticationEngine;
    private readonly IJwtEngine _jwtEngine;
    private readonly IUserCreationEngine _userCreationEngine;
    private readonly UserManager<AppUser> _userManager;

    public AuthorizationManager(
        IGoogleAuthenticationEngine googleAuthenticationEngine,
        IJwtEngine jwtEngine,
        IUserCreationEngine userCreationEngine,
        UserManager<AppUser> userManager)
    {
        _googleAuthenticationEngine = googleAuthenticationEngine;
        _jwtEngine = jwtEngine;
        _userCreationEngine = userCreationEngine;
        _userManager = userManager;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <exception cref=""></exception>
    /// <returns></returns>
    public async Task<AuthenticateUserResult> AuthenticateGoogleUserAsync(AuthenticateGoogleUserCommand command)
    {
        var googleLogin = await _googleAuthenticationEngine.AuthenticateGoogleSsoAsync(command.IdToken);
        var user = await _userManager.FindByLoginAsync(googleLogin);
        var userId = user?.Id;
        if (userId == null)
        {
            userId = await _userCreationEngine.CreateUserAsync(new NewUserDto
            {
                Name = googleLogin.Claims.Single(x => x.Type == ProjectManagerClaimTypes.Name).Value,
                Email = googleLogin.Claims.Single(x => x.Type == ProjectManagerClaimTypes.Email).Value,
                ExternalLogins = new List<ExternalLoginDto>
                {
                    googleLogin
                }
            });
        }

        var claims = await _userManager.GetClaimsAsync(new AppUser { Id = userId });
        var jwt = _jwtEngine.WriteToken(claims);
        var refreshToken = await _userManager.GenerateRefreshTokenAsync(user);
        return new AuthenticateUserResult
        {
            AccessToken = jwt,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthenticateUserResult> RefreshAuthenticatedUserAsync(RefreshAuthenticatedUserCommand command)
    {
        var user = await _userManager.FindByIdAsync(command.UserId);
        if (user == null)
        {
            throw new ProjectManagerDataNotFoundException("No user with that ID could be found");
        }

        var canRefresh = user.ValidateRefreshAgainst(command.RefreshToken);
        if (!canRefresh)
        {
            throw new ProjectManagerBadRequestException("Invalid Refresh Token");
        }

        var claims = await _userManager.GetClaimsAsync(user);
        var refreshToken = await _userManager.GenerateRefreshTokenAsync(user);
        var jwt = _jwtEngine.WriteToken(claims);
        return new AuthenticateUserResult
        {
            RefreshToken = refreshToken,
            AccessToken = jwt
        };
    }

    public async Task<SignOutUserResult> SignOutUserAsync(SignoutUserCommand command)
    {
        var user = await _userManager.FindByIdAsync(command.UserId);
        if (user == null)
        {
            throw new ProjectManagerDataNotFoundException("No user with that ID could be found");
        }

        await _userManager.RevokeRefreshTokenAsync(user);
        return Result<SignOutUserResult>.Success();
    }
}