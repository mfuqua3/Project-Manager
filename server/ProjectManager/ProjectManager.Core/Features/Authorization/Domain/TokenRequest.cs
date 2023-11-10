using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Core.Features.Authorization.Domain;

public class TokenRequest
{
    [Required]
    public string IdToken { get; set; }
}