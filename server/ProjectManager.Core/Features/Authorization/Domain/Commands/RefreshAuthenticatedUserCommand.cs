﻿namespace ProjectManager.Features.Authorization.Domain.Commands;

public class RefreshAuthenticatedUserCommand
{
    public string RefreshToken { get; set; }
    public string UserId { get; set; }
}