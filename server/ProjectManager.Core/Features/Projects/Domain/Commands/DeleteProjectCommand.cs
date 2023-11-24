using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Features.Projects.Domain.Commands;

public class DeleteProjectCommand
{
    [FromRoute, Required]
    public Guid Id { get; set; }
}