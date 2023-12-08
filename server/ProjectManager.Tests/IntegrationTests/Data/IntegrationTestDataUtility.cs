using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Data.Entities;
using ProjectManager.Features.Users.Domain.Common;

namespace ProjectManager.Tests.IntegrationTests.Data;

public class IntegrationTestDataUtility
{
    private readonly ProjectManagerDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public IntegrationTestDataUtility(ProjectManagerDbContext dbContext, UserManager<AppUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task InitializeDatabaseAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.MigrateAsync();
    }

    public async Task SetUpTestUsers()
    {
        var adminUser = TestAdmin.Create();
        await _userManager.CreateAsync(adminUser);
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ProjectManagerClaimTypes.Id, adminUser.Id),
            new Claim(ProjectManagerClaimTypes.Email, adminUser.Email!),
            new Claim(ProjectManagerClaimTypes.Name, adminUser.Name)
        });
        await _userManager.AddClaimsAsync(adminUser, identity.Claims);
    }

    public async Task TearDownAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
    }
}