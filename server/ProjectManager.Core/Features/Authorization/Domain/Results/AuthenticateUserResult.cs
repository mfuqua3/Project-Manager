using System.Text.Json.Serialization;

namespace ProjectManager.Features.Authorization.Domain.Results;

public class AuthenticateUserResult
{
    public string AccessToken { get; set; }
    [JsonIgnore]
    public string RefreshToken { get; set; }
}