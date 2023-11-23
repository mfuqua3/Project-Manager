using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Features.Authorization.Domain.Commands;

public class AuthenticateGoogleUserCommand
{
    [Required]
    public string IdToken { get; set; }
}