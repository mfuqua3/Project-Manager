using System.Security.Claims;

namespace ProjectManager.Features.Authorization.Abstractions;

public interface IJwtEngine
{
    public string WriteToken(IEnumerable<Claim> claims);
}