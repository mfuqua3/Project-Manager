namespace ProjectManager.WebApi;

public class HostingOptions
{
    public CorsHostingOptions Cors { get; set; }
}

public class CorsHostingOptions
{
    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
}