using System.Collections.Generic;
using System.Security.Claims;
using ProjectManager.Features.Users.Abstractions;

namespace ProjectManager.Features.Users.Domain.Common;

public sealed class GoogleLoginDto : ExternalLoginDto
{
    public GoogleLoginDto(string userId, IEnumerable<Claim> claims) : base(
        ProjectManagerAuthenticationDefaults.GoogleAuthenticationScheme, "Google SSO", userId, claims)
    {
    }
}