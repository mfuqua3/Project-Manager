using System.Collections.Generic;

namespace ProjectManager.Features.Users;

public class GoogleOptions
{
    public string ClientId { get; set; }
    public List<string> ValidEmails { get; set; }
}