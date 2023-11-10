using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ProjectManager.Core.Features.Authorization.Engines;

public interface IJwtEngine
{
    public string WriteToken(IEnumerable<Claim> claims);
}
public class JwtEngine : IJwtEngine
{
    private readonly IOptionsMonitor<JwtOptions> _jwtOptionsMonitor;

    public JwtEngine(IOptionsMonitor<JwtOptions> jwtOptionsMonitor)
    {
        _jwtOptionsMonitor = jwtOptionsMonitor;
    }
    public string WriteToken(IEnumerable<Claim> claims)
    {
        var configuration = _jwtOptionsMonitor.CurrentValue;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Secret));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create JwtSecurityToken object
        var token = new JwtSecurityToken(
            issuer: configuration.Issuer,
            audience: configuration.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(configuration.ExpiryTtlMinutes),
            signingCredentials: signingCredentials
        );

        // Generate token and send response
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}