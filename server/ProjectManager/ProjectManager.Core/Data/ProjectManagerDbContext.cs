using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Core.Data.Entities;
using ProjectManager.Core.Utility.Extensions;

namespace ProjectManager.Core.Data;

public class ProjectManagerDbContext : IdentityDbContext<AppUser>
{
    public ProjectManagerDbContext(DbContextOptions<ProjectManagerDbContext> options): base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectManagerDbContext).Assembly);
        modelBuilder.ApplySoftDeleteQueryFilters();
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        this.ProcessCustomerInterfaces();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}