using ProjectManager.Core.Data;
using ProjectManager.Core.Features.Authorization;
using ProjectManager.Core.Utility.DependencyInjection;

namespace ProjectManager.Core;

public class ProjectManagerModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddModule<DataModule>();
        services.AddModule<AuthorizationModule>();
    }
}