using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Common.DependencyInjection;
using ProjectManager.Data.Entities;

namespace ProjectManager.Data;

public class IdentityModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddIdentityCore<AppUser>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<AppUser>()
            .AddEntityFrameworkStores<ProjectManagerDbContext>()
            .AddDefaultTokenProviders();
    }
}