namespace ProjectManager.Features.Authorization;

public class JwtOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryTtlMinutes { get; set; }
    public string Secret { get; set; }
}