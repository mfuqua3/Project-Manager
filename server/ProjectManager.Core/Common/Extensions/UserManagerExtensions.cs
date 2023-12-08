using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectManager.Data.Entities;

namespace ProjectManager.Common.Extensions;

/// <summary>
/// Extension methods for UserManager.
/// </summary>
public static class UserManagerExtensions
{
    /// <summary>
    /// Generates a new refresh token for an application user.
    /// </summary>
    /// <param name="userManager">The UserManager.</param>
    /// <param name="user">The AppUser.</param>
    /// <returns>A new refresh token.</returns>
    public static async Task<string> GenerateRefreshTokenAsync(this UserManager<AppUser> userManager, AppUser user)
    {
        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow + TimeSpan.FromDays(14);
        await userManager.UpdateAsync(user);
        return user.RefreshToken;
    }

    /// <summary>
    /// Revokes the user's existing refresh token.
    /// </summary>
    /// <param name="userManager">The UserManager.</param>
    /// <param name="user">The AppUser.</param>
    public static async Task RevokeRefreshTokenAsync(this UserManager<AppUser> userManager, AppUser user)
    {
        user.RefreshToken = null;
        await userManager.UpdateAsync(user);
    }

    /// <summary>
    /// Generates a new refresh token.
    /// </summary>
    /// <returns>A new string of random numbers converted to Base64.</returns>
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}