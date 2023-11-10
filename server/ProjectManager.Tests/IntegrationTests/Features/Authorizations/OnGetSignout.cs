using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using ProjectManager.Core.Data.Entities;
using ProjectManager.Core.Utility.Extensions;
using ProjectManager.Tests.Infrastructure;
using ProjectManager.Tests.IntegrationTests.Data;
using ProjectManager.Tests.Utility;
using Shouldly;

namespace ProjectManager.Tests.IntegrationTests.Features.Authorizations;

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
        var request = CreateRequest("Authorizations/signout")
            .AddAuthorizations(claims);
        await request.GetAsync();
    }
    [Test]
    public async Task RevokesRefreshToken()
    {
        var userManager = GetService<UserManager<AppUser>>();
        var user = await userManager.FindByEmailAsync(TestAdmin.Email);
        user!.RefreshToken.ShouldBeNull();
    }
}