using System.Text.Json.Serialization;

namespace ProjectManager.Core.Features.Authorization.Domain;

public class TokenResponse
{
    public string AccessToken { get; set; }
    [JsonIgnore]
    public string RefreshToken { get; set; }
}