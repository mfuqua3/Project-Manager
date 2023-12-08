using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectManager.Data.Entities;
using ProjectManager.Features.Users.Domain.Common;

namespace ProjectManager.Features.Users.Utility;

public static class UserManagerExtensions
{
    public static async Task<T> FindByLoginAsync<T>(this UserManager<T> userManager, ExternalLoginDto loginDto)
        where T : class =>
        await userManager.FindByLoginAsync(loginDto.Name, loginDto.UserId);
}