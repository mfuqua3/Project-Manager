namespace ProjectManager.Features.Users;

public class AuthorizationOptions
{
    public GoogleOptions Google { get; set; }
    public JwtOptions Jwt { get; set; }
}