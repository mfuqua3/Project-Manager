using Microsoft.AspNetCore.Mvc;
using ProjectManager.Common.Contracts;
using ProjectManager.Common.Exceptions;
using ProjectManager.Features.Projects.Abstractions;
using ProjectManager.Features.Projects.Domain.Commands;
using ProjectManager.Features.Projects.Domain.Common;
using ProjectManager.Features.Projects.Domain.Results;

namespace ProjectManager.WebApi.Controllers;

/// <summary>
/// Controls Project configuration workflows
/// </summary>
public class ProjectsController : ApiController
{
    private readonly IProjectsManager _projectsManager;
    /// <summary>
    /// Initializes the controller
    /// </summary>
    /// <param name="projectsManager"></param>
    public ProjectsController(IProjectsManager projectsManager)
    {
        _projectsManager = projectsManager;
    }
    /// <summary>
    /// Gets a paginated list of projects
    /// </summary>
    /// <param name="command">The request</param>
    /// <returns>The paginated response.</returns>
    /// <response code="200">The paginated list of projects.</response>
    /// <exception cref="ProjectManagerBadRequestException">Throw for a malformed paginated request.</exception>
    [HttpGet]
    public async Task<ActionResult<PagedList<ProjectListItemDto>>> GetProjectListAsync([FromQuery]GetProjectListCommand command) 
        => Ok(await _projectsManager.GetProjectListAsync(command));
    /// <summary>
    /// Creates a new projects
    /// </summary>
    /// <param name="command">The creation parameters</param>
    /// <returns></returns>
    /// <response code="201">The created project.</response>
    /// <exception cref="ProjectManagerBadRequestException">Thrown for disallowed parameter, conflict, or if a required field isn't supplied.</exception>
    [HttpPost]
    public async Task<ActionResult<CreateProjectResult>> CreateProjectAsync(CreateProjectCommand command)
    {
        var result = await _projectsManager.CreateProjectAsync(command);
        return Created(Url.Action("GetProject", new { Id = result.Id }) ?? string.Empty, result);
    }
    /// <summary>
    /// Fetches the details of a specific project
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <response code="200">The requested project.</response>
    /// <exception cref="ProjectManagerDataNotFoundException">Thrown if the requested project does not exist.</exception>
    [HttpGet("{Id}")]
    public async Task<ActionResult<GetProjectResult>> GetProjectAsync(GetProjectCommand command) => 
        Ok(await _projectsManager.GetProjectAsync(command));
    /// <summary>
    /// Deletes a specific project
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <response code="204">Successful deletion.</response>
    /// <exception cref="ProjectManagerDataNotFoundException">Thrown if the requested project does not exist.</exception>
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteProjectAsync(DeleteProjectCommand command)
    {
        await _projectsManager.DeleteProjectAsync(command);
        return NoContent();
    }
}