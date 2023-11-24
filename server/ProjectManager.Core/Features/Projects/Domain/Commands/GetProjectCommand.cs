using System;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Features.Projects.Domain.Commands;

public class GetProjectCommand
{
    [FromRoute]
    public Guid Id { get; set; }
}