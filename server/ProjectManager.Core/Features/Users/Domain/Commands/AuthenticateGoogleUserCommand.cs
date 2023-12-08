using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Features.Users.Domain.Commands;

public class AuthenticateGoogleUserCommand
{
    [Required]
    public string IdToken { get; set; }
}