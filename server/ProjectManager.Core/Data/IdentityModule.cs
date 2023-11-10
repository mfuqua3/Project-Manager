using Microsoft.AspNetCore.Identity;
using ProjectManager.Core.Data.Entities;
using ProjectManager.Core.Utility.DependencyInjection;

namespace ProjectManager.Core.Data;

public class IdentityModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<ProjectManagerDbContext>()
            .AddDefaultTokenProviders();
    }
}