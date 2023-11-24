using System;

namespace ProjectManager.Features.Projects.Domain.Results;

public class CreateProjectResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}