using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Features.Projects.Domain.Commands;

public class CreateProjectCommand
{
    [Required]
    public string Name { get; set; }
}