using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ProjectManager.Common.Exceptions;
using ProjectManager.Common.Extensions;
using ProjectManager.Data.Entities;
using ProjectManager.Features.Authorization.Abstractions;
using ProjectManager.Features.Authorization.Domain.Commands;
using ProjectManager.Features.Authorization.Domain.Common;
using ProjectManager.Features.Authorization.Domain.Results;

namespace ProjectManager.Features.Authorization.Managers;

public class AuthorizationManager : IAuthorizationManager
{
    private readonly IGoogleAuthenticationEngine _googleAuthenticationEngine;
    private readonly IJwtEngine _jwtEngine;
    private readonly UserManager<AppUser> _userManager;

    public AuthorizationManager(
        IGoogleAuthenticationEngine googleAuthenticationEngine,
        IJwtEngine jwtEngine,
        UserManager<AppUser> userManager)
    {
        _googleAuthenticationEngine = googleAuthenticationEngine;
        _jwtEngine = jwtEngine;
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
        var claimsIdentity = await _googleAuthenticationEngine.AuthenticateGoogleSsoAsync(command.IdToken);
        var googleId = claimsIdentity.Claims.Single(x => x.Type == AppClaimTypes.GoogleId).Value;
        var user = await _userManager.FindByLoginAsync("Google", googleId);
        if (user == null)
        {
            var name = claimsIdentity.Claims.Single(x => x.Type == AppClaimTypes.Name).Value;
            var email = claimsIdentity.Claims.Single(x => x.Type == AppClaimTypes.Email).Value;
            user = new AppUser { Email = email, EmailConfirmed = true, UserName = email, Name = name };
            await _userManager.CreateAsync(user);
            await _userManager.AddLoginAsync(user,
                new ExternalLoginInfo(new ClaimsPrincipal(claimsIdentity), "Google", googleId, "Google SSO"));
            await _userManager.AddClaimsAsync(user, claimsIdentity.Claims.Append(new Claim(AppClaimTypes.Id, user.Id)));
        }

        claimsIdentity.AddClaim(new Claim(AppClaimTypes.Id, user.Id));
        var jwt = _jwtEngine.WriteToken(claimsIdentity.Claims);
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

    public async Task SignOutUserAsync(SignoutUserCommand command)
    {
        var user = await _userManager.FindByIdAsync(command.UserId);
        if (user == null)
        {
            throw new ProjectManagerDataNotFoundException("No user with that ID could be found");
        }

        await _userManager.RevokeRefreshTokenAsync(user);
    }
}