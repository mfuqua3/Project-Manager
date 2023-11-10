namespace ProjectManager.Core.Features.Authorization;

public class GoogleOptions
{
    public string ClientId { get; set; }
    public List<string> ValidEmails { get; set; }
}

public class JwtOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryTtlMinutes { get; set; }
    public string Secret { get; set; }
}

public class AuthorizationOptions
{
    public GoogleOptions Google { get; set; }
    public JwtOptions Jwt { get; set; }
}