using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Common.DependencyInjection;
using ProjectManager.Features.Projects.Abstractions;
using ProjectManager.Features.Projects.Managers;

namespace ProjectManager.Features.Projects;

public class ProjectsModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IProjectsManager, ProjectsManager>();
    }
}