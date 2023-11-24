using System.Threading.Tasks;
using ProjectManager.Common.Contracts;
using ProjectManager.Features.Projects.Domain.Commands;
using ProjectManager.Features.Projects.Domain.Common;
using ProjectManager.Features.Projects.Domain.Results;

namespace ProjectManager.Features.Projects.Abstractions;

public interface IProjectsManager
{
    Task<PagedList<ProjectListItemDto>> GetProjectListAsync(GetProjectListCommand command);
    Task<CreateProjectResult> CreateProjectAsync(CreateProjectCommand command);
    Task<DeleteProjectResult> DeleteProjectAsync(DeleteProjectCommand command);
    Task<GetProjectResult> GetProjectAsync(GetProjectCommand command);
}