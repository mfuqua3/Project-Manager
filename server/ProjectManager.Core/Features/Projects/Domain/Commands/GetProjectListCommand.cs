using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Common.Contracts;

namespace ProjectManager.Features.Projects.Domain.Commands;

public class GetProjectListCommand : IPaginated
{
    [FromQuery, Range(0, int.MaxValue)]
    public int Page { get; set; }
    [FromQuery, Range(0, int.MaxValue)]
    public int PageSize { get; set; }
}