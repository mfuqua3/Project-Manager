using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using ProjectManager.Common.Extensions;
using ProjectManager.Data.Entities;
using ProjectManager.Tests.Infrastructure;
using ProjectManager.Tests.IntegrationTests.Data;
using Shouldly;

namespace ProjectManager.Tests.IntegrationTests.Features.Users;

[TestFixture]
public class OnGetSignout : IntegrationTest
{
    [OneTimeSetUp]
    public async Task ArrangeAndAct()
    {
        var userManager = GetService<UserManager<AppUser>>();
        var user = await userManager.FindByEmailAsync(TestAdmin.Email);
        var claims = await userManager.GetClaimsAsync(user!);
        await userManager.GenerateRefreshTokenAsync(user);
        var client = CreateClient();
        await client.GetAsync("api/Authorizations/signout");
    }
    [Test]
    public async Task RevokesRefreshToken()
    {
        var userManager = GetService<UserManager<AppUser>>();
        var user = await userManager.FindByEmailAsync(TestAdmin.Email);
        user!.RefreshToken.ShouldBeNull();
    }
}