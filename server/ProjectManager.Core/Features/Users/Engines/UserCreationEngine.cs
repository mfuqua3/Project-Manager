using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectManager.Data.Entities;
using ProjectManager.Features.Users.Abstractions;
using ProjectManager.Features.Users.Domain.Common;
using ProjectManager.Features.Users.Utility;

namespace ProjectManager.Features.Users.Engines;

class UserCreationEngine : IUserCreationEngine
{
    private readonly UserManager<AppUser> _userManager;

    public UserCreationEngine(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> CreateUserAsync(NewUserDto newUser)
    {
        var user = new AppUser
            { Email = newUser.Email, EmailConfirmed = true, UserName = newUser.Email, Name = newUser.Name };
        await _userManager.CreateAsync(user);
        foreach (var externalLogin in newUser.ExternalLogins)
        {
            await _userManager.AddLoginAsync(user,
                new ExternalLoginInfo(
                    new ClaimsPrincipal(new ClaimsIdentity(externalLogin.Claims)),
                    externalLogin.Name,
                    externalLogin.UserId,
                    externalLogin.DisplayName));
        }

        var claimsIdentity = new ProjectManagerClaimsIdentityBuilder()
            .AddNameClaim(newUser.Name)
            .AddEmailClaim(newUser.Email)
            .AddSubjectClaim(user.Id)
            .Build();
        await _userManager.AddClaimsAsync(user, claimsIdentity.Claims);
        return user.Id;
    }
}