using System.Collections.Generic;
using System.Security.Claims;

namespace ProjectManager.Features.Users.Abstractions;

public interface IJwtEngine
{
    public string WriteToken(IEnumerable<Claim> claims);
}