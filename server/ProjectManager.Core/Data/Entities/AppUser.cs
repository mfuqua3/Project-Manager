using Microsoft.AspNetCore.Identity;
using ProjectManager.Common.Contracts;

namespace ProjectManager.Data.Entities;

public class AppUser : IdentityUser, IRefreshToken, INamed
{
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string Name { get; set; }
}