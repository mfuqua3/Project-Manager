using System.Collections.Generic;
using System.Security.Claims;

namespace ProjectManager.Features.Users.Domain.Common;

public class ExternalLoginDto
{
    public ExternalLoginDto(string name, string displayName, string userId, IEnumerable<Claim> claims)
    {
        Name = name;
        DisplayName = displayName;
        UserId = userId;
        Claims = claims;
    }

    public ExternalLoginDto(string name, string userId, IEnumerable<Claim> claims) : this(name, name, userId, claims)
    {
    }

    public string Name { get; }
    public string DisplayName { get; }
    public IEnumerable<Claim> Claims { get; }
    public string UserId { get; }
}