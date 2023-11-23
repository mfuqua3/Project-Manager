using ProjectManager.Common.DependencyInjection;
using ProjectManager.Data;
using ProjectManager.Features.Authorization;

namespace ProjectManager;

public class ProjectManagerModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddModule<DataModule>();
        services.AddModule<AuthorizationModule>();
    }
}