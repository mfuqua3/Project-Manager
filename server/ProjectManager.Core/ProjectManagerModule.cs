using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Common.DependencyInjection;
using ProjectManager.Data;
using ProjectManager.Features.Projects;
using ProjectManager.Features.Users;

namespace ProjectManager;

public class ProjectManagerModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(ProjectManagerApplication.Assembly);
        services.AddModule<DataModule>();
        services.AddModule<AuthorizationModule>();
        services.AddModule<ProjectsModule>();
    }
}