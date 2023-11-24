using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Common.DependencyInjection;
using ProjectManager.Common.Exceptions;

namespace ProjectManager.Data;
public class DataModule : Module<DataOptions>
{
    public override void ConfigureServices(IServiceCollection services, DataOptions options)
    {
        if (string.IsNullOrEmpty(options.ConnectionString))
        {
            throw new ProjectManagerConfigurationException("A connection string must be provided");
        }

        services.AddDbContextFactory<ProjectManagerDbContext>(opt=>opt.UseNpgsql(options.ConnectionString));
        services.AddModule<IdentityModule>();
    }
}