namespace ProjectManager.Core.Features.Authorization.Domain;

public class RefreshRequest
{
    public string RefreshToken { get; set; }
    public string UserId { get; set; }
}