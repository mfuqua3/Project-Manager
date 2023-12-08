using System.Collections.Generic;

namespace ProjectManager.Features.Users.Domain.Common;

public class NewUserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public List<ExternalLoginDto> ExternalLogins { get; set; } = new();
}