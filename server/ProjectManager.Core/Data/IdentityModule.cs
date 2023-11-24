using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Common.DependencyInjection;
using ProjectManager.Data.Entities;

namespace ProjectManager.Data;

public class IdentityModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<ProjectManagerDbContext>()
            .AddDefaultTokenProviders();
    }
}